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
        Text = 1,
        IntegerAddition = 2,
        IntegerMultiplication = 4,
        Search = 8
    }

    public class ColumnDefinition
    {
        public Identifier ColumnName;
        public SQLDataType DataType;
        public ColumnEncryptionFlags EncryptionFlags;
        public int? Length;
        public bool Nullable;
        public bool isRowId;

        public ColumnDefinition(string columnName = "",
                                SQLDataType dataType = SQLDataType.INT,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None)
        {
            this.ColumnName = new Identifier(columnName);
            this.DataType = dataType;
            this.EncryptionFlags = encryptionFlags;
            this.Length = length;
            this.Nullable = nullable;
            this.isRowId = isRowId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(ColumnName.ToString());
            sb.Append(" ");
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
