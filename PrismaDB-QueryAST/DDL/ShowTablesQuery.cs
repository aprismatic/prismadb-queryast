using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DDL
{
    public class ShowTablesQuery : DdlQuery
    {
        public ShowTablesQuery() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.ShowTablesQueryToString(this);

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override object Clone() => new ShowTablesQuery();
    }
}
