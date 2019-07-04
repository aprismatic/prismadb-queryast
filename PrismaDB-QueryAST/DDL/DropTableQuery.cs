using System.Collections.Generic;

namespace PrismaDB.QueryAST.DDL
{
    public class DropTableQuery : DdlQuery
    {
        public TableRef TableName;

        public DropTableQuery()
            : this(new TableRef(""))
        { }

        public DropTableQuery(string newTableName)
            : this(new TableRef(newTableName))
        { }

        public DropTableQuery(TableRef newTable)
        {
            TableName = newTable;
        }

        public DropTableQuery(DropTableQuery other)
        {
            TableName = other.TableName;
        }

        public override List<TableRef> GetTables() => new List<TableRef> { TableName.Clone() };

        public override string ToString() => DialectResolver.Dialect.DropTableQueryToString(this);

        public override object Clone() => new DropTableQuery(this);
    }
}
