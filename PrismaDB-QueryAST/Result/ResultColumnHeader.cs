using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System;
using System.Runtime.Serialization;

namespace PrismaDB.QueryAST.Result
{
    [DataContract]
    public class ResultColumnHeader : PrismaDB.Result.ResultColumnHeader
    {
        private Expression _expression;
        private ColumnDefinition _columnDefinition;

        [IgnoreDataMember]
        public Expression Expression
        {
            get => _expression;

            set
            {
                _expression = value;
                if (value is ColumnRef colRef)
                    ColumnName = colRef.DisplayName();
                else
                    ColumnName = value.Alias.id;
            }
        }

        [IgnoreDataMember]
        public ColumnDefinition ColumnDefinition
        {
            get => _columnDefinition;

            set
            {
                _columnDefinition = value;
                DataType = DataTypes.GetDotNetType(_columnDefinition.DataType);
            }
        }

        public ResultColumnHeader() : this("") { }

        public ResultColumnHeader(string columnName, Type dataType = null, int? maxLength = null)
             : base(columnName, dataType, maxLength) { }

        public ResultColumnHeader(Expression exp, Type dataType = null, int? maxLength = null)
        {
            Expression = exp;
            MaxLength = maxLength;

            if (dataType != null)
                DataType = dataType;
        }

        public ResultColumnHeader(Expression exp, ColumnDefinition columnDef, Type dataType = null, int? maxLength = null)
        {
            Expression = exp;
            ColumnDefinition = columnDef;
            MaxLength = maxLength;

            if (dataType != null)
                DataType = dataType;
        }
    }
}
