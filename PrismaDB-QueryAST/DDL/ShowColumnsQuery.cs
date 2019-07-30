using System.Collections.Generic;

namespace PrismaDB.QueryAST.DDL
{
    public class ShowColumnsQuery : DdlQuery
    {
        public TableRef TableName;

        public ShowColumnsQuery() : this("") { }

        public ShowColumnsQuery(string tableName)
        {
            TableName = new TableRef(tableName);
        }

        public ShowColumnsQuery(TableRef table)
            : this(table.Table.id) { }

        public ShowColumnsQuery(ShowColumnsQuery other)
        {
            TableName = other.TableName.Clone();
        }

        public override List<TableRef> GetTables() => new List<TableRef> { TableName };

        public override string ToString() => DialectResolver.Dialect.ShowColumnsQueryToString(this);

        public override object Clone() => new ShowColumnsQuery(this);
    }
}
