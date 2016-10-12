using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
        public override Expression Clone() { return new BooleanTrue(NOT); }
        public override object Eval(DataRow r) { return !NOT; }
        public override List<ColumnRef> GetColumns() { return new List<ColumnRef>(); }
        public override string ToString() { return NOT ? "(1<>1)" : "(1=1)"; }
    }

    public class BooleanIn : BooleanExpression
    {
        public ColumnRef Column;
        public List<Constant> InValues;

        public BooleanIn()
        {
            ColumnName = "";
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

        public override Expression Clone()
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

        public override object Eval(DataRow r)
        {
            return InValues.Select(x => x.ToString()).Contains(r[Column.ColumnName].ToString()) ? !NOT : NOT;
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
            if (InValues.Count == 0)
            {
                var BoolFalse = new BooleanTrue(true);
                return BoolFalse.ToString();
            }

            var sb = new StringBuilder();

            if (NOT)
                sb.Append(" NOT ");

            sb.Append(" ( ");

            sb.Append(Column.ToString());
            sb.Append(" IN (");
            sb.Append(String.Join(" , ", InValues));
            sb.Append(" ) ");

            sb.Append(" ) ");

            return sb.ToString();
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

        public override Expression Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new BooleanEquals(left_clone, right_clone, NOT);

            return clone;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (NOT)
                sb.Append(" NOT");

            sb.Append(" ( ");
            sb.Append(left.ToString());
            sb.Append(" = ");
            sb.Append(right.ToString());
            sb.Append(" ) ");

            return sb.ToString();
        }

        public override object Eval(DataRow r)
        {
            bool res;

            var leftEval = left.Eval(r);
            var rightEval = right.Eval(r);

            if (leftEval is string && rightEval is string)
            {
                res = ((string)leftEval == (string)rightEval) ? !NOT : NOT;
            }
            else if (leftEval is Int32 && rightEval is Int32)
            {
                res = ((int)leftEval == (int)rightEval) ? !NOT : NOT; // assume data in DataRow are in int
            }
            else
            {
                throw new ApplicationException("Left and right expressions of BooleanEquals are not of the same type");
            }

            return res;
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
