using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class RebalanceOpetreeCommand : Command
    {
        public List<Constant> WithValues;

        public RebalanceOpetreeCommand()
        {
            WithValues = new List<Constant>();
        }
        public RebalanceOpetreeCommand(List<Constant> withValues)
        {
            WithValues = new List<Constant>();
            foreach (var value in withValues)
                WithValues.Add(value);
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.RebalanceOpetreeCommandToString(this);

        public override object Clone() => new RebalanceOpetreeCommand(WithValues);
    }
}