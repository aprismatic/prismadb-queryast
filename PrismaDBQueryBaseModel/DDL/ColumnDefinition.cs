using System.Text;

namespace PrismaDBQueryBaseModel.DDL
{
    public enum SQLDataType
    {
        INT,
        UNIQUEIDENTIFIER,
        VARBINARY,
        VARCHAR
    }

    public class ColumnDefinition
    {
        public string ColumnName;
        public SQLDataType DataType;
        public int? Length;
        public bool Nullable;
        public bool isRowId;

        public ColumnDefinition(string ColumnName = "",
                                SQLDataType DataType = SQLDataType.INT,
                                int? Length = null,
                                bool Nullable = true,
                                bool isRowId = false)
        {
            this.ColumnName = ColumnName;
            this.DataType = DataType;
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
