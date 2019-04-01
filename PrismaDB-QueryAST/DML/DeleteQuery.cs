using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class DeleteQuery : DmlQuery
    {
        public TableRef DeleteTable;
        public WhereClause Where;

        public DeleteQuery()
        {
            DeleteTable = new TableRef("");
            Where = new WhereClause();
        }

        public DeleteQuery(DeleteQuery other)
        {
            DeleteTable = other.DeleteTable.Clone();
            Where = new WhereClause(other.Where);
        }

        public override List<TableRef> GetTables() => new List<TableRef> { DeleteTable.Clone()};

        public override string ToString() => DialectResolver.Dialect.DeleteQueryToString(this);

        public override object Clone() => new DeleteQuery(this);
    }
}
