using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            return new ScalarFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override object Eval(ResultRow r)
        {
            throw new NotImplementedException("Functions can't currently be evaluated on proxy side.");
        }

        public override List<ColumnRef> GetColumns()
        {
            return Parameters.SelectMany(x => x.GetColumns()).ToList();
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

    public class PaillierAdditionFunction : ScalarFunction
    {
        public PaillierAdditionFunction(string functionName) : base(functionName) { }
        public PaillierAdditionFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public PaillierAdditionFunction(Identifier functionName) : base(functionName) { }
        public PaillierAdditionFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public PaillierAdditionFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public PaillierAdditionFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public PaillierAdditionFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public PaillierAdditionFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public PaillierAdditionFunction(string functionName, Expression left, Expression right, Expression N)
            : this(functionName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(N);
        }

        public PaillierAdditionFunction(string functionName, Expression left, Expression right, Expression N, string aliasName)
            : this(functionName, aliasName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(N);
        }

        public override object Clone()
        {
            return new PaillierAdditionFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is PaillierAdditionFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.PaillierAdditionFunctionToString(this);
        }
    }

    public class ElGamalMultiplicationFunction : ScalarFunction
    {
        public ElGamalMultiplicationFunction(string functionName) : base(functionName) { }
        public ElGamalMultiplicationFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public ElGamalMultiplicationFunction(Identifier functionName) : base(functionName) { }
        public ElGamalMultiplicationFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public ElGamalMultiplicationFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public ElGamalMultiplicationFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public ElGamalMultiplicationFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public ElGamalMultiplicationFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public ElGamalMultiplicationFunction(string functionName, Expression left, Expression right, Expression P)
            : this(functionName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(P);
        }

        public ElGamalMultiplicationFunction(string functionName, Expression left, Expression right, Expression P, string aliasName)
            : this(functionName, aliasName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(P);
        }

        public override object Clone()
        {
            return new ElGamalMultiplicationFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is ElGamalMultiplicationFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ElGamalMultiplicationFunctionToString(this);
        }
    }

    public class SumAggregationFunction : ScalarFunction
    {
        public SumAggregationFunction(string functionName) : base(functionName) { }
        public SumAggregationFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public SumAggregationFunction(Identifier functionName) : base(functionName) { }
        public SumAggregationFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public SumAggregationFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public SumAggregationFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public SumAggregationFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public SumAggregationFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            return new SumAggregationFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is SumAggregationFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.SumAggregationFunctionToString(this);
        }
    }

    public class CountAggregationFunction : ScalarFunction
    {
        public CountAggregationFunction(string functionName) : base(functionName) { }
        public CountAggregationFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public CountAggregationFunction(Identifier functionName) : base(functionName) { }
        public CountAggregationFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public CountAggregationFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public CountAggregationFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public CountAggregationFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public CountAggregationFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            return new CountAggregationFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is CountAggregationFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.CountAggregationFunctionToString(this);
        }
    }

    public class AvgAggregationFunction : ScalarFunction
    {
        public AvgAggregationFunction(string functionName) : base(functionName) { }
        public AvgAggregationFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public AvgAggregationFunction(Identifier functionName) : base(functionName) { }
        public AvgAggregationFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public AvgAggregationFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public AvgAggregationFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public AvgAggregationFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public AvgAggregationFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            return new AvgAggregationFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is AvgAggregationFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.AvgAggregationFunctionToString(this);
        }
    }

    public class PaillierAggregationSumFunction : ScalarFunction
    {
        public PaillierAggregationSumFunction(string functionName) : base(functionName) { }
        public PaillierAggregationSumFunction(string functionName, string aliasName) : base(functionName, aliasName) { }
        public PaillierAggregationSumFunction(Identifier functionName) : base(functionName) { }
        public PaillierAggregationSumFunction(Identifier functionName, Identifier alias) : base(functionName, alias) { }
        public PaillierAggregationSumFunction(string functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public PaillierAggregationSumFunction(string functionName, string aliasName, List<Expression> parameters) : base(functionName, aliasName, parameters) { }
        public PaillierAggregationSumFunction(Identifier functionName, List<Expression> parameters) : base(functionName, parameters) { }
        public PaillierAggregationSumFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            return new PaillierAggregationSumFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is PaillierAggregationSumFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.PaillierAggregationSumFunctionToString(this);
        }
    }
}
