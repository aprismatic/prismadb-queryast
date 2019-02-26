namespace PrismaDB.QueryAST.DDL
{
    public class ShowQuery : DdlQuery
    {
        public ShowQuery() { }

        public override string ToString()
        {
            return DialectResolver.Dialect.ShowQueryToString(this);
        }

        public override object Clone()
        {
            return new ShowQuery();
        }
    }
}
