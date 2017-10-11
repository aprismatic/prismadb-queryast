using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public class Function : Expression
    {
        public string FunctionName;

        public Function(string functionName)
        {
            setValue(functionName);
        }

        public override Expression Clone()
        {
            var clone = new Function(FunctionName);

            return clone;
        }

        public override bool Equals(object other)
        {
            var otherF = other as Function;
            if (otherF == null) return false;

            return FunctionName.Equals(otherF.FunctionName);
        }

        public override object Eval(DataRow r)
        {
            return r[ToString()];
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override int GetHashCode()
        {
            return unchecked(
                FunctionName.GetHashCode());
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (string)value[0];
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FunctionToString(this);
        }
    }
}
