using System.Collections.Generic;
using System.Data;

namespace PrismaDBQueryBaseModel.DML
{
    public abstract class Expression
    {
        public string ColumnName;

        public Expression Parent;

        public abstract void setValue(params object[] value);

        public abstract Expression Clone();

        public abstract object Eval(DataRow r);

        public abstract List<ColumnRef> GetColumns();

        public abstract override string ToString();
    }
}
