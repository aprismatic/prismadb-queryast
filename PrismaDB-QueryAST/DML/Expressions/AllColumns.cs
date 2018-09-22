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
            setValue(tableName);
        }

        public AllColumns()
            : this("")
        { }

        public AllColumns(TableRef table)
            : this()
        {
            Table = table.Clone();
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            Table = new TableRef((string)value[0]);
        }

        public override object Clone()
        {
            var clone = new AllColumns(Table);

            return clone;
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
