﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class OrderByClause : Clause
    {
        public List<Tuple<ColumnRef, OrderDirection>> OrderColumns;

        public OrderByClause()
        {
            OrderColumns = new List<Tuple<ColumnRef, OrderDirection>>();
        }

        public OrderByClause(OrderByClause other)
        {
            OrderColumns = new List<Tuple<ColumnRef, OrderDirection>>(other.OrderColumns.Capacity);
            OrderColumns.AddRange(other.OrderColumns.Select(
                x => new Tuple<ColumnRef, OrderDirection>(x.Item1.Clone() as ColumnRef, x.Item2)));
        }

        public override object Clone()
        {
            return new OrderByClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.OrderByClauseToString(this);
        }

        public override List<ColumnRef> GetColumns()
        {
            return OrderColumns.SelectMany(x => x.Item1.GetColumns()).ToList();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return OrderColumns.SelectMany(x => x.Item1.GetNoCopyColumns()).ToList();
        }
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}