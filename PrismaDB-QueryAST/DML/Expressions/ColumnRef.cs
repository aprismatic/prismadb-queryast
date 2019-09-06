using PrismaDB.QueryAST.Result;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class ColumnRef : Expression
    {
        public TableRef Table;
        public Identifier ColumnName;

        public ColumnRef(string tableName, string columnName, string aliasName)
            : this(new TableRef(tableName), new Identifier(columnName), new Identifier(aliasName))
        { }

        public ColumnRef(string tableName, string columnName)
            : this(tableName, columnName, "")
        { }

        public ColumnRef(string columnName)
            : this("", columnName, "")
        { }

        public ColumnRef(Identifier columnName)
            : this(new TableRef(""), columnName, new Identifier(""))
        { }

        public ColumnRef(TableRef table, string columnName)
            : this(table, new Identifier(columnName), new Identifier(""))
        { }

        public ColumnRef(string tableName, Identifier columnName)
            : this(new TableRef(tableName), columnName, new Identifier(""))
        { }

        public ColumnRef(TableRef table, Identifier columnName)
            : this(table, columnName, new Identifier(""))
        { }

        public ColumnRef(TableRef table, Identifier columnName, Identifier alias)
        {
            Parent = null;
            Table = table;
            ColumnName = columnName;
            Alias = alias;
        }

        public override object Clone() => new ColumnRef(Table.Clone(), ColumnName.Clone(), Alias.Clone());

        public override object Eval(ResultRow r) => r[this];

        public override List<ColumnRef> GetColumns() =>
            new List<ColumnRef> { this };

        public override List<PlaceholderConstant> GetPlaceholders() =>
            new List<PlaceholderConstant>();

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
