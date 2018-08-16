using System;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.Result;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Constant : Expression { }

    public class IntConstant : Constant
    {
        public Int64 intvalue;

        public IntConstant()
            : this(0)
        { }

        public IntConstant(Int64 value)
        {
            setValue(value, "");
        }

        public IntConstant(Int64 value, string aliasName)
        {
            setValue(value, aliasName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            intvalue = (Int64)value[0];

            if (value.Length > 1)
                Alias = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new IntConstant(intvalue, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return intvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.IntConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is IntConstant otherIC)) return false;

            return (this.Alias.Equals(otherIC.Alias))
                && (this.intvalue == otherIC.intvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                intvalue.GetHashCode());
        }
    }

    public class StringConstant : Constant
    {
        public string strvalue;

        public StringConstant()
            : this("")
        { }

        public StringConstant(string strvalue)
        {
            setValue(strvalue, "");
        }

        public StringConstant(string strvalue, string aliasName)
        {
            setValue(strvalue, aliasName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            strvalue = (string)((string)value[0]).Clone();

            if (value.Length > 1)
                Alias = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new StringConstant(strvalue, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return strvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.StringConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is StringConstant otherSC)) return false;

            return (this.Alias.Equals(otherSC.Alias))
                && (this.strvalue == otherSC.strvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                strvalue.GetHashCode());
        }
    }

    public class BinaryConstant : Constant
    {
        public byte[] binvalue;

        public BinaryConstant()
            : this(new byte[0])
        { }

        public BinaryConstant(byte[] binvalue)
        {
            setValue(binvalue, "");
        }

        public BinaryConstant(byte[] binvalue, string aliasName)
        {
            setValue(binvalue, aliasName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            binvalue = (byte[])((byte[])value[0]).Clone();

            if (value.Length > 1)
                Alias = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new BinaryConstant(binvalue, Alias.id);

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

        public override string ToString()
        {
            return DialectResolver.Dialect.BinaryConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BinaryConstant otherBC)) return false;

            return (this.Alias.Equals(otherBC.Alias))
                && (this.binvalue.SequenceEqual(otherBC.binvalue));
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                binvalue.Aggregate(1, unchecked((x, y) => x * y.GetHashCode())));
        }
    }

    public class FloatingPointConstant : Constant
    {
        public Decimal floatvalue;

        public FloatingPointConstant()
            : this(0)
        { }

        public FloatingPointConstant(Decimal value)
        {
            setValue(value, "");
        }

        public FloatingPointConstant(Decimal value, string ColumnName)
        {
            setValue(value, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            floatvalue = (Decimal)value[0];

            if (value.Length > 1)
                Alias = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new FloatingPointConstant(floatvalue, Alias.id);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            return floatvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FloatingPointConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is FloatingPointConstant otherIC)) return false;

            return (this.Alias.Equals(otherIC.Alias))
                && (this.floatvalue == otherIC.floatvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                floatvalue.GetHashCode());
        }
    }

    public class NullConstant : Constant
    {
        public NullConstant()
            : this(null)
        { }

        public NullConstant(string aliasName)
        {
            setValue(aliasName);
        }

        public override object Clone()
        {
            return new NullConstant(Alias.id);
        }

        public override void setValue(params object[] value)
        {
            switch (value.Length)
            {
                case 1:
                    Alias = new Identifier((string)value[0]);
                    break;
                default:
                    throw new ArgumentException("NullConstant.setValue expects zero or one arguments");
            }
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("NULL constant should not be used in WHERE clause like that."); // TODO: reconsider
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.NullConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is NullConstant otherNC)) return false;

            return Alias.Equals(otherNC.Alias);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode());
        }
    }
}
