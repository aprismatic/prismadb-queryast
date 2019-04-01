using System;
using System.Collections.Generic;
using PrismaDB.QueryAST.Result;

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
            Table = table.Clone();
        }

        public override object Clone() => new AllColumns(Table);

        public override object Eval(ResultRow r) =>
            throw new ApplicationException("AllColumns should not be in a WHERE clause.");

        public override List<ColumnRef> GetColumns() =>
            throw new ApplicationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");

        public override List<ColumnRef> GetNoCopyColumns() =>
            throw new ApplicationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");

        public override bool UpdateChild(Expression child, Expression newChild) =>
            throw new ApplicationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");

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
