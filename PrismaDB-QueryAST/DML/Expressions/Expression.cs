using System;
using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Expression : ICloneable
    {
        public Identifier Alias;

        public Expression Parent;

        public abstract void setValue(params object[] value);

        public abstract object Clone();

        public abstract object Eval(DataRow r);

        public abstract List<ColumnRef> GetColumns();

        public abstract override string ToString();

        public abstract override bool Equals(object other);

        public abstract override int GetHashCode();
    }
}
