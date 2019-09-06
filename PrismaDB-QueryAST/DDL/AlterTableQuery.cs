using PrismaDB.QueryAST.DML;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DDL
{
    public enum AlterType
    {
        ADD,
        DROP,
        MODIFY
    }

    public class AlterTableQuery : DdlQuery
    {
        public TableRef TableName;
        public AlterType AlterType;
        public List<AlteredColumn> AlteredColumns;

        public AlterTableQuery(AlterType type = AlterType.ADD)
            : this(type, new TableRef(""))
        { }

        public AlterTableQuery(AlterType type, string tableName)
            : this(type, new TableRef(tableName))
        { }

        public AlterTableQuery(AlterType type, TableRef table)
        {
            AlterType = type;
            TableName = table;
            AlteredColumns = new List<AlteredColumn>();
        }

        public AlterTableQuery(AlterTableQuery other)
        {
            TableName = other.TableName.Clone();
            AlterType = other.AlterType;
            AlteredColumns = new List<AlteredColumn>(other.AlteredColumns.Count);
            AlteredColumns.AddRange(other.AlteredColumns.Select(x => x.Clone() as AlteredColumn));
        }

        public override List<TableRef> GetTables() => new List<TableRef> { TableName };

        public override List<PlaceholderConstant> GetPlaceholders() => new List<PlaceholderConstant>();

        public override string ToString() => DialectResolver.Dialect.AlterTableQueryToString(this);

        public override object Clone() => new AlterTableQuery(this);
    }
}
