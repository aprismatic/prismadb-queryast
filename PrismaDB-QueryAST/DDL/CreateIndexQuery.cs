using PrismaDB.QueryAST.DML;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DDL
{
    public enum IndexModifier
    {
        DEFAULT,
        MSSQL_UNIQUE,
        MySQL_UNIQUE,
        MSSQL_FULLTEXT,
        MySQL_FULLTEXT,
        MySQL_SPATIAL,
        Postgres_UNIQUE
    }

    public enum IndexType
    {
        DEFAULT,
        MSSQL_CLUSTERED,
        MSSQL_NONCLUSTERED,
        MySQL_BTREE,
        MySQL_HASH,
        Postgres_BTREE,
        Postgres_HASH,
        Postgres_GIN,
        Postgres_GIST
    }

    public class CreateIndexQuery : DdlQuery
    {
        public IndexModifier Modifier;
        public IndexType Type;
        public TableRef OnTable;
        public List<ColumnRef> OnColumns;
        public Identifier Name;
        public Identifier MsSqlFullTextKeyIndex;

        public CreateIndexQuery()
        {
            Name = new Identifier();
            OnTable = new TableRef("");
            Type = IndexType.DEFAULT;
            Modifier = IndexModifier.DEFAULT;
            OnColumns = new List<ColumnRef>();
            MsSqlFullTextKeyIndex = null;
        }

        public CreateIndexQuery(string name, TableRef table, IndexType type = IndexType.DEFAULT, IndexModifier modifier = IndexModifier.DEFAULT, string msSqlFullTextKeyIndex = null, params ColumnRef[] columns)
        {
            Name = new Identifier(name);
            OnTable = table;
            Type = type;
            Modifier = modifier;
            OnColumns = new List<ColumnRef>(columns.Length);
            OnColumns.AddRange(columns.Select(x => x));
            if (msSqlFullTextKeyIndex != null)
                MsSqlFullTextKeyIndex = new Identifier(msSqlFullTextKeyIndex);
        }

        public CreateIndexQuery(string name, string table, IndexType type = IndexType.DEFAULT, IndexModifier modifier = IndexModifier.DEFAULT, string msSqlFullTextKeyIndex = null, params ColumnRef[] columns)
            : this(name, new TableRef(table), type, modifier, msSqlFullTextKeyIndex, columns)
        { }

        public CreateIndexQuery(CreateIndexQuery other)
            : this(other.Name.id, other.OnTable, other.Type, other.Modifier, other.MsSqlFullTextKeyIndex?.id, other.OnColumns.ToArray())
        { }

        public override List<TableRef> GetTables() => new List<TableRef> { OnTable.Clone() };

        public override List<PlaceholderConstant> GetPlaceholders() => new List<PlaceholderConstant>();

        public override string ToString() => DialectResolver.Dialect.CreateIndexQueryToString(this);

        public override object Clone() => new CreateIndexQuery(this);
    }
}
