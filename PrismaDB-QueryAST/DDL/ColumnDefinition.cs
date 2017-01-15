using System;
using System.Text;

namespace PrismaDB.QueryAST.DDL
{
    public enum SQLDataType
    {
        INT,
        UNIQUEIDENTIFIER,
        VARBINARY,
        VARCHAR
    }

    [Flags]
    public enum ColumnEncryptionFlags
    {
        None = 0,
        Text = 2,
        IntegerAddition = 4,
        IntegerMultiplication = 8,
        Search = 16
    }

    public class ColumnDefinition
    {
        public string ColumnName;
        public SQLDataType DataType;
        public ColumnEncryptionFlags EncryptionFlags;
        public int? Length;
        public bool Nullable;
        public bool isRowId;

        public ColumnDefinition(string ColumnName = "",
                                SQLDataType DataType = SQLDataType.INT,
                                int? Length = null,
                                bool Nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags EncryptionFlags = ColumnEncryptionFlags.None)
        {
            this.ColumnName = ColumnName;
            this.DataType = DataType;
            this.EncryptionFlags = EncryptionFlags;
            this.Length = Length;
            this.Nullable = Nullable;
            this.isRowId = isRowId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(ColumnName);
            sb.Append("] ");
            sb.Append(DataType.ToString());

            if (Length != null)
            {
                sb.Append("(");
                sb.Append(Length < 0 ? "MAX" : Length.ToString());
                sb.Append(")");
            }

            sb.Append(" ");
            if (!Nullable)
                sb.Append("NOT ");
            sb.Append("NULL");

            if (isRowId)
                sb.Append(" DEFAULT NEWID()");

            return sb.ToString();
        }
    }
}
