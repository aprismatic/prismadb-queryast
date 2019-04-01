using PrismaDB.QueryAST.Result;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class ColumnRef : Expression
    {
        public TableRef Table;
        public Identifier ColumnName;

        public ColumnRef(string tableName, string columnName, string aliasName)
        {
            Parent = null;
            Table = new TableRef(tableName);
            ColumnName = new Identifier(columnName);
            Alias = new Identifier(aliasName);
        }

        public ColumnRef(string tableName, string columnName)
            : this(tableName, columnName, "")
        { }

        public ColumnRef(string columnName)
            : this("", columnName, "")
        { }

        public ColumnRef(Identifier columnName)
            : this("", columnName.id, "")
        { }

        public ColumnRef(TableRef table, string columnName)
            : this(columnName)
        {
            Table = table.Clone();
        }

        public ColumnRef(string tableName, Identifier columnName)
            : this(tableName, columnName.id, "")
        { }

        public ColumnRef(TableRef table, Identifier columnName)
            : this(columnName.id)
        {
            Table = table.Clone();
        }

        public ColumnRef(TableRef table, Identifier columnName, Identifier alias)
            : this("", columnName.id, alias.id)
        {
            Table = table.Clone();
        }

        public override object Clone() => new ColumnRef(Table, ColumnName, Alias);

        public override object Eval(ResultRow r) => r[this];

        public override List<ColumnRef> GetColumns() =>
            new List<ColumnRef>
            {
                (ColumnRef)Clone()
            };

        public override List<ColumnRef> GetNoCopyColumns() =>
            new List<ColumnRef>
            {
                this
            };

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.ColumnRefToString(this);

        public override bool Equals(object other)
        {
            if (!(other is ColumnRef otherCR)) return false;

            return Alias.Equals(otherCR.Alias)
                && ColumnName.Equals(otherCR.ColumnName)
                && Table.Equals(otherCR.Table);
        }

        public override int GetHashCode() =>
            unchecked(Alias.GetHashCode() *
                      ColumnName.GetHashCode() *
                      Table.GetHashCode());

        public string DisplayName() => Alias.id.Length == 0 ? ColumnName.id : Alias.id;

        public void AppendColumnName(string value)
        {
            ColumnName.id += value;

            if (Alias.id.Length != 0)
                Alias.id += value;
        }
    }
}
