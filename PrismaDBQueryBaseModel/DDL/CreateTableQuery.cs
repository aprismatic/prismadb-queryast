using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismaDBQueryBaseModel.DDL
{
    public class CreateTableQuery : DDLQuery
    {
        public TableRef TableName;
        public List<ColumnDefinition> ColumnDefinitions;

        public CreateTableQuery()
        {
            TableName = new TableRef("");
            ColumnDefinitions = new List<ColumnDefinition>();
        }

        public CreateTableQuery(string newTableName)
        {
            TableName = new TableRef(newTableName);
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
