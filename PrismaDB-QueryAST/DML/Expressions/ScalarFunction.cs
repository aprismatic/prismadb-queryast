using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class ScalarFunction : Expression
    {
        public Identifier Function;
        public List<Expression> Parameters;

        public ScalarFunction(string functionName)
        {
            setValue(new Identifier(functionName), new Identifier(""), new List<Expression>());
        }

        public ScalarFunction(string functionName, string aliasName)
        {
            setValue(new Identifier(functionName), new Identifier(aliasName), new List<Expression>());
        }

        public ScalarFunction(Identifier function)
        {
            setValue(function, new Identifier(""), new List<Expression>());
        }

        public ScalarFunction(Identifier function, Identifier alias)
        {
            setValue(function, alias, new List<Expression>());
        }

        public ScalarFunction(string functionName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(""), parameters);
        }

        public ScalarFunction(string functionName, string aliasName, List<Expression> parameters)
        {
            setValue(new Identifier(functionName), new Identifier(aliasName), parameters);
        }

        public ScalarFunction(Identifier function, List<Expression> parameters)
        {
            setValue(function, new Identifier(""), parameters);
        }

        public ScalarFunction(Identifier function, Identifier alias, List<Expression> parameters)
        {
            setValue(function, alias, parameters);
        }

        public override object Clone()
        {
            var clone = new ScalarFunction(Function, Alias, Parameters);

            return clone;
        }

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;
            return (Function.Equals(otherF.Function)) &&
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
            return unchecked(Function.GetHashCode() * Alias.GetHashCode() *
                Parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            Function = (Identifier)value[0];
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
