using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Constant : Expression
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
                    return new FloatingPointConstant((decimal)doubleValue);
                case float floatValue:
                    return new FloatingPointConstant((decimal)floatValue);
                case decimal decimalValue:
                    return new FloatingPointConstant(decimalValue);
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
    }

    public class IntConstant : Constant
    {
        public Int64 intvalue;

        public IntConstant() : this(0) { }

        public IntConstant(Int64 value, string aliasName = "")
        {
            intvalue = value;
            Alias = new Identifier(aliasName);
        }

        public override object Clone() => new IntConstant(intvalue, Alias.id);

        public override object Eval(ResultRow r) => intvalue;

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.IntConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is IntConstant otherIC)) return false;

            return Alias.Equals(otherIC.Alias)
                && intvalue == otherIC.intvalue;
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      intvalue.GetHashCode());
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

        public override object Clone() => new StringConstant(strvalue, Alias.id);

        public override object Eval(ResultRow r) => strvalue;

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.StringConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is StringConstant otherSC)) return false;

            return Alias.Equals(otherSC.Alias)
                && strvalue == otherSC.strvalue;
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      strvalue.GetHashCode());
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

        public override object Clone() => new BinaryConstant(binvalue, Alias.id);

        public override object Eval(ResultRow r) => throw new NotImplementedException("This method should not be called.");

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.BinaryConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is BinaryConstant otherBC)) return false;

            return Alias.Equals(otherBC.Alias)
                && binvalue.SequenceEqual(otherBC.binvalue);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      binvalue.Aggregate(1, unchecked((x, y) => x * y.GetHashCode())));
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

        public override object Clone() => new FloatingPointConstant(floatvalue, Alias.id);

        public override object Eval(ResultRow r) => floatvalue;

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.FloatingPointConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is FloatingPointConstant otherIC)) return false;

            return Alias.Equals(otherIC.Alias)
                && floatvalue == otherIC.floatvalue;
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      floatvalue.GetHashCode());
    }

    public class NullConstant : Constant
    {
        public NullConstant() : this("") { }

        public NullConstant(string aliasName = "")
        {
            Alias = new Identifier(aliasName);
        }

        public override object Clone() => new NullConstant(Alias.id);

        public override object Eval(ResultRow r) =>
            throw new InvalidOperationException("NULL constant should not be used in WHERE clause like that.");

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.NullConstantToString(this);

        public override bool Equals(object other)
        {
            if (!(other is NullConstant otherNC)) return false;

            return Alias.Equals(otherNC.Alias);
        }

        public override int GetHashCode() => unchecked(Alias.GetHashCode());
    }
}
