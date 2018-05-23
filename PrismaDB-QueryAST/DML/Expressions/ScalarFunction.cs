using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
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

        public ScalarFunction(string functionName, string aliasName)
        {
            setValue(new Identifier(functionName), new Identifier(aliasName), new List<Expression>());
        }

        public ScalarFunction(Identifier functionName)
        {
            setValue(functionName, new Identifier(""), new List<Expression>());
        }

        public ScalarFunction(Identifier functionName, Identifier alias)
        {
            setValue(functionName, alias, new List<Expression>());
        }

        public ScalarFunction(string functionName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(""), parameters);
        }

        public ScalarFunction(string functionName, string aliasName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(aliasName), parameters);
        }

        public ScalarFunction(Identifier functionName, List<Expression> parameters)
        {
            setValue(functionName, new Identifier(""), parameters);
        }

        public ScalarFunction(Identifier functionName, Identifier alias, List<Expression> parameters)
        {
            setValue(functionName, alias, parameters);
        }

        public override object Clone()
        {
            var clone = new ScalarFunction(FunctionName, Alias, Parameters);

            return clone;
        }

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
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
            return unchecked(FunctionName.GetHashCode() * Alias.GetHashCode() *
                Parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (Identifier)value[0];
            Alias = new Identifier("");
            Parameters = new List<Expression>();

            if (value.Length > 1)
            {
                if (value[1].GetType() == typeof(Identifier))
                {
                    Alias = (Identifier)value[1];
                }
                else
                {
                    Parameters = ((List<Expression>)value[1]).Select(x => x.Clone() as Expression).ToList();
                }

                if (value.Length > 2)
                {
                    Parameters = ((List<Expression>)value[2]).Select(x => x.Clone() as Expression).ToList();
                }
            }
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ScalarFunctionToString(this);
        }
    }
}
