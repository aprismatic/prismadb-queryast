using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DDL
{
    public class UseStatement : DdlQuery
    {
        public DatabaseRef Database;

        public UseStatement(DatabaseRef database)
        {
            Database = database;
        }

        public UseStatement(string database)
            : this(new DatabaseRef(database))
        { }

        public UseStatement(UseStatement other)
            : this(other.Database.Clone())
        { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override List<ConstantContainer> GetConstants() => new List<ConstantContainer>();

        public override string ToString() => DialectResolver.Dialect.UseStatementToString(this);

        public override object Clone() => new UseStatement(this);
    }
}
