using System;

namespace PrismaDB.QueryAST
{
    public abstract class Query : ICloneable
    {
        public abstract override string ToString();
        public abstract object Clone();
    }
}
