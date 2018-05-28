using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class JoinClause : Clause
    {
        public TableRef JoinTable;
        public ColumnRef FirstColumn;
        public ColumnRef SecondColumn;
        public JoinType JoinType;

        public JoinClause()
        {
            JoinTable = new TableRef("");
            FirstColumn = new ColumnRef("");
            SecondColumn = new ColumnRef("");
            JoinType = JoinType.LEFT;
        }

        public JoinClause(JoinClause other)
        {
            JoinTable = other.JoinTable.Clone();
            FirstColumn = other.FirstColumn.Clone() as ColumnRef;
            SecondColumn = other.SecondColumn.Clone() as ColumnRef;
            JoinType = other.JoinType;
        }

        public override object Clone()
        {
            return new JoinClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.JoinClauseToString(this);
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>
            {
                FirstColumn,
                SecondColumn
            };
            return res;
        }
    }

    public enum JoinType
    {
        LEFT
    }
}