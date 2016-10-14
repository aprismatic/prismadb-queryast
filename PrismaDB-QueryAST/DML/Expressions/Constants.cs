using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
                ColumnName = (string)value[1];
        }

        public override Expression Clone()
        {
            var clone = new IntConstant(intvalue, ColumnName);

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
            if (ColumnName.Length == 0)
                return intvalue.ToString();

            var sb = new StringBuilder(intvalue.ToString());

            sb.Append(" AS [");
            sb.Append(ColumnName);
            sb.Append("]");

            return sb.ToString();
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
                ColumnName = (string)value[1];
        }

        public override Expression Clone()
        {
            var clone = new StringConstant(strvalue, ColumnName);

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
            var sb = new StringBuilder("'");
            sb.Append(strvalue);
            sb.Append("'");

            if (ColumnName.Length > 0)
            {
                sb.Append(" AS [");
                sb.Append(ColumnName);
                sb.Append("]");
            }

            return sb.ToString();
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
                ColumnName = (string)value[1];
        }

        public override Expression Clone()
        {
            var clone = new BinaryConstant(binvalue, ColumnName);

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
            var sb = new StringBuilder("0x");
            sb.Append(Commons.Helper.ByteArrayToHex(binvalue));

            if (ColumnName.Length > 0)
            {
                sb.Append(" AS [");
                sb.Append(ColumnName);
                sb.Append("]");
            }

            return sb.ToString();
        }
    }
}
