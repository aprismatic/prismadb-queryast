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
            TableName = newTable.Clone();
        }

        public DropTableQuery(DropTableQuery other)
        {
            TableName = other.TableName.Clone();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DropTableQueryToString(this);
        }

        public override object Clone()
        {
            return new DropTableQuery(this);
        }
    }
}
