using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class ConstantContainer : Expression
    {
        public Constant constant;

        public ConstantContainer(object value = null, int index = 0, string aliasName = "")
        {
            Alias = new Identifier(aliasName);

            if (value == null)
                this.constant = new PlaceholderConstant(index);
            else if (value is Constant constant)
                this.constant = constant;
            else
                this.constant = Constant.GetConstant(value);
        }

        public ConstantContainer(ConstantContainer other)
        {
            Alias = new Identifier(other.Alias.id);
            constant = (Constant)other.constant.Clone();
        }

        public override object Clone() => new ConstantContainer(this);

        public override object Eval(ResultRow r) => constant.Eval(r);

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer> { this };

        public override string ToString() => DialectResolver.Dialect.ConstantContainerToString(this);

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override bool Equals(object other)
        {
            if (!(other is ConstantContainer otherCC)) return false;

            return Alias.Equals(otherCC.Alias)
                && constant.Equals(otherCC.constant);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      constant.GetHashCode());
    }

    public abstract class Constant : ICloneable
    {
        public static Constant GetConstant(object value)
        {
            if (value == null)
                return new NullConstant();

            switch (value)
            {
                case int intValue:
                    return new IntConstant(intValue);
                case long longValue:
                    return new IntConstant(longValue);
                case short shortValue:
                    return new IntConstant(shortValue);
                case byte byteValue:
                    return new IntConstant(byteValue);
                case sbyte sbyteValue:
                    return new IntConstant(sbyteValue);
                case double doubleValue:
                    return new DecimalConstant((decimal)doubleValue);
                case float floatValue:
                    return new DecimalConstant((decimal)floatValue);
                case decimal decimalValue:
                    return new DecimalConstant(decimalValue);
                case byte[] byteaValue:
                    return new BinaryConstant(byteaValue);
                case DateTime datetimeValue:
                    return new StringConstant(
                        datetimeValue.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture));
                case string stringValue:
                    return new StringConstant(stringValue);
                case DBNull _:
                    return new NullConstant();
                default:
                    throw new NotSupportedException("Type not supported by GetConstant.");
            }
        }

        public abstract object Clone();

        public abstract object Eval(ResultRow r);

        public abstract override string ToString();

        public abstract override bool Equals(object other);

        public abstract override int GetHashCode();
    }

    public class IntConstant : Constant
    {
        public Int64 intvalue;

        public IntConstant() : this(0) { }

        public IntConstant(Int64 value)
        {
            intvalue = value;
        }

        public override object Clone() => new IntConstant(intvalue);

        public override object Eval(ResultRow r) => intvalue;

        public override string ToString() => DialectResolver.Dialect.IntConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is IntConstant otherIC)) return false;
            return intvalue == otherIC.intvalue;
        }

        public override int GetHashCode() => unchecked(intvalue.GetHashCode());
    }

    public class StringConstant : Constant
    {
        public string strvalue;

        public StringConstant() : this("") { }

        public StringConstant(string value)
        {
            strvalue = value;
        }

        public override object Clone() => new StringConstant(strvalue);

        public override object Eval(ResultRow r) => strvalue;

        public override string ToString() => DialectResolver.Dialect.StringConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is StringConstant otherSC)) return false;
            return strvalue == otherSC.strvalue;
        }

        public override int GetHashCode() => unchecked(strvalue.GetHashCode());
    }

    public class BinaryConstant : Constant
    {
        public byte[] binvalue;

        public BinaryConstant() : this(new byte[0]) { }

        public BinaryConstant(byte[] value)
        {
            binvalue = value;
        }

        public override object Clone() => new BinaryConstant((byte[])binvalue.Clone());

        public override object Eval(ResultRow r) => throw new NotImplementedException("This method should not be called.");

        public override string ToString() => DialectResolver.Dialect.BinaryConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is BinaryConstant otherBC)) return false;

            return binvalue.SequenceEqual(otherBC.binvalue);
        }

        public override int GetHashCode() => unchecked(binvalue.Aggregate(1, unchecked((x, y) => x * y.GetHashCode())));
    }

    public class DecimalConstant : Constant
    {
        public Decimal decimalvalue;

        public DecimalConstant() : this(0) { }

        public DecimalConstant(Decimal value)
        {
            decimalvalue = value;
        }

        public override object Clone() => new DecimalConstant(decimalvalue);

        public override object Eval(ResultRow r) => decimalvalue;

        public override string ToString() => DialectResolver.Dialect.DecimalConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is DecimalConstant otherIC)) return false;

            return decimalvalue == otherIC.decimalvalue;
        }

        public override int GetHashCode() => unchecked(decimalvalue.GetHashCode());
    }

    public class NullConstant : Constant
    {
        public NullConstant() { }

        public override object Clone() => new NullConstant();

        public override object Eval(ResultRow r) =>
            throw new InvalidOperationException("NULL constant should not be used in WHERE clause like that.");

        public override string ToString() => DialectResolver.Dialect.NullConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is NullConstant)) return false;
            return true;
        }

        public override int GetHashCode() => 1;
    }

    public class PlaceholderConstant : Constant
    {
        public int index;

        public PlaceholderConstant() : this(0) { }

        public PlaceholderConstant(int index)
        {
            this.index = index;
        }

        public override object Clone() => new PlaceholderConstant(index);

        public override object Eval(ResultRow r) =>
            throw new InvalidOperationException("Placeholder constant should be replaced with a proper constant.");

        public override string ToString() => DialectResolver.Dialect.PlaceholderConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is PlaceholderConstant otherPC)) return false;
            return index == otherPC.index;
        }

        public override int GetHashCode() => unchecked(index.GetHashCode());
    }
}
