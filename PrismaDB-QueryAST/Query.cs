using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST
{
    public abstract class Query : ICloneable
    {
        public abstract List<TableRef> GetTables();
        public abstract override string ToString();
        public abstract object Clone();
    }
}
