namespace PrismaDB.QueryAST.DDL
{
    public class MsSqlCreateFullTextCatalogQuery : DdlQuery
    {
        public Identifier Name;
        public bool AsDefault;

        public MsSqlCreateFullTextCatalogQuery(string name, bool asDefault = true)
        {
            Name = new Identifier(name);
            AsDefault = asDefault;
        }

        public MsSqlCreateFullTextCatalogQuery(MsSqlCreateFullTextCatalogQuery other)
            : this(other.Name.id, other.AsDefault)
        { }

        public override string ToString()
        {
            return DialectResolver.Dialect.MsSqlCreateFullTextCatalogQueryToString(this);
        }

        public override object Clone()
        {
            return new MsSqlCreateFullTextCatalogQuery(this);
        }
    }
}
