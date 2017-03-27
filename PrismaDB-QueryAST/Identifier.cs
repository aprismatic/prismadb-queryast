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

        public new virtual string ToString()
        {
            return "[" + id + "]";
        }

        public new virtual bool Equals(object other)
        {
            var otherID = other as Identifier;
            if (otherID == null) return false;

            return this.id == otherID.id;
        }

        public new virtual int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
