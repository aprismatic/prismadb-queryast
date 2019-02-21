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

        public IntConstant() : this(0) { }

        public IntConstant(Int64 value, string aliasName = "")
        {
            intvalue = value;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            return new IntConstant(intvalue, Alias.id);
        }

        public override object Eval(ResultRow r)
        {
            return intvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
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

        public StringConstant() : this("") { }

        public StringConstant(string value, string aliasName = "")
        {
            strvalue = (string)value.Clone();
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            return new StringConstant(strvalue, Alias.id);
        }

        public override object Eval(ResultRow r)
        {
            return strvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
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

        public BinaryConstant() : this(new byte[0]) { }

        public BinaryConstant(byte[] value, string aliasName = "")
        {
            binvalue = (byte[])value.Clone();
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            return new BinaryConstant(binvalue, Alias.id);
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("This method should not be called.");
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return new List<ColumnRef>();
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

        public FloatingPointConstant() : this(0) { }

        public FloatingPointConstant(Decimal value, string aliasName = "")
        {
            floatvalue = value;
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            return new FloatingPointConstant(floatvalue, Alias.id);
        }

        public override object Eval(ResultRow r)
        {
            return floatvalue;
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
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
        public NullConstant() : this("") { }

        public NullConstant(string aliasName = "")
        {
            Alias = new Identifier(aliasName);
        }

        public override object Clone()
        {
            return new NullConstant(Alias.id);
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("NULL constant should not be used in WHERE clause like that."); // TODO: reconsider
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
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
