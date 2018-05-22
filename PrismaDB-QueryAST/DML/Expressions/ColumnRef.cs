using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public class ColumnRef : Expression
    {
        public TableRef Table;
        public Identifier ColumnName;

        public ColumnRef(string tableName, string columnName, string aliasName)
        {
            setValue(tableName, columnName, aliasName);
        }

        public ColumnRef(string tableName, string columnName)
            : this(tableName, columnName, columnName)
        { }

        public ColumnRef(string columnName)
            : this("", columnName, columnName)
        { }

        public ColumnRef(Identifier columnName)
            : this("", columnName.id, columnName.id)
        { }

        public ColumnRef(TableRef table, string columnName)
            : this(columnName)
        {
            Table = table.Clone();
        }

        public ColumnRef(string tableName, Identifier columnName)
            : this(tableName, columnName.id, columnName.id)
        { }

        public ColumnRef(TableRef table, Identifier columnName)
            : this(columnName.id)
        {
            Table = table.Clone();
        }

        public ColumnRef(TableRef table, Identifier columnName, Identifier alias)
        {
            setValue("", columnName.id, alias.id);
            Table = table.Clone();
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            Table = new TableRef((string)value[0]);
            ColumnName = new Identifier((string)value[1]);
            Alias = new Identifier((string)value[2]);
        }

        public override object Clone()
        {
            var clone = new ColumnRef(Table, ColumnName, Alias);

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

            return Alias.Equals(otherCR.Alias)
                && ColumnName.Equals(otherCR.ColumnName)
                && Table.Equals(otherCR.Table);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Alias.GetHashCode() *
                ColumnName.GetHashCode() *
                Table.GetHashCode());
        }
    }
}
