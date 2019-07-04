using PrismaDB.QueryAST.Result;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class WhereClause : Clause
    {
        public ConjunctiveNormalForm CNF;

        public WhereClause(BooleanExpression ex)
        {
            CNF = new ConjunctiveNormalForm();
            var disj = new Disjunction();
            disj.OR.Add(ex);
            CNF.AND.Add(disj);
        }

        public WhereClause()
        {
            CNF = new ConjunctiveNormalForm();
        }

        public WhereClause(WhereClause other)
        {
            CNF = new ConjunctiveNormalForm(other.CNF);
        }

        public override object Clone()
        {
            return new WhereClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.WhereClauseToString(this);
        }

        public bool CheckDataRow(ResultRow r)
        {
            foreach (var eachAND in CNF.AND)
            {
                // eachAND.OR is a list of BooleanExpression
                var eachANDbool = (eachAND.OR).Any(c => (bool)c.Eval(r));
                if (!eachANDbool)
                    return false;
            }
            return true;
        }

        public override List<ColumnRef> GetColumns()
        {
            var whereCols = new List<ColumnRef>();
            foreach (var eachBE in CNF.AND.SelectMany(eachAND => eachAND.OR))
            {
                whereCols.AddRange(eachBE.GetColumns());
            }
            return whereCols;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var whereCols = new List<ColumnRef>();
            foreach (var eachBE in CNF.AND.SelectMany(eachAND => eachAND.OR))
            {
                whereCols.AddRange(eachBE.GetNoCopyColumns());
            }
            return whereCols;
        }
    }

    public class ConjunctiveNormalForm
    {
        public List<Disjunction> AND;

        public ConjunctiveNormalForm()
        {
            AND = new List<Disjunction>();
        }

        public ConjunctiveNormalForm(ConjunctiveNormalForm other)
        {
            AND = new List<Disjunction>(other.AND.Count);

            foreach (var disj_copy in other.AND.Select(disj => new Disjunction(disj)))
            {
                AND.Add(disj_copy);
            }
        }

        public bool IsEmpty()
        {
            return (AND.Count == 0);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ConjunctiveNormalFormToString(this);
        }
    }

    public class Disjunction
    {
        public List<BooleanExpression> OR;

        public Disjunction()
        {
            OR = new List<BooleanExpression>();
        }

        public Disjunction(Disjunction other)
        {
            OR = new List<BooleanExpression>(other.OR.Count);

            foreach (var boolexp in other.OR)
            {
                OR.Add((BooleanExpression)boolexp);
            }
        }

        public bool IsEmpty()
        {
            return (OR.Count == 0);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DisjunctionToString(this);
        }
    }
}
