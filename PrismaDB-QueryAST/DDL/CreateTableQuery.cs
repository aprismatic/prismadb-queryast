using System.Collections.Generic;
using System.Linq;

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

        public CreateTableQuery(CreateTableQuery other)
        {
            TableName = other.TableName.Clone();
            ColumnDefinitions = new List<ColumnDefinition>(other.ColumnDefinitions.Count);
            ColumnDefinitions.AddRange(other.ColumnDefinitions.Select(x => x.Clone() as ColumnDefinition));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.CreateTableQueryToString(this);
        }

        public override object Clone()
        {
            return new CreateTableQuery(this);
        }
    }
}
