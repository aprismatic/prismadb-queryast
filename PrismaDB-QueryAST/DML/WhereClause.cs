using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PrismaDB.QueryAST.DML
{
    public class WhereClause
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

        public override string ToString()
        {
            if (CNF.IsEmpty())
                return "";
            return " WHERE " + CNF.ToString();
        }

        public bool CheckDataRow(DataRow r)
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

        public List<ColumnRef> GetWhereColumns()
        {
            var whereCols = new List<ColumnRef>();
            foreach (var eachBE in CNF.AND.SelectMany(eachAND => eachAND.OR))
            {
                whereCols.AddRange(eachBE.GetColumns());
            }
            return whereCols.Distinct().ToList();
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
            var sb = new StringBuilder(" ( ");

            sb.Append(String.Join(" ) AND ( ", AND));

            sb.Append(" ) ");

            return sb.ToString();
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
                OR.Add((BooleanExpression)boolexp.Clone());
            }
        }

        public bool IsEmpty()
        {
            return (OR.Count == 0);
        }

        public override string ToString()
        {
            var sb = new StringBuilder(" ( ");

            sb.Append(String.Join(" ) OR ( ", OR));

            sb.Append(" ) ");

            return sb.ToString();
        }
    }
}
