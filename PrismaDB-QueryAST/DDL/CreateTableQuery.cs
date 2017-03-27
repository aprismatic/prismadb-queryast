using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var sb = new StringBuilder("CREATE TABLE ");
            sb.Append(TableName.ToString());

            sb.Append(" ( ");
            sb.Append(String.Join(" , ", ColumnDefinitions.Select(x => x.ToString())));
            sb.Append(" ) ");

            return sb.ToString();
        }
    }
}
