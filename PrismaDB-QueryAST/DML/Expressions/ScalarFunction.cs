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

        public ScalarFunction()
        {
            FunctionName = new Identifier("");
            Alias = new Identifier("");
            Parameters = new List<Expression>();
        }

        public ScalarFunction(string functionName = "", string aliasName = "")
        {
            FunctionName = new Identifier(functionName);
            Alias = new Identifier(aliasName);
            Parameters = new List<Expression>();
        }

        public ScalarFunction(Identifier functionName, Identifier alias, List<Expression> parameters)
            : this(functionName.id, alias.id)
        {
            foreach (var v in parameters)
                Parameters.Add(v.Clone() as Expression);
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

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return Parameters.SelectMany(x => x.GetNoCopyColumns()).ToList();
        }

        public override int GetHashCode()
        {
            return unchecked(FunctionName.GetHashCode() * Alias.GetHashCode() *
                Parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ScalarFunctionToString(this);
        }
    }

    public class PaillierAdditionFunction : ScalarFunction
    {
        public PaillierAdditionFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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

    public class PaillierSubtractionFunction : ScalarFunction
    {
        public PaillierSubtractionFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
        public PaillierSubtractionFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public PaillierSubtractionFunction(string functionName, Expression left, Expression right, Expression N)
            : this(functionName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(N);
        }

        public PaillierSubtractionFunction(string functionName, Expression left, Expression right, Expression N, string aliasName)
            : this(functionName, aliasName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(N);
        }

        public override object Clone()
        {
            return new PaillierSubtractionFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is PaillierSubtractionFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.PaillierSubtractionFunctionToString(this);
        }
    }

    public class ElGamalMultiplicationFunction : ScalarFunction
    {
        public ElGamalMultiplicationFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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
    public class ElGamalDivisionFunction : ScalarFunction
    {
        public ElGamalDivisionFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
        public ElGamalDivisionFunction(Identifier functionName, Identifier alias, List<Expression> parameters) : base(functionName, alias, parameters) { }

        public ElGamalDivisionFunction(string functionName, Expression left, Expression right, Expression P)
            : this(functionName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(P);
        }

        public ElGamalDivisionFunction(string functionName, Expression left, Expression right, Expression P, string aliasName)
            : this(functionName, aliasName)
        {
            Parameters.Add(left);
            Parameters.Add(right);
            Parameters.Add(P);
        }

        public override object Clone()
        {
            return new ElGamalDivisionFunction(FunctionName, Alias, Parameters);
        }

        public override bool Equals(object other)
        {
            if (!(other is ElGamalDivisionFunction otherF)) return false;
            return (FunctionName.Equals(otherF.FunctionName)) &&
                   (Alias.Equals(otherF.Alias)) &&
                   (Parameters.SequenceEqual(otherF.Parameters));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ElGamalDivisionFunctionToString(this);
        }
    }

    public class SumAggregationFunction : ScalarFunction
    {
        public SumAggregationFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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
        public CountAggregationFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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
        public AvgAggregationFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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
        public PaillierAggregationSumFunction(string functionName = "", string aliasName = "") : base(functionName, aliasName) { }
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
