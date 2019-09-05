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

        public ScalarFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : this(new Identifier(functionName), new Identifier(aliasName), parameters)
        { }

        public ScalarFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
        {
            FunctionName = functionName;
            Alias = alias ?? new Identifier();
            _parameters = new List<Expression>();
            if (parameters != null)
            {
                foreach (var v in parameters)
                    AddChild(v);
            }
        }

        public override object Clone()
        {
            var res = new ScalarFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public void SetChild(int index, Expression child)
        {
            child.Parent = this;
            _parameters[index] = child;
        }

        public void InsertChild(int index, Expression child)
        {
            child.Parent = this;
            _parameters.Insert(index, child);
        }

        public void RemoveChild(Expression child)
        {
            _parameters.Remove(child);
        }

        public void RemoveChildAt(int index)
        {
            _parameters.RemoveAt(index);
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

        public override object Clone()
        {
            var res = new PaillierAdditionFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new PaillierSubtractionFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new ElGamalMultiplicationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new ElGamalDivisionFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new SumAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new CountAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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

        public override object Clone()
        {
            var res = new AvgAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

        public override bool Equals(object other)
        {
            if (!(other is AvgAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.AvgAggregationFunctionToString(this);
    }

    public class MinAggregationFunction : ScalarFunction
    {
        public MinAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public MinAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            var res = new MinAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

        public override bool Equals(object other)
        {
            if (!(other is MinAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.MinAggregationFunctionToString(this);
    }

    public class MaxAggregationFunction : ScalarFunction
    {
        public MaxAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public MaxAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            var res = new MaxAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

        public override bool Equals(object other)
        {
            if (!(other is MaxAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.MaxAggregationFunctionToString(this);
    }

    public class StDevAggregationFunction : ScalarFunction
    {
        public StDevAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public StDevAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            var res = new StDevAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

        public override bool Equals(object other)
        {
            if (!(other is StDevAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.StDevAggregationFunctionToString(this);
    }

    public class LinRegAggregationFunction : ScalarFunction
    {
        public LinRegAggregationFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public LinRegAggregationFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            var res = new LinRegAggregationFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

        public override bool Equals(object other)
        {
            if (!(other is LinRegAggregationFunction otherF)) return false;

            return FunctionName.Equals(otherF.FunctionName) &&
                   Alias.Equals(otherF.Alias) &&
                   Parameters.SequenceEqual(otherF.Parameters);
        }

        public override string ToString() => DialectResolver.Dialect.LinRegAggregationFunctionToString(this);
    }

    public class PaillierAggregationSumFunction : ScalarFunction
    {
        public PaillierAggregationSumFunction(string functionName = "", string aliasName = "", ICollection<Expression> parameters = null)
            : base(functionName, aliasName, parameters) { }

        public PaillierAggregationSumFunction(Identifier functionName, Identifier alias = null, ICollection<Expression> parameters = null)
            : base(functionName, alias, parameters) { }

        public override object Clone()
        {
            var res = new PaillierAggregationSumFunction(FunctionName.Clone(), Alias.Clone());
            foreach (var v in Parameters)
                res.AddChild((Expression)v.Clone());
            return res;
        }

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
