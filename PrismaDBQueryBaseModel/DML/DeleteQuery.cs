using System.Text;

namespace PrismaDBQueryBaseModel.DML
{
    public class DeleteQuery : DMLQuery
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

        public override string ToString()
        {
            var sb = new StringBuilder("DELETE FROM ");

            sb.Append(DeleteTable.ToString());

            sb.Append(" ");

            sb.Append(Where.ToString());

            return sb.ToString();
        }
    }
}
