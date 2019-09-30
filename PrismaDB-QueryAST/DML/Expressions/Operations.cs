using Microsoft.CSharp.RuntimeBinder;
using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Operation : Expression
    {
        protected Expression _left, _right;

        public Expression left
        {
            get => _left;
            set
            {
                _left = value;
                _left.Parent = this;
            }
        }

        public Expression right
        {
            get => _right;
            set
            {
                _right = value;
                _right.Parent = this;
            }
        }

        [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
        public override bool UpdateChild(Expression child, Expression newChild)
        {
            if (child == left)
            {
                left = newChild;
                return true;
            }
            if (child == right)
            {
                right = newChild;
                return true;
            }

            return false;
        }
    }

    public class Addition : Operation
    {
        public Addition(Expression left, Expression right, string aliasName = "")
        {
            this.left = left;
            this.right = right;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new Addition(left_clone, right_clone, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            try { return (dynamic)left.Eval(r) + (dynamic)right.Eval(r); }
            catch (RuntimeBinderException) { return Convert.ToDecimal(left.Eval(r)) + Convert.ToDecimal(right.Eval(r)); }
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ConstantContainer> GetConstants()
        {
            var res = new List<ConstantContainer>();
            res.AddRange(left.GetConstants());
            res.AddRange(right.GetConstants());
            return res;
        }

        public override string ToString() => DialectResolver.Dialect.AdditionToString(this);

        public override bool Equals(object other)
        {
            if (!(other is Addition otherA)) return false;

            return Alias.Equals(otherA.Alias)
                && left.Equals(otherA.left)
                && right.Equals(otherA.right);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      left.GetHashCode() *
                      right.GetHashCode());
    }

    public class Subtraction : Operation
    {
        public Subtraction(Expression left, Expression right, string aliasName = "")
        {
            this.left = left;
            this.right = right;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new Subtraction(left_clone, right_clone, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            try { return (dynamic)left.Eval(r) - (dynamic)right.Eval(r); }
            catch (RuntimeBinderException) { return Convert.ToDecimal(left.Eval(r)) - Convert.ToDecimal(right.Eval(r)); }
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ConstantContainer> GetConstants()
        {
            var res = new List<ConstantContainer>();
            res.AddRange(left.GetConstants());
            res.AddRange(right.GetConstants());
            return res;
        }

        public override string ToString() => DialectResolver.Dialect.SubtractionToString(this);

        public override bool Equals(object other)
        {
            if (!(other is Subtraction otherS)) return false;

            return Alias.Equals(otherS.Alias)
                && left.Equals(otherS.left)
                && right.Equals(otherS.right);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      left.GetHashCode() *
                      right.GetHashCode());
    }

    public class Multiplication : Operation
    {
        public Multiplication(Expression left, Expression right, string aliasName = "")
        {
            this.left = left;
            this.right = right;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Multiplication(left_clone as Expression, right_clone as Expression, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            try { return (dynamic)left.Eval(r) * (dynamic)right.Eval(r); }
            catch (RuntimeBinderException) { return Convert.ToDecimal(left.Eval(r)) * Convert.ToDecimal(right.Eval(r)); }
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ConstantContainer> GetConstants()
        {
            var res = new List<ConstantContainer>();
            res.AddRange(left.GetConstants());
            res.AddRange(right.GetConstants());
            return res;
        }

        public override string ToString() => DialectResolver.Dialect.MultiplicationToString(this);

        public override bool Equals(object other)
        {
            if (!(other is Multiplication otherM)) return false;

            return Alias.Equals(otherM.Alias)
                && left.Equals(otherM.left)
                && right.Equals(otherM.right);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      left.GetHashCode() *
                      right.GetHashCode());
    }

    public class Division : Operation
    {
        public Division(Expression left, Expression right, string aliasName = "")
        {
            this.left = left;
            this.right = right;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Division(left_clone as Expression, right_clone as Expression, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            try { return (dynamic)left.Eval(r) / (dynamic)right.Eval(r); }
            catch (RuntimeBinderException) { return Convert.ToDecimal(left.Eval(r)) / Convert.ToDecimal(right.Eval(r)); }
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ConstantContainer> GetConstants()
        {
            var res = new List<ConstantContainer>();
            res.AddRange(left.GetConstants());
            res.AddRange(right.GetConstants());
            return res;
        }

        public override string ToString() => DialectResolver.Dialect.DivisionToString(this);

        public override bool Equals(object other)
        {
            if (!(other is Division otherD)) return false;

            return Alias.Equals(otherD.Alias)
                && left.Equals(otherD.left)
                && right.Equals(otherD.right);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      left.GetHashCode() *
                      right.GetHashCode());
    }
}
