using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Clause : ICloneable
    {
        public abstract object Clone();

        public abstract List<ColumnRef> GetColumns();

        public abstract override string ToString();
    }
}
