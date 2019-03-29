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
            Table = table.Clone();
        }

        public override object Clone()
        {
            return new AllColumns(Table);
        }

        public override object Eval(ResultRow r)
        {
            throw new ApplicationException("AllColumns should not be in a WHERE clause.");
        }

        public override List<ColumnRef> GetColumns()
        {
            throw new ApplicationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            throw new ApplicationException("AllColumns needs to be replaced with corresponding ColumnRefs in table.");
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.AllColumnsToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is AllColumns otherCR)) return false;

            return Table.Equals(otherCR.Table);
        }

        public override int GetHashCode()
        {
            return Table.GetHashCode();
        }
    }
}
