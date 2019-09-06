using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class GroupByClause : Clause
    {
        public List<ColumnRef> GroupColumns;

        public GroupByClause()
        {
            GroupColumns = new List<ColumnRef>();
        }

        public GroupByClause(GroupByClause other)
        {
            GroupColumns = new List<ColumnRef>(other.GroupColumns.Capacity);
            GroupColumns.AddRange(other.GroupColumns.Select(x => x.Clone() as ColumnRef));
        }

        public override object Clone()
        {
            return new GroupByClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.GroupByClauseToString(this);
        }

        public override List<ColumnRef> GetColumns()
        {
            return GroupColumns.SelectMany(x => x.GetColumns()).ToList();
        }

        public override List<PlaceholderConstant> GetPlaceholders() => new List<PlaceholderConstant>();
    }
}