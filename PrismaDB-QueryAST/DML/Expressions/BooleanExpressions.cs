using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public abstract class BooleanExpression : Expression
    {
        public bool NOT = false;
    }

    public class BooleanTrue : BooleanExpression
    {
        public BooleanTrue() { setValue(false); }
        public BooleanTrue(bool NOT) { setValue(NOT); }
        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanTrue.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else
                throw new ArgumentException("BooleanTrue.setValue expects no more than one argument");
        }
        public override object Clone() { return new BooleanTrue(NOT); }
        public override object Eval(DataRow r) { return !NOT; }
        public override List<ColumnRef> GetColumns() { return new List<ColumnRef>(); }
        public override string ToString() { return DialectResolver.Dialect.BooleanTrueToString(this); }
        public override bool Equals(object other)
        {
                && (NOT == (other as BooleanTrue)?.NOT);
        }
        public override int GetHashCode() { return unchecked(ColumnName.GetHashCode() * (NOT.GetHashCode() + 1)); }
    }

    public class BooleanIn : BooleanExpression
    {
        public ColumnRef Column;
        public List<Constant> InValues;

        public BooleanIn()
        {
            ColumnName = new Identifier("");
            Column = new ColumnRef("");
            InValues = new List<Constant>();
            NOT = false;
        }

        public BooleanIn(ColumnRef column, params Constant[] values)
            : this()
        {
            Column = (ColumnRef)column.Clone();
            InValues.AddRange(values);
        }

        public BooleanIn(bool NOT, ColumnRef column, params Constant[] values)
            : this(column, values)
        {
            this.NOT = NOT;
        }

        public override void setValue(params object[] value)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            var res = new BooleanIn
            {
                Column = Column,
                NOT = NOT
            };
            foreach (var v in InValues)
                res.InValues.Add((Constant)v.Clone());
            return res;
        }

        public override object Eval(DataRow r)  // TODO: Check for correctness
        {
            return InValues.Select(x => x.ToString()).Contains(r[Column.ColumnName.id].ToString()) ? !NOT : NOT;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>()
            {
                (ColumnRef)(Column.Clone())
            };
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanInToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanIn otherBI)) return false;

            return (this.NOT != otherBI.NOT)
                && (this.ColumnName == otherBI.ColumnName)
                && (this.Column.Equals(otherBI.Column))
                && (this.InValues.All(x => otherBI.InValues.Exists(y => x.Equals(y))));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                ColumnName.GetHashCode() *
                Column.GetHashCode() *
                InValues.Aggregate(1, (x, y) => unchecked(x * y.GetHashCode())));
        }
    }

    public class BooleanEquals : BooleanExpression
    {
        public Expression left, right;

        public BooleanEquals(Expression left, Expression right)
        {
            setValue(left, right);
        }

        public BooleanEquals(Expression left, Expression right, bool NOT)
        {
            setValue(left, right, NOT);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanEquals.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else if (value.Length == 2)
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
            }
            else
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
                NOT = (bool)value[2];
            }
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new BooleanEquals(left_clone, right_clone, NOT);

            return clone;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanEqualsToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanEquals otherBE)) return false;

            return (this.NOT != otherBE.NOT)
                && (this.ColumnName == otherBE.ColumnName)
                && (this.left.Equals(otherBE.left))
                && (this.right.Equals(otherBE.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                ColumnName.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }

        public override object Eval(DataRow r)
        {
            var leftEval = left.Eval(r);
            var rightEval = right.Eval(r);

            if (leftEval is String && rightEval is String)
            {
                return ((String)leftEval == (String)rightEval) ? !NOT : NOT;
            }
            else if (leftEval is Int32 leftInt)
            {
                if (rightEval is Int32 rightInt)
                    return (leftInt == rightInt) ? !NOT : NOT; // assume data in DataRow are in int/double
                else if (rightEval is Double rightDouble)
                    return (leftInt == rightDouble) ? !NOT : NOT; // assume data in DataRow are in int/double
            }
            else if (leftEval is Double leftDouble)
            {
                if (rightEval is Int32 rightInt)
                    return (leftDouble == rightInt) ? !NOT : NOT; // assume data in DataRow are in int/double
                else if (rightEval is Double rightDouble)
                    return (leftDouble == rightDouble) ? !NOT : NOT; // assume data in DataRow are in int/double
            }

            throw new ApplicationException(
                 "Left and right expressions of BooleanEquals are not of the same type.\n" +
                $"Left expression is \"{left}\" of type {left.GetType()}\n" +
                $"Left expression evaluates to \"{leftEval}\" of type {leftEval.GetType()}\n" +
                $"Right expression is \"{right}\" of type {right.GetType()}\n" +
                $"Right expression evaluates to \"{rightEval}\" of type {rightEval.GetType()}");
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }
    }
}
