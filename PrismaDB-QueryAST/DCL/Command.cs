using System;

namespace PrismaDB.QueryAST.DCL
{
    public abstract class Command : ICloneable
    {
        public abstract object Clone();
    }
}
