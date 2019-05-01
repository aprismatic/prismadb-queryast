using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class FromClause : Clause
    {
        public List<FromTable> FromTables;

        public FromClause()
        {
            FromTables = new List<FromTable>();
        }

        public FromClause(FromClause other)
        {
            FromTables = new List<FromTable>(other.FromTables.Capacity);
            FromTables.AddRange(other.FromTables.Select(x => x.Clone() as FromTable));
        }

        public override object Clone()
        {
            return new FromClause(this);
        }

        public List<TableRef> GetTables()
        {
            return FromTables.SelectMany(x => x.GetTables()).ToList();
        }

        public override List<ColumnRef> GetColumns()
        {
            return FromTables.SelectMany(x => x.GetColumns()).ToList();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return FromTables.SelectMany(x => x.GetNoCopyColumns()).ToList();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FromClauseToString(this);
        }
    }

    public abstract class FromTable : Clause
    {
        public abstract List<TableRef> GetTables();
    }

    public abstract class SingleTable : FromTable { }

    public class TableRefSource : SingleTable
    {
        public TableRef Table { get; set; }

        public TableRefSource()
        {
            Table = new TableRef("");
        }

        public TableRefSource(TableRefSource other)
        {
            Table = other.Table.Clone();
        }

        public override object Clone()
        {
            return new TableRefSource(this);
        }

        public override List<TableRef> GetTables()
        {
            return new List<TableRef> { Table.Clone() };
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.TableRefSourceToString(this);
        }
    }

    public class SelectSubQuery : SingleTable
    {
        public Identifier Alias { get; set; }
        public SelectQuery Select { get; set; }

        public SelectSubQuery()
        {
            Select = new SelectQuery();
            Alias = new Identifier();
        }

        public SelectSubQuery(SelectSubQuery other)
        {
            Select = new SelectQuery(other.Select);
            Alias = other.Alias.Clone();
        }

        public override object Clone()
        {
            return new SelectSubQuery(this);
        }

        public override List<TableRef> GetTables()
        {
            return new List<TableRef>();
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.SelectSubQueryToString(this);
        }
    }

    public class JoinedTable : FromTable
    {
        public FromTable Table1;
        public FromTable Table2;
        public ColumnRef FirstColumn;
        public ColumnRef SecondColumn;
        public JoinType JoinType;

        public JoinedTable()
        {
            Table1 = new TableRefSource();
            Table2 = new TableRefSource();
            FirstColumn = new ColumnRef("");
            SecondColumn = new ColumnRef("");
            JoinType = JoinType.INNER;
        }

        public JoinedTable(JoinedTable other)
        {
            Table1 = other.Table1.Clone() as FromTable;
            Table2 = other.Table2.Clone() as FromTable;
            FirstColumn = other.FirstColumn.Clone() as ColumnRef;
            SecondColumn = other.SecondColumn.Clone() as ColumnRef;
            JoinType = other.JoinType;
        }

        public override object Clone()
        {
            return new JoinedTable(this);
        }

        public override List<TableRef> GetTables()
        {
            var res = new List<TableRef>();
            res.AddRange(Table1.GetTables());
            res.AddRange(Table2.GetTables());
            return res;
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            if (FirstColumn.ColumnName.id == "" || SecondColumn.ColumnName.id == "")
                return res;

            res.AddRange(FirstColumn.GetColumns());
            res.AddRange(SecondColumn.GetColumns());
            res.AddRange(Table1.GetColumns());
            res.AddRange(Table2.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            if (FirstColumn.ColumnName.id == "" || SecondColumn.ColumnName.id == "")
                return res;

            res.AddRange(FirstColumn.GetNoCopyColumns());
            res.AddRange(SecondColumn.GetNoCopyColumns());
            res.AddRange(Table1.GetNoCopyColumns());
            res.AddRange(Table2.GetNoCopyColumns());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.JoinedTableToString(this);
        }
    }

    public enum JoinType
    {
        INNER,
        LEFT_OUTER,
        RIGHT_OUTER,
        FULL_OUTER,
        CROSS
    }
}
