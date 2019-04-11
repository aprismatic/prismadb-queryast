using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class ScalarFunction : Expression
    {
        public Identifier FunctionName;
        protected List<Expression> _parameters;
        public ReadOnlyCollection<Expression> Parameters => _parameters.AsReadOnly();

        public ScalarFunction()
        {
            FunctionName = new Identifier("");
            Alias = new Identifier("");
            _parameters = new List<Expression>();
        }

        public ScalarFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
        {
            FunctionName = new Identifier(functionName);
            Alias = new Identifier(aliasName);
            _parameters = new List<Expression>();
            if (parameters != null)
            {
                foreach (var v in parameters)
                    AddChild(v.Clone() as Expression);
            }
        }

        public ScalarFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : this(functionName.id, alias?.id ?? "", parameters) { }

        public override object Clone() => new ScalarFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is ScalarFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName)
                && Alias.Equals(otherF.Alias)
                && Parameters.SequenceEqual(otherF.Parameters);
        }

        public override object Eval(ResultRow r) =>
            throw new NotImplementedException("Functions can't currently be evaluated on proxy side.");

        public override List<ColumnRef> GetColumns() => Parameters.SelectMany(x => x.GetColumns()).ToList();

        public override List<ColumnRef> GetNoCopyColumns() => Parameters.SelectMany(x => x.GetNoCopyColumns()).ToList();

        public override bool UpdateChild(Expression child, Expression newChild)
        {
            for (var i = 0; i < Parameters.Count; i++)
            {
                if (_parameters[i] == child) // ReSharper disable once PossibleUnintendedReferenceComparison
                {
                    _parameters[i] = newChild;
                    newChild.Parent = this;
                    return true;
                }
            }

            return false;
        }

        public void AddChild(Expression child)
        {
            child.Parent = this;
            _parameters.Add(child);
        }

        public void AddChild(int index, Expression child)
        {
            child.Parent = this;
            _parameters[index] = child;
        }

        public override int GetHashCode() =>
            unchecked(FunctionName.GetHashCode() *
                      Alias.GetHashCode() *
                      _parameters.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b));

        public override string ToString() => DialectResolver.Dialect.ScalarFunctionToString(this);
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class PaillierAdditionFunction : ScalarFunction
    {
        public PaillierAdditionFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public PaillierAdditionFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public PaillierAdditionFunction(string functionName, Expression left, Expression right, Expression N)
            : this(functionName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(N);
        }

        public PaillierAdditionFunction(string functionName, Expression left, Expression right, Expression N, string aliasName)
            : this(functionName, aliasName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(N);
        }

        public override object Clone() => new PaillierAdditionFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is PaillierAdditionFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.PaillierAdditionFunctionToString(this);
    }

    public class PaillierSubtractionFunction : ScalarFunction
    {
        public PaillierSubtractionFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public PaillierSubtractionFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public PaillierSubtractionFunction(string functionName, Expression left, Expression right, Expression N)
            : this(functionName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(N);
        }

        public PaillierSubtractionFunction(string functionName, Expression left, Expression right, Expression N, string aliasName)
            : this(functionName, aliasName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(N);
        }

        public override object Clone() => new PaillierSubtractionFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is PaillierSubtractionFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.PaillierSubtractionFunctionToString(this);
    }

    public class ElGamalMultiplicationFunction : ScalarFunction
    {
        public ElGamalMultiplicationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public ElGamalMultiplicationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public ElGamalMultiplicationFunction(string functionName, Expression left, Expression right, Expression P)
            : this(functionName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(P);
        }

        public ElGamalMultiplicationFunction(string functionName, Expression left, Expression right, Expression P, string aliasName)
            : this(functionName, aliasName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(P);
        }

        public override object Clone() => new ElGamalMultiplicationFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is ElGamalMultiplicationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.ElGamalMultiplicationFunctionToString(this);
    }

    public class ElGamalDivisionFunction : ScalarFunction
    {
        public ElGamalDivisionFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public ElGamalDivisionFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public ElGamalDivisionFunction(string functionName, Expression left, Expression right, Expression P)
            : this(functionName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(P);
        }

        public ElGamalDivisionFunction(string functionName, Expression left, Expression right, Expression P, string aliasName)
            : this(functionName, aliasName)
        {
            AddChild(left);
            AddChild(right);
            AddChild(P);
        }

        public override object Clone() => new ElGamalDivisionFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is ElGamalDivisionFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.ElGamalDivisionFunctionToString(this);
    }

    public class SumAggregationFunction : ScalarFunction
    {
        public SumAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public SumAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone() => new SumAggregationFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is SumAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.SumAggregationFunctionToString(this);
    }

    public class CountAggregationFunction : ScalarFunction
    {
        public CountAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public CountAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone() => new CountAggregationFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is CountAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.CountAggregationFunctionToString(this);
    }

    public class AvgAggregationFunction : ScalarFunction
    {
        public AvgAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public AvgAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone() => new AvgAggregationFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is AvgAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.AvgAggregationFunctionToString(this);
    }

    public class StDevAggregationFunction : ScalarFunction
    {
        public StDevAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public StDevAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone() => new StDevAggregationFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is StDevAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.StDevAggregationFunctionToString(this);
    }

    public class PaillierAggregationSumFunction : ScalarFunction
    {
        public PaillierAggregationSumFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public PaillierAggregationSumFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone() => new PaillierAggregationSumFunction(FunctionName, Alias, Parameters);

        public override bool Equals(object other)
        {
            if (!(other is PaillierAggregationSumFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.PaillierAggregationSumFunctionToString(this);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}
