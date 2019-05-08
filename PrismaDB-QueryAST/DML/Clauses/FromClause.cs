using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class FromClause : Clause
    {
        public List<FromSource> Sources;

        public FromClause()
        {
            Sources = new List<FromSource>();
        }

        public FromClause(FromClause other)
        {
            Sources = new List<FromSource>(other.Sources.Capacity);
            Sources.AddRange(other.Sources.Select(x => x.Clone() as FromSource));
        }

        public override object Clone()
        {
            return new FromClause(this);
        }

        public List<TableRef> GetTables()
        {
            return Sources.SelectMany(x => x.GetTables()).ToList();
        }

        public override List<ColumnRef> GetColumns()
        {
            return Sources.SelectMany(x => x.GetColumns()).ToList();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return Sources.SelectMany(x => x.GetNoCopyColumns()).ToList();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FromClauseToString(this);
        }
    }

    public class FromSource : Clause
    {
        public SingleTable FirstTable { get; set; }
        public List<JoinedTable> JoinedTables;

        public FromSource(string tableName = "")
        {
            FirstTable = new TableSource(tableName);
            JoinedTables = new List<JoinedTable>();
        }

        public FromSource(TableRef table)
        {
            FirstTable = new TableSource(table);
            JoinedTables = new List<JoinedTable>();
        }

        public FromSource(SelectSubQuery subQuery)
        {
            FirstTable = subQuery;
            JoinedTables = new List<JoinedTable>();
        }

        public FromSource(FromSource other)
        {
            FirstTable = other.FirstTable.Clone() as SingleTable;
            JoinedTables = new List<JoinedTable>();
            JoinedTables.AddRange(other.JoinedTables.Select(x => x.Clone() as JoinedTable));
        }

        public override object Clone()
        {
            return new FromSource(this);
        }

        public List<TableRef> GetTables()
        {
            var res = new List<TableRef>();
            res.AddRange(FirstTable.GetTables());
            res.AddRange(JoinedTables.SelectMany(x => x.GetTables()).ToList());
            return res;
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(FirstTable.GetColumns());
            res.AddRange(JoinedTables.SelectMany(x => x.GetColumns()).ToList());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(FirstTable.GetNoCopyColumns());
            res.AddRange(JoinedTables.SelectMany(x => x.GetNoCopyColumns()).ToList());
            return res;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.FromSourceToString(this);
        }
    }

    public abstract class SingleTable : Clause
    {
        public abstract List<TableRef> GetTables();
    }

    public class TableSource : SingleTable
    {
        public TableRef Table { get; set; }

        public TableSource(string tableName = "")
        {
            Table = new TableRef(tableName);
        }

        public TableSource(TableRef table)
        {
            Table = table;
        }

        public TableSource(TableSource other)
        {
            Table = other.Table.Clone();
        }

        public override object Clone()
        {
            return new TableSource(this);
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
            return DialectResolver.Dialect.TableSourceToString(this);
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

        public SelectSubQuery(SelectQuery select, string alias = "")
        {
            Select = new SelectQuery(select);
            Alias = new Identifier(alias);
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

    public class JoinedTable : Clause
    {
        public SingleTable SecondTable;
        public ColumnRef FirstColumn;
        public ColumnRef SecondColumn;
        public JoinType JoinType;

        public JoinedTable()
        {
            SecondTable = new TableSource();
            FirstColumn = new ColumnRef("");
            SecondColumn = new ColumnRef("");
            JoinType = JoinType.INNER;
        }

        public JoinedTable(JoinedTable other)
        {
            SecondTable = other.SecondTable.Clone() as SingleTable;
            FirstColumn = other.FirstColumn.Clone() as ColumnRef;
            SecondColumn = other.SecondColumn.Clone() as ColumnRef;
            JoinType = other.JoinType;
        }

        public override object Clone()
        {
            return new JoinedTable(this);
        }

        public List<TableRef> GetTables()
        {
            return SecondTable.GetTables();
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            if (FirstColumn.ColumnName.id != "")
                res.AddRange(FirstColumn.GetColumns());
            if (SecondColumn.ColumnName.id != "")
                res.AddRange(SecondColumn.GetColumns());
            res.AddRange(SecondTable.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            if (FirstColumn.ColumnName.id != "")
                res.AddRange(FirstColumn.GetNoCopyColumns());
            if (SecondColumn.ColumnName.id != "")
                res.AddRange(SecondColumn.GetNoCopyColumns());
            res.AddRange(SecondTable.GetNoCopyColumns());
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
