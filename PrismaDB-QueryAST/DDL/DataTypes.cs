using System;

namespace PrismaDB.QueryAST.DDL
{
    public enum SqlDataType
    {
        MSSQL_INT = 0,
        MSSQL_TINYINT = 1,
        MSSQL_SMALLINT = 2,
        MSSQL_BIGINT = 3,
        MSSQL_FLOAT = 4,
        MSSQL_DECIMAL = 5,

        MSSQL_DATE = 100,
        MSSQL_DATETIME = 101,

        MSSQL_CHAR = 200,
        MSSQL_VARCHAR = 201,
        MSSQL_TEXT = 202,
        MSSQL_NCHAR = 203,
        MSSQL_NVARCHAR = 204,
        MSSQL_NTEXT = 205,

        MSSQL_BINARY = 300,
        MSSQL_VARBINARY = 301,

        MSSQL_UNIQUEIDENTIFIER = 400,


        MySQL_INT = 1000,
        MySQL_TINYINT = 1001,
        MySQL_SMALLINT = 1002,
        MySQL_BIGINT = 1003,
        MySQL_DOUBLE = 1004,
        MySQL_FLOAT = 1005,
        MySQL_DECIMAL = 1006,

        MySQL_DATE = 1100,
        MySQL_DATETIME = 1101,
        MySQL_TIMESTAMP = 1102,

        MySQL_CHAR = 1200,
        MySQL_VARCHAR = 1201,
        MySQL_TEXT = 1202,

        MySQL_BINARY = 1300,
        MySQL_VARBINARY = 1301,
        MySQL_BLOB = 1302,

        MySQL_ENUM = 1400,


        Postgres_INT4 = 2000,
        Postgres_INT2 = 2001,
        Postgres_INT8 = 2002,
        Postgres_FLOAT8 = 2003,
        Postgres_FLOAT4 = 2004,
        Postgres_DECIMAL = 2005,

        Postgres_DATE = 2100,
        Postgres_TIMESTAMP = 2101,

        Postgres_CHAR = 2200,
        Postgres_VARCHAR = 2201,
        Postgres_TEXT = 2202,

        Postgres_BYTEA = 2300
    }

    public static class DataTypes
    {
        public static Type GetDotNetType(SqlDataType type)
        {
            switch (type)
            {
                case SqlDataType.MSSQL_INT:
                case SqlDataType.MySQL_INT:
                case SqlDataType.Postgres_INT4:
                    return typeof(Int32);
                case SqlDataType.MSSQL_BIGINT:
                case SqlDataType.MySQL_BIGINT:
                case SqlDataType.Postgres_INT8:
                    return typeof(Int64);
                case SqlDataType.MSSQL_SMALLINT:
                case SqlDataType.MySQL_SMALLINT:
                case SqlDataType.Postgres_INT2:
                    return typeof(Int16);
                case SqlDataType.MSSQL_TINYINT:
                    return typeof(Byte);
                case SqlDataType.MySQL_TINYINT:
                    return typeof(SByte);
                case SqlDataType.MSSQL_UNIQUEIDENTIFIER:
                    return typeof(Guid);
                case SqlDataType.MSSQL_DATETIME:
                case SqlDataType.MySQL_DATETIME:
                case SqlDataType.Postgres_TIMESTAMP:
                    return typeof(DateTime);
                case SqlDataType.MySQL_FLOAT:
                case SqlDataType.Postgres_FLOAT4:
                    return typeof(Single);
                case SqlDataType.MSSQL_FLOAT:
                case SqlDataType.MySQL_DOUBLE:
                case SqlDataType.Postgres_FLOAT8:
                    return typeof(Double);
                case SqlDataType.MSSQL_DECIMAL:
                case SqlDataType.MySQL_DECIMAL:
                case SqlDataType.Postgres_DECIMAL:
                    return typeof(Decimal);
                case SqlDataType.MSSQL_CHAR:
                case SqlDataType.MySQL_CHAR:
                case SqlDataType.Postgres_CHAR:
                case SqlDataType.MSSQL_NCHAR:
                case SqlDataType.MSSQL_VARCHAR:
                case SqlDataType.MySQL_VARCHAR:
                case SqlDataType.Postgres_VARCHAR:
                case SqlDataType.MSSQL_NVARCHAR:
                case SqlDataType.MSSQL_TEXT:
                case SqlDataType.MySQL_TEXT:
                case SqlDataType.Postgres_TEXT:
                case SqlDataType.MSSQL_NTEXT:
                case SqlDataType.MySQL_TIMESTAMP:
                case SqlDataType.MySQL_ENUM:
                case SqlDataType.MSSQL_DATE:
                case SqlDataType.MySQL_DATE:
                case SqlDataType.Postgres_DATE:
                    return typeof(String);
                case SqlDataType.MSSQL_BINARY:
                case SqlDataType.MSSQL_VARBINARY:
                case SqlDataType.MySQL_BINARY:
                case SqlDataType.MySQL_VARBINARY:
                case SqlDataType.MySQL_BLOB:
                case SqlDataType.Postgres_BYTEA:
                    return typeof(Byte[]);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlDataType(Type type, TargetDatabase target)
        {
            if (type == typeof(byte[]))
                return GetSqlVarBinaryDataType(target);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32:
                    return GetSqlIntDataType(target);
                case TypeCode.Int16:
                    return GetSqlSmallIntDataType(target);
                case TypeCode.Int64:
                    return GetSqlBigIntDataType(target);
                case TypeCode.Single:
                    return GetSqlFloatDataType(target);
                case TypeCode.Double:
                    return GetSqlDoubleDataType(target);
                case TypeCode.Decimal:
                    return GetSqlDecimalDataType(target);
                case TypeCode.String:
                    return GetSqlVarCharDataType(target);
                case TypeCode.DateTime:
                    return GetSqlDateTimeDataType(target);
            }

            throw new ArgumentOutOfRangeException();
        }

        public static SqlDataType GetSqlIntDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_INT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_INT;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_INT4;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlSmallIntDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_SMALLINT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_SMALLINT;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_INT2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlBigIntDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_BIGINT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_BIGINT;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_INT8;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlFloatDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_FLOAT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_FLOAT;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_FLOAT4;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlDoubleDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_FLOAT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_DOUBLE;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_FLOAT8;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlDecimalDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_DECIMAL;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_DECIMAL;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_DECIMAL;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlBinaryDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_BINARY;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_BINARY;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_BYTEA;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlVarBinaryDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_VARBINARY;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_VARBINARY;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_BYTEA;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlVarCharDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_VARCHAR;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_VARCHAR;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_VARCHAR;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlTextDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_TEXT;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_TEXT;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_TEXT;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static SqlDataType GetSqlDateTimeDataType(TargetDatabase target)
        {
            switch (target)
            {
                case TargetDatabase.MS_SQL_Server:
                    return SqlDataType.MSSQL_DATETIME;
                case TargetDatabase.MySQL:
                    return SqlDataType.MySQL_DATETIME;
                case TargetDatabase.PostgreSQL:
                case TargetDatabase.CockroachDB:
                    return SqlDataType.Postgres_TIMESTAMP;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
