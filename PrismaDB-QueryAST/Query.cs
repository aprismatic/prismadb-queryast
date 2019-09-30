using PrismaDB.QueryAST.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST
{
    public abstract class Query : ICloneable
    {
        public void SetConstant(object value, string label = "")
        {
            if (value == null)
                throw new ArgumentNullException("Value cannot be null.");

            ConstantContainer target;

            if (String.IsNullOrEmpty(label))
                target = GetConstants().FirstOrDefault(x => x.constant is PlaceholderConstant);
            else
                target = GetConstants().FirstOrDefault(x => x.constant is PlaceholderConstant pc && pc.label == label);

            if (value is Constant constant)
                target.constant = constant;
            else
                target.constant = Constant.GetConstant(value);
        }

        public abstract List<TableRef> GetTables();
        public abstract List<ConstantContainer> GetConstants();
        public abstract override string ToString();
        public abstract object Clone();
    }
}
