using PrismaDB.QueryAST.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DDL
{
    [Flags]
    public enum ColumnEncryptionFlags
    {
        None = 0,
        Store = 1,
        Addition = 2,
        Multiplication = 4,
        Search = 8,
        Range = 16,
        Wildcard = 32
    }

    public class ColumnDefinition : ICloneable
    {
        public Identifier ColumnName;
        public SqlDataType DataType;
        public ColumnEncryptionFlags EncryptionFlags;
        public int? KeyVersion;
        public int? Length;
        public List<StringConstant> EnumValues;
        public bool Nullable;
        public bool IsRowId;
        public Expression DefaultValue;
        public bool AutoIncrement;
        public bool PrimaryKey;

        public ColumnDefinition()
            : this("", SqlDataType.MSSQL_INT)
        { }

        public ColumnDefinition(string columnName,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                int? keyVersion = null,
                                Expression defaultValue = null,
                                bool autoIncrement = false,
                                bool primaryKey = false)
            : this(new Identifier(columnName),
                   dataType,
                   length,
                   nullable,
                   isRowId,
                   encryptionFlags,
                   keyVersion,
                   defaultValue,
                   autoIncrement,
                   primaryKey)
        { }

        public ColumnDefinition(Identifier column,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                int? keyVersion = null,
                                Expression defaultValue = null,
                                bool autoIncrement = false,
                                bool primaryKey = false)
        {
            ColumnName = column == null ? new Identifier("") : column;
            DataType = dataType;
            EncryptionFlags = encryptionFlags;
            KeyVersion = keyVersion;
            Length = length;
            EnumValues = new List<StringConstant>();
            Nullable = nullable;
            IsRowId = isRowId;
            DefaultValue = defaultValue;
            AutoIncrement = autoIncrement;
            PrimaryKey = primaryKey;
        }

        public ColumnDefinition(ColumnDefinition other)
        {
            ColumnName = other.ColumnName;
            DataType = other.DataType;
            EncryptionFlags = other.EncryptionFlags;
            KeyVersion = other.KeyVersion;
            Length = other.Length;
            EnumValues = other.EnumValues.Select(item => (StringConstant)item).ToList();
            Nullable = other.Nullable;
            IsRowId = other.IsRowId;
            DefaultValue = other.DefaultValue;
            AutoIncrement = other.AutoIncrement;
            PrimaryKey = other.PrimaryKey;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ColumnDefinitionToString(this);
        }

        public object Clone()
        {
            return new ColumnDefinition(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is ColumnDefinition otherCD)) return false;

            return (ColumnName.Equals(otherCD.ColumnName))
                && (DataType == otherCD.DataType)
                && (EncryptionFlags == otherCD.EncryptionFlags)
                && (KeyVersion == otherCD.KeyVersion)
                && (Length == otherCD.Length)
                && (EnumValues.SequenceEqual(otherCD.EnumValues))
                && (Nullable == otherCD.Nullable)
                && (IsRowId == otherCD.IsRowId)
                && (Equals(DefaultValue, otherCD.DefaultValue))
                && (AutoIncrement == otherCD.AutoIncrement)
                && (PrimaryKey == otherCD.PrimaryKey);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnName.GetHashCode() *
                DataType.GetHashCode() *
                EncryptionFlags.GetHashCode() *
                (KeyVersion == null ? 1 : KeyVersion.GetHashCode()) *
                (Length == null ? 1 : Length.GetHashCode()) *
                EnumValues.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b) *
                (1 + Nullable.GetHashCode()) *
                (1 + IsRowId.GetHashCode()) *
                DefaultValue.GetHashCode() *
                (1 + AutoIncrement.GetHashCode()) *
                (1 + PrimaryKey.GetHashCode()));
        }
    }
}
