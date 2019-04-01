using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public abstract class Expression : ICloneable
    {
        public Identifier Alias;

        public Expression Parent;

        public abstract object Clone();

        public abstract object Eval(ResultRow r);

        public abstract List<ColumnRef> GetColumns();

        public abstract List<ColumnRef> GetNoCopyColumns();

        public abstract bool UpdateChild(Expression child, Expression newChild);

        public abstract override string ToString();

        public abstract override bool Equals(object other);

        public abstract override int GetHashCode();
    }
}
