﻿using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnHeader
    {
        private Expression _expression;
        private ColumnDefinition _columnDefinition;

        public string ColumnName { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int? MaxLength { get; set; }
        
        [JsonIgnore]
        [XmlIgnore]
        public Type DataType { get; set; }

        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        public ColumnDefinition ColumnDefinition
        {
            get => _columnDefinition;

            set
            {
                _columnDefinition = value;

                switch (_columnDefinition.DataType)
                {
                    case SqlDataType.MSSQL_INT:
                    case SqlDataType.MYSQL_INT:
                        DataType = typeof(Int32);
                        break;
                    case SqlDataType.MSSQL_BIGINT:
                    case SqlDataType.MYSQL_BIGINT:
                        DataType = typeof(Int64);
                        break;
                    case SqlDataType.MSSQL_SMALLINT:
                    case SqlDataType.MYSQL_SMALLINT:
                        DataType = typeof(Int16);
                        break;
                    case SqlDataType.MSSQL_TINYINT:
                        DataType = typeof(Byte);
                        break;
                    case SqlDataType.MYSQL_TINYINT:
                        DataType = typeof(SByte);
                        break;
                    case SqlDataType.MSSQL_UNIQUEIDENTIFIER:
                        DataType = typeof(Guid);
                        break;
                    case SqlDataType.MSSQL_DATETIME:
                    case SqlDataType.MYSQL_DATETIME:
                        DataType = typeof(DateTime);
                        break;
                    case SqlDataType.MSSQL_FLOAT:
                    case SqlDataType.MYSQL_DOUBLE:
                        DataType = typeof(Double);
                        break;
                    case SqlDataType.MSSQL_CHAR:
                    case SqlDataType.MYSQL_CHAR:
                    case SqlDataType.MSSQL_NCHAR:
                    case SqlDataType.MSSQL_VARCHAR:
                    case SqlDataType.MYSQL_VARCHAR:
                    case SqlDataType.MSSQL_NVARCHAR:
                    case SqlDataType.MSSQL_TEXT:
                    case SqlDataType.MYSQL_TEXT:
                    case SqlDataType.MSSQL_NTEXT:
                    case SqlDataType.MYSQL_TIMESTAMP:
                    case SqlDataType.MYSQL_ENUM:
                    case SqlDataType.MSSQL_DATE:
                    case SqlDataType.MYSQL_DATE:
                        DataType = typeof(String);
                        break;
                    case SqlDataType.MSSQL_BINARY:
                    case SqlDataType.MSSQL_VARBINARY:
                    case SqlDataType.MYSQL_BINARY:
                    case SqlDataType.MYSQL_VARBINARY:
                    case SqlDataType.MYSQL_BLOB:
                        DataType = typeof(byte[]);
                        break;
                    default:
                        throw new ApplicationException("DataType not supported in DataTable.");
                }
            }
        }

        public ResultColumnHeader() : this("") { }

        public ResultColumnHeader(string columnName, Type dataType = null, int? maxLength = null)
        {
            ColumnName = columnName;
            DataType = dataType;
            MaxLength = maxLength;
        }

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

        public void RemoveMetadata()
        {
            _expression = null;
            _columnDefinition = null;
        }
    }
}
