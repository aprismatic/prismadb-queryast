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

        public override string DisplayName()
        {
            return Alias.id;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.AdditionToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Addition otherA)) return false;

            return (this.Alias == otherA.Alias)
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

        public override string DisplayName()
        {
            return Alias.id;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.MultiplicationToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Multiplication otherM)) return false;

            return (this.Alias == otherM.Alias)
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

    public class PaillierAddition : Operation
    {
        public Expression N;

        public PaillierAddition(Expression left, Expression right, Expression N)
        {
            setValue(left, right, N, "");
        }

        public PaillierAddition(Expression left, Expression right, Expression N, string aliasName)
        {
            setValue(left, right, N, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 3)
                throw new ArgumentException("PaillierAddition constructor expects 3 or 4 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            N = (Expression)value[2];

            if (value.Length > 3)
                Alias = new Identifier((string)value[3]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;
            var N_clone = N.Clone() as Expression;

            var clone = new PaillierAddition(left_clone, right_clone, N_clone, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override List<ColumnRef> GetColumns()
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override string DisplayName()
        {
            return Alias.id;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.PaillierAdditionToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is PaillierAddition otherPA)) return false;

            return (this.Alias == otherPA.Alias)
                && (this.left.Equals(otherPA.left))
                && (this.right.Equals(otherPA.right))
                && (this.N.Equals(otherPA.N));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode() *
                N.GetHashCode());
        }
    }

    public class ElGamalMultiplication : Operation
    {
        public Expression P;

        public ElGamalMultiplication(Expression left, Expression right, Expression P)
        {
            setValue(left, right, P, "");
        }

        public ElGamalMultiplication(Expression left, Expression right, Expression P, string aliasName)
        {
            setValue(left, right, P, aliasName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 3)
                throw new ArgumentException("ElGamalMultiplication constructor expects 3 or 4 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            P = (Expression)value[2];

            if (value.Length > 3)
                Alias = new Identifier((string)value[3]);
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;
            var P_clone = P.Clone() as Expression;

            var clone = new ElGamalMultiplication(left_clone, right_clone, P_clone, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override List<ColumnRef> GetColumns()
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override string DisplayName()
        {
            return Alias.id;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ElGamalMultiplicationToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is ElGamalMultiplication otherEGM)) return false;

            return (this.Alias == otherEGM.Alias)
                && (this.left.Equals(otherEGM.left))
                && (this.right.Equals(otherEGM.right))
                && (this.P.Equals(otherEGM.P));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode() *
                P.GetHashCode());
        }
    }
}
