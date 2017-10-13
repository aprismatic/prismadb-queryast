namespace PrismaDB.QueryAST.DML
{
    public class DeleteQuery : DMLQuery
    {
        public TableRef DeleteTable;
        public WhereClause Where;

        public DeleteQuery()
        {
            DeleteTable = new TableRef("");
            Where = new WhereClause();
        }

        public DeleteQuery(DeleteQuery other)
        {
            DeleteTable = other.DeleteTable.Clone();
            Where = new WhereClause(other.Where);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DeleteQueryToString(this);
        }
    }
}
