using System;

namespace PrismaDB.QueryAST
{
    public class DatabaseRef
    {
        public Identifier Database;

        public DatabaseRef(string DatabaseName)
        {
            Database = new Identifier(DatabaseName);
        }

        public DatabaseRef Clone()
        {
            return new DatabaseRef(Database.id);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DatabaseRefToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is DatabaseRef otherDR)) return false;

            return String.Equals(Database.id, otherDR.Database.id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return unchecked(
                Database.GetHashCode());
        }
    }
}
