using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

        public Addition(Expression left, Expression right, string ColumnName)
        {
            setValue(left, right, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Addition constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                ColumnName = new Identifier((string)value[2]);
        }

        public override Expression Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Addition(left_clone, right_clone, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
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

        public override string ToString()
        {
            var sb = new StringBuilder(" ( ");
            sb.Append(left.ToString());
            sb.Append(" + ");
            sb.Append(right.ToString());
            sb.Append(" ) ");

            if (ColumnName.id.Length > 0)
            {
                sb.Append("AS ");
                sb.Append(ColumnName.ToString());
                sb.Append(" ");
            }

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherA = other as Addition;
            if (otherA == null) return false;

            return (this.ColumnName == otherA.ColumnName)
                && (this.left.Equals(otherA.left))
                && (this.right.Equals(otherA.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
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

        public Multiplication(Expression left, Expression right, string ColumnName)
        {
            setValue(left, right, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Multiplication constructor expects 2 or 3 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            if (value.Length > 2)
                ColumnName = new Identifier((string)value[2]);
        }

        public override Expression Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();

            var clone = new Multiplication(left_clone, right_clone, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
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

        public override string ToString()
        {
            var sb = new StringBuilder(" ( ");
            sb.Append(left.ToString());
            sb.Append(" * ");
            sb.Append(right.ToString());
            sb.Append(" ) ");

            if (ColumnName.id.Length > 0)
            {
                sb.Append("AS ");
                sb.Append(ColumnName.ToString());
                sb.Append(" ");
            }

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherM = other as Multiplication;
            if (otherM == null) return false;

            return (this.ColumnName == otherM.ColumnName)
                && (this.left.Equals(otherM.left))
                && (this.right.Equals(otherM.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
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

        public PaillierAddition(Expression left, Expression right, Expression N, string ColumnName)
        {
            setValue(left, right, N, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 3)
                throw new ArgumentException("PaillierAddition constructor expects 3 or 4 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            N = (Expression)value[2];

            if (value.Length > 3)
                ColumnName = new Identifier((string)value[3]);
        }

        public override Expression Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();
            var N_clone = N.Clone();

            var clone = new PaillierAddition(left_clone, right_clone, N_clone, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override List<ColumnRef> GetColumns()
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[dbo].[PaillierAddition] ( ");
            sb.Append(left.ToString());
            sb.Append(" , ");
            sb.Append(right.ToString());
            sb.Append(" , ");
            sb.Append(N.ToString());

            if (ColumnName.id.Length > 0)
            {
                sb.Append(" ) AS ");
                sb.Append(ColumnName.ToString());
                sb.Append(" ");
            }
            else
                sb.Append(" ) ");

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherPA = other as PaillierAddition;
            if (otherPA == null) return false;

            return (this.ColumnName == otherPA.ColumnName)
                && (this.left.Equals(otherPA.left))
                && (this.right.Equals(otherPA.right))
                && (this.N.Equals(otherPA.N));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
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

        public ElGamalMultiplication(Expression left, Expression right, Expression P, string ColumnName)
        {
            setValue(left, right, P, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length < 3)
                throw new ArgumentException("ElGamalMultiplication constructor expects 3 or 4 arguments");

            left = (Expression)value[0];
            right = (Expression)value[1];

            P = (Expression)value[2];

            if (value.Length > 3)
                ColumnName = new Identifier((string)value[3]);
        }

        public override Expression Clone()
        {
            var left_clone = left.Clone();
            var right_clone = right.Clone();
            var P_clone = P.Clone();

            var clone = new ElGamalMultiplication(left_clone, right_clone, P_clone, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override List<ColumnRef> GetColumns()
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[dbo].[ElGamalMultiplication] ( ");
            sb.Append(left.ToString());
            sb.Append(" , ");
            sb.Append(right.ToString());
            sb.Append(" , ");
            sb.Append(P.ToString());

            if (ColumnName.id.Length > 0)
            {
                sb.Append(" ) AS ");
                sb.Append(ColumnName.ToString());
                sb.Append(" ");
            }
            else
                sb.Append(" ) ");

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherEGM = other as ElGamalMultiplication;
            if (otherEGM == null) return false;

            return (this.ColumnName == otherEGM.ColumnName)
                && (this.left.Equals(otherEGM.left))
                && (this.right.Equals(otherEGM.right))
                && (this.P.Equals(otherEGM.P));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode() *
                P.GetHashCode());
        }
    }
}
