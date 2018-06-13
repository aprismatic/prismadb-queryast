namespace PrismaDB.QueryAST.DDL
{
    public class UseStatement : DdlQuery
    {
        public DatabaseRef Database;

        public UseStatement(DatabaseRef database)
        {
            Database = database.Clone();
        }

        public UseStatement(string database)
            : this(new DatabaseRef(database))
        { }

        public UseStatement(UseStatement other)
            : this(other.Database)
        { }

        public override string ToString()
        {
            return DialectResolver.Dialect.UseStatementToString(this);
        }

        public override object Clone()
        {
            return new UseStatement(this);
        }
    }
}
