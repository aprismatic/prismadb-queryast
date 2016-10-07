using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrismaDBQueryBaseModel.DML;

namespace PrismaDBQueryBaseModel.DDL
{
    public enum IndexType
    {
        CLUSTERED,
        NONCLUSTERED,
    }

    public class CreateIndexQuery : DDLQuery
    {
        public IndexType Type;
        public TableRef OnTable;
        public List<ColumnRef> OnColumns;
        public string Name;

        public CreateIndexQuery(IndexType type, string name, TableRef table, params ColumnRef[] columns)
        {
            Type = type;
            Name = name;
            OnTable = table.Clone();
            OnColumns = new List<ColumnRef>(columns.Length);
            OnColumns.AddRange(columns.Select(x => x.Clone() as ColumnRef));
        }

        public CreateIndexQuery(IndexType type, string name, string table, params ColumnRef[] columns)
            : this(type, name, new TableRef(table), columns)
        { }

        public CreateIndexQuery(CreateIndexQuery other)
            : this(other.Type, other.Name, other.OnTable, other.OnColumns.ToArray())
        { }

        public override string ToString()
        {
            var sb = new StringBuilder("CREATE ");

            sb.Append(Type.ToString());
            sb.Append(" INDEX [");
            sb.Append(Name);
            sb.Append("] ON ");
            sb.Append(OnTable.ToString());
            sb.Append(" ( ");
            sb.Append(String.Join(" , ", OnColumns.Select(x => x.ToString())));
            sb.Append(" ) ");

            return sb.ToString();
        }
    }
}
