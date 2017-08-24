using System;

namespace PrismaDB.QueryAST
{
    public class Identifier
    {
        public string id;

        public Identifier(string newId)
        {
            id = newId;
        }

        public Identifier Clone()
        {
            return new Identifier(id);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.IdentifierToString(this);
        }

        public override bool Equals(object other)
        {
            var otherId = other as Identifier;
            if (otherId == null) return false;

            return String.Equals(id, otherId.id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return id.ToLowerInvariant().GetHashCode();
        }
    }
}
