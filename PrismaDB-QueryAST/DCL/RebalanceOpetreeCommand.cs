using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class RebalanceOpetreeCommand : Command
    {
        public List<Constant> WithValues;
        public bool StatusCheck;

        public RebalanceOpetreeCommand(bool statusCheck = false)
        {
            WithValues = new List<Constant>();
            StatusCheck = statusCheck;
        }
        public RebalanceOpetreeCommand(List<Constant> withValues, bool statusCheck = false)
        {
            WithValues = new List<Constant>();
            foreach (var value in withValues)
                WithValues.Add(value);
            StatusCheck = statusCheck;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.RebalanceOpetreeCommandToString(this);

        public override object Clone() => new RebalanceOpetreeCommand(WithValues, StatusCheck);
    }
}