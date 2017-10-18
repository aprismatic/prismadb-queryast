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
            setValue(new Identifier(functionName), new Identifier(""), new List<Expression>());
        }

        public ScalarFunction(string functionName, string columnName)
        {
            setValue(new Identifier(functionName), new Identifier(columnName), new List<Expression>());
        }

        public ScalarFunction(Identifier functionName)
        {
            setValue(functionName, new Identifier(""), new List<Expression>());
        }

        public ScalarFunction(Identifier functionName, Identifier columnName)
        {
            setValue(functionName, columnName, new List<Expression>());
        }

        public ScalarFunction(string functionName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(""), parameters);
        }

        public ScalarFunction(string functionName, string columnName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(columnName), parameters);
        }

        public ScalarFunction(Identifier functionName, List<Expression> parameters)
        {
            setValue(functionName, new Identifier(""), parameters);
        }

        public ScalarFunction(Identifier functionName, Identifier columnName, List<Expression> parameters)
        {
            setValue(functionName, columnName, parameters);
        }

        public override Expression Clone()
        {
            var clone = new ScalarFunction(FunctionName, ColumnName, Parameters);

            return clone;
        }

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;

            if (Parameters.Count != otherF.Parameters.Count) return false;

            if (!FunctionName.Equals(otherF.FunctionName)) return false;

            if (!ColumnName.Equals(otherF.ColumnName)) return false;

            if (Parameters.Count == 0) return true;

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
            return unchecked(FunctionName.GetHashCode() * ColumnName.GetHashCode() *
                Parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (Identifier)value[0];
            ColumnName = new Identifier("");
            Parameters = new List<Expression>();

            if (value.Length > 1)
            {
                if (value[1].GetType() == typeof(Identifier))
                    ColumnName = (Identifier)value[1];
                else
                    Parameters = ((List<Expression>)value[1]).Select(x => x.Clone()).ToList();
                if (value.Length > 2)
                    Parameters = ((List<Expression>)value[2]).Select(x => x.Clone()).ToList();
            }
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ScalarFunctionToString(this);
        }
    }
}
