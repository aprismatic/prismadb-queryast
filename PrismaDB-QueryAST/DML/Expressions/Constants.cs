using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Constant : Expression { }

    public class IntConstant : Constant
    {
        public Int32 intvalue;

        public IntConstant(Int32 value)
        {
            setValue(value, "");
        }

        public IntConstant(Int32 value, string ColumnName)
        {
            setValue(value, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            intvalue = (Int32)value[0];

            if (value.Length > 1)
                ColumnName = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new IntConstant(intvalue, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
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

            return (this.ColumnName.Equals(otherIC.ColumnName))
                && (this.intvalue == otherIC.intvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                intvalue.GetHashCode());
        }
    }

    public class StringConstant : Constant
    {
        public string strvalue;

        public StringConstant(string strvalue)
        {
            setValue(strvalue, "");
        }

        public StringConstant(string strvalue, string ColumnName)
        {
            setValue(strvalue, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            strvalue = (string)((string)value[0]).Clone();

            if (value.Length > 1)
                ColumnName = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new StringConstant(strvalue, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
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

            return (this.ColumnName.Equals(otherSC.ColumnName))
                && (this.strvalue == otherSC.strvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                strvalue.GetHashCode());
        }
    }

    public class BinaryConstant : Constant
    {
        public byte[] binvalue;

        public BinaryConstant(byte[] binvalue)
        {
            setValue(binvalue, "");
        }

        public BinaryConstant(byte[] binvalue, string ColumnName)
        {
            setValue(binvalue, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            binvalue = (byte[])((byte[])value[0]).Clone();

            if (value.Length > 1)
                ColumnName = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new BinaryConstant(binvalue, ColumnName.id);

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
            return DialectResolver.Dialect.BinaryConstantToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BinaryConstant otherBC)) return false;

            return (this.ColumnName.Equals(otherBC.ColumnName))
                && (this.binvalue.SequenceEqual(otherBC.binvalue));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                binvalue.Aggregate(1, unchecked((x, y) => x * y.GetHashCode())));
        }
    }

    public class FloatingPointConstant : Constant
    {
        public Double floatvalue;

        public FloatingPointConstant(Double value)
        {
            setValue(value, "");
        }

        public FloatingPointConstant(Double value, string ColumnName)
        {
            setValue(value, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;

            floatvalue = (Double)value[0];

            if (value.Length > 1)
                ColumnName = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new FloatingPointConstant(floatvalue, ColumnName.id);

            return clone;
        }

        public override object Eval(DataRow r)
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

            return (this.ColumnName.Equals(otherIC.ColumnName))
                && (this.floatvalue == otherIC.floatvalue);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                floatvalue.GetHashCode());
        }
    }
}
