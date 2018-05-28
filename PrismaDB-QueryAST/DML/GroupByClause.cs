using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class GroupByClause : ICloneable
    {
        public List<ColumnRef> GroupColumns;

        public GroupByClause()
        {
            GroupColumns = new List<ColumnRef>();
        }

        public GroupByClause(GroupByClause other)
        {
            GroupColumns = new List<ColumnRef>(other.GroupColumns.Capacity);
            GroupColumns.AddRange(other.GroupColumns);
        }

        public object Clone()
        {
            return new GroupByClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.GroupByClauseToString(this);
        }

        public List<ColumnRef> GetGroupByColumns()
        {
            return GroupColumns.Distinct().ToList();
        }
    }
}