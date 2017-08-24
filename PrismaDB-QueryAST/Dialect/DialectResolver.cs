using System;

namespace PrismaDB.QueryAST
{
    public static class DialectResolver
    {
        private static IDialect _dialect;
        public static IDialect Dialect
        {
            get
            {
                if(_dialect == null)
                    throw new ApplicationException("Dialect should be set before using the AST classes.");
                return _dialect;
            }
            set => _dialect = value;
        }
    }
}
