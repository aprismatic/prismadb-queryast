using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class RegisterUserCommand : Command
    {
        public StringConstant UserId;
        public StringConstant Password;

        public RegisterUserCommand()
        {
            UserId = new StringConstant("");
            Password = new StringConstant("");
        }

        public RegisterUserCommand(string userId, string password)
        {
            UserId = new StringConstant(userId);
            Password = new StringConstant(password);
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.RegisterUserCommandToString(this);

        public override object Clone() => new RegisterUserCommand(UserId.strvalue, Password.strvalue);
    }
}