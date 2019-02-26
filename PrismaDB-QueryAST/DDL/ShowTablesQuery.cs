namespace PrismaDB.QueryAST.DDL
{
    public class ShowTablesQuery : DdlQuery
    {
        public ShowTablesQuery() { }

        public override string ToString()
        {
            return DialectResolver.Dialect.ShowTablesQueryToString(this);
        }

        public override object Clone()
        {
            return new ShowTablesQuery();
        }
    }
}
