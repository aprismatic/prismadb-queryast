using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Operation : Expression
    {
        protected Expression _left, _right;

        public Expression left
        {
            get { return _left; }
            set
            {
                _left = value;
                _left.Parent = this;
            }
        }

        public Expression right
        {
            get { return _right; }
            set
            {
                _right = value;
                _right.Parent = this;
            }
        }
    }

    public class Addition : Operation
    {
        public Addition(Expression left, Expression right)
        {
            setValue(left, right, "");
        }

        public Addition(Expression left, Expression right, string aliasName)
        {
            setValue(left, right, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Addition constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                Alias = new Identifier((string)value[2]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new Addition(left_clone, right_clone, Alias.id);
            clone.Parent = this.Parent;

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return (int)left.Eval(r) + (int)right.Eval(r);
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.AdditionToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Addition otherA)) return false;

            return (this.Alias.Equals(otherA.Alias))
                && (this.left.Equals(otherA.left))
                && (this.right.Equals(otherA.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }
    }

    public class Subtraction : Operation
    {
        public Subtraction(Expression left, Expression right)
        {
            setValue(left, right, "");
        }

        public Subtraction(Expression left, Expression right, string aliasName)
        {
            setValue(left, right, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Subtraction constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                Alias = new Identifier((string)value[2]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new Subtraction(left_clone, right_clone, Alias.id);
            clone.Parent = this.Parent;

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return (int)left.Eval(r) - (int)right.Eval(r);
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.SubtractionToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Subtraction otherS)) return false;

            return (this.Alias.Equals(otherS.Alias))
                && (this.left.Equals(otherS.left))
                && (this.right.Equals(otherS.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }
    }

    public class Multiplication : Operation
    {
        public Multiplication(Expression left, Expression right)
        {
            setValue(left, right, "");
        }

        public Multiplication(Expression left, Expression right, string aliasName)
        {
            setValue(left, right, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Multiplication constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                Alias = new Identifier((string)value[2]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Multiplication(left_clone as Expression, right_clone as Expression, Alias.id);
            clone.Parent = this.Parent;

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return (int)left.Eval(r) * (int)right.Eval(r);
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.MultiplicationToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Multiplication otherM)) return false;

            return (this.Alias.Equals(otherM.Alias))
                && (this.left.Equals(otherM.left))
                && (this.right.Equals(otherM.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }
    }

    public class Division : Operation
    {
        public Division(Expression left, Expression right)
        {
            setValue(left, right, "");
        }

        public Division(Expression left, Expression right, string aliasName)
        {
            setValue(left, right, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Division constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                Alias = new Identifier((string)value[2]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Division(left_clone as Expression, right_clone as Expression, Alias.id);
            clone.Parent = this.Parent;

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return (int)left.Eval(r) / (int)right.Eval(r);
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DivisionToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Division otherD)) return false;

            return (this.Alias.Equals(otherD.Alias))
                && (this.left.Equals(otherD.left))
                && (this.right.Equals(otherD.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }
    }
}
