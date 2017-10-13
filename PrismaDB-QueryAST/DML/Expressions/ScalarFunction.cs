using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class ScalarFunction : Expression
    {
        public Identifier FunctionName;
        public List<Expression> Parameters;

        public ScalarFunction(string functionName)
        {
            setValue(new Identifier(functionName), new List<Expression>());
        }

        public ScalarFunction(Identifier functionName)
        {
            setValue(functionName, new List<Expression>());
        }

        public ScalarFunction(string functionName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), parameters);
        }

        public ScalarFunction(Identifier functionName, List<Expression> parameters)
        {
            setValue(functionName, parameters);
        }

        public override Expression Clone()
        {
            var clone = new ScalarFunction(FunctionName, Parameters);

            return clone;
        }

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;

            if (Parameters.Count != otherF.Parameters.Count) return false;

            if (!FunctionName.Equals(otherF.FunctionName)) return false;

            return Parameters.Zip(otherF.Parameters, (x, y) => x.Equals(y)).Aggregate((x, y) => x && y);
        }

        public override object Eval(DataRow r)
        {
            throw new NotImplementedException("Functions can't currently be evaluated on proxy side.");
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override int GetHashCode()
        {
            return unchecked(FunctionName.GetHashCode() *
                Parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (Identifier)value[0];

            Parameters = value.Length > 1
                ? ((List<Expression>) value[1]).Select(x => x.Clone()).ToList()
                : new List<Expression>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ScalarFunctionToString(this);
        }
    }
}
