using System.Collections.Generic;

namespace PrismaDB.QueryAST.DDL
{
    public class CreateTableQuery : DDLQuery
    {
        public TableRef TableName;
        public List<ColumnDefinition> ColumnDefinitions;

        public CreateTableQuery()
            : this(new TableRef(""))
        { }

        public CreateTableQuery(string newTableName)
            : this(new TableRef(newTableName))
        { }

        public CreateTableQuery(TableRef newTable)
        {
            TableName = newTable.Clone();
            ColumnDefinitions = new List<ColumnDefinition>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.CreateTableQueryToString(this);
        }
    }
}
