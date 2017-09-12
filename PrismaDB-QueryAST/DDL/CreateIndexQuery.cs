using PrismaDB.QueryAST.DML;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DDL
{
    public enum IndexModifier
    {
        MSSQL_UNIQUE,
        MySQL_UNIQUE,
        MySQL_FULLTEXT,
        MySQL_SPATIAL,
        DEFAULT
    }

    public enum IndexType
    {
        MSSQL_CLUSTERED,
        MSSQL_NONCLUSTERED,
        MySQL_BTREE,
        MySQL_HASH,
        DEFAULT
    }

    public class CreateIndexQuery : DDLQuery
    {
        public IndexModifier Modifier;
        public IndexType Type;
        public TableRef OnTable;
        public List<ColumnRef> OnColumns;
        public Identifier Name;

        public CreateIndexQuery(string name, TableRef table, IndexType type = IndexType.DEFAULT, IndexModifier modifier = IndexModifier.DEFAULT, params ColumnRef[] columns)
        {
            Name = new Identifier(name);
            OnTable = table.Clone();
            Type = type;
            Modifier = modifier;
            OnColumns = new List<ColumnRef>(columns.Length);
            OnColumns.AddRange(columns.Select(x => x.Clone() as ColumnRef));
        }

        public CreateIndexQuery(string name, string table, IndexType type = IndexType.DEFAULT, IndexModifier modifier = IndexModifier.DEFAULT, params ColumnRef[] columns)
            : this(name, new TableRef(table), type, modifier, columns)
        { }

        public CreateIndexQuery(CreateIndexQuery other)
            : this(other.Name.id, other.OnTable, other.Type, other.Modifier, other.OnColumns.ToArray())
        { }

        public override string ToString()
        {
            return DialectResolver.Dialect.CreateIndexQueryToString(this);
        }
    }
}
