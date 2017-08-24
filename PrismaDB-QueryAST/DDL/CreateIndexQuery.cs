using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.DDL
{
    public enum IndexType
    {
        CLUSTERED,
        NONCLUSTERED,
    }

    public class CreateIndexQuery : DDLQuery
    {
        public IndexType Type;
        public TableRef OnTable;
        public List<ColumnRef> OnColumns;
        public Identifier Name;

        public CreateIndexQuery(IndexType type, string name, TableRef table, params ColumnRef[] columns)
        {
            Type = type;
            Name = new Identifier(name);
            OnTable = table.Clone();
            OnColumns = new List<ColumnRef>(columns.Length);
            OnColumns.AddRange(columns.Select(x => x.Clone() as ColumnRef));
        }

        public CreateIndexQuery(IndexType type, string name, string table, params ColumnRef[] columns)
            : this(type, name, new TableRef(table), columns)
        { }

        public CreateIndexQuery(CreateIndexQuery other)
            : this(other.Type, other.Name.id, other.OnTable, other.OnColumns.ToArray())
        { }

        public override string ToString()
        {
            return DialectResolver.Dialect.CreateIndexQueryToString(this);
        }
    }
}
