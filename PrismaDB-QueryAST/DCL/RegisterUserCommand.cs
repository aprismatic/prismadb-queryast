using PrismaDB.QueryAST.DML;

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

        public override string ToString()
        {
            return DialectResolver.Dialect.RegisterUserCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new RegisterUserCommand(UserId.strvalue, Password.strvalue);

            return clone;
        }
    }
}