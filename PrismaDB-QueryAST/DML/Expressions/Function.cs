using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public class Function : Expression
    {
        public string FunctionName;
        public List<Constant> Parameters;

        public Function(string functionName)
        {
            setValue(functionName, new List<Constant>);
        }

        public Function(string functionName, List<Constant> parameters)
        {
            setValue(functionName, parameters);
        }

        public override Expression Clone()
        {
            var clone = new Function(FunctionName, new List<Constant>(Parameters));

            return clone;
        }

        public override bool Equals(object other)
        {
            var otherF = other as Function;
            if (otherF == null) return false;

            return FunctionName.Equals(otherF.FunctionName)
                && Parameters.Equals(otherF.Parameters);
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
                FunctionName.GetHashCode() *
                Parameters.GetHashCode());
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (string)value[0];

            if (value.Length > 1)
                Parameters = (List<Constant>)value[1];
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FunctionToString(this);
        }
    }
}
