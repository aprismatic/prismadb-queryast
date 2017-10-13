using System.Collections.Generic;
using System.Data;

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
            : this(columnName)
        {
            Table = table.Clone();
        }

        public ColumnRef(string tableName, Identifier column)
            : this(tableName, column.id)
        { }

        public ColumnRef(TableRef table, Identifier column)
            : this(column.id)
        {
            Table = table.Clone();
        }

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
            return DialectResolver.Dialect.ColumnRefToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is ColumnRef otherCR)) return false;

            return ColumnName.Equals(otherCR.ColumnName)
                && Table.Equals(otherCR.Table);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                Table.GetHashCode());
        }
    }
}
