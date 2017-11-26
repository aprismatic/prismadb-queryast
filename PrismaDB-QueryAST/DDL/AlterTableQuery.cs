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

    public class AlterTableQuery : DDLQuery
    {
        public TableRef TableName;
        public AlterType AlterType;
        public List<ColumnDefinition> ColumnDefinitions;

        public AlterTableQuery(AlterType type = AlterType.ADD)
            : this(type, new TableRef(""))
        { }

        public AlterTableQuery(AlterType type, string newTableName)
            : this(type, new TableRef(newTableName))
        { }

        public AlterTableQuery(AlterType type, TableRef newTable)
        {
            AlterType = type;
            TableName = newTable.Clone();
            ColumnDefinitions = new List<ColumnDefinition>();
        }

        public AlterTableQuery(AlterTableQuery other)
        {
            TableName = other.TableName.Clone();
            AlterType = other.AlterType;
            ColumnDefinitions = new List<ColumnDefinition>(other.ColumnDefinitions.Count);
            ColumnDefinitions.AddRange(other.ColumnDefinitions.Select(x => x.Clone() as ColumnDefinition));
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.AlterTableQueryToString(this);
        }

        public override object Clone()
        {
            return new AlterTableQuery(this);
        }
    }
}
