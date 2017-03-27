using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PrismaDB.QueryAST.DML
{
    public class ColumnRef : Expression
    {
        public TableRef Table;

        public ColumnRef(string tableName, string columnName)
        {
            setValue(tableName, columnName);
        }

        public ColumnRef(string columnName)
            : this("", columnName)
        { }

        public ColumnRef(Identifier column)
            : this("", column.id)
        { }

        public ColumnRef(TableRef table, string columnName)
            : this(table.Table.id, columnName)
        { }

        public ColumnRef(string tableName, Identifier column)
            : this(tableName, column.id)
        { }

        public ColumnRef(TableRef table, Identifier column)
            : this(table.Table.id, column.id)
        { }

        public override void setValue(params object[] value)
        {
            Parent = null;
            Table = new TableRef((string)value[0]);
            ColumnName = new Identifier((string)value[1]);
        }

        public override Expression Clone()
        {
            var clone = new ColumnRef(Table, ColumnName);

            return clone;
        }

        public override object Eval(DataRow r)
        {
            return r[ToString()];
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>
            {
                (ColumnRef)Clone()
            };
        }

        public override string ToString()
        {
            var tblnm = Table.ToString();

            var sb = new StringBuilder(tblnm);

            if (sb.Length > 0)
                sb.Append(".");

            sb.Append(ColumnName.ToString());

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherCR = other as ColumnRef;
            if (otherCR == null) return false;

            return (this.ColumnName == otherCR.ColumnName)
                && (this.Table.Equals(otherCR.Table));
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                Table.GetHashCode());
        }
    }
}
