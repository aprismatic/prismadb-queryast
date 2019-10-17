using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class AllColumns : Expression
    {
        public TableRef Table;

        public AllColumns(string tableName)
        {
            Table = new TableRef(tableName);
        }

        public AllColumns()
            : this("")
        { }

        public AllColumns(TableRef table)
            : this()
        {
            Table = table;
        }

        public override object Clone() => new AllColumns(Table.Clone());

        public override object Eval(ResultRow r) =>
            throw new InvalidOperationException("AllColumns should not be in a WHERE clause.");

        public override List<ColumnRef> GetColumns() =>
            throw new InvalidOperationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override bool UpdateChild(Expression child, Expression newChild) =>
            throw new InvalidOperationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");

        public override string ToString() => DialectResolver.Dialect.AllColumnsToString(this);

        public override bool Equals(object other)
        {
            if (!(other is AllColumns otherCR)) return false;

            return Table.Equals(otherCR.Table);
        }

        public override int GetHashCode() =>
            unchecked(Table.GetHashCode());
    }
}
