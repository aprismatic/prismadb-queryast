namespace PrismaDB.QueryAST.DDL
{
    public class DescribeQuery : DdlQuery
    {
        public TableRef TableName;

        public DescribeQuery() : this("") { }

        public DescribeQuery(string tableName)
        {
            TableName = new TableRef(tableName);
        }

        public DescribeQuery(TableRef table)
            : this(table.Table.id) { }

        public DescribeQuery(DescribeQuery other)
        {
            TableName = other.TableName.Clone();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DescribeQueryToString(this);
        }

        public override object Clone()
        {
            return new DescribeQuery(this);
        }
    }
}
