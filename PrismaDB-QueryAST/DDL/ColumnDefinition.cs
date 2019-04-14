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
                                bool autoIncrement = false)
            : this(new Identifier(columnName),
                   dataType,
                   length,
                   nullable,
                   isRowId,
                   encryptionFlags,
                   keyVersion,
                   defaultValue,
                   autoIncrement)
        { }

        public ColumnDefinition(Identifier column,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                int? keyVersion = null,
                                Expression defaultValue = null,
                                bool autoIncrement = false)
        {
            this.ColumnName = column == null ? new Identifier("") : column.Clone();
            this.DataType = dataType;
            this.EncryptionFlags = encryptionFlags;
            this.KeyVersion = keyVersion;
            this.Length = length;
            this.EnumValues = new List<StringConstant>();
            this.Nullable = nullable;
            this.IsRowId = isRowId;
            this.DefaultValue = defaultValue;
            this.AutoIncrement = autoIncrement;
        }

        public ColumnDefinition(ColumnDefinition other)
        {
            ColumnName = other.ColumnName.Clone();
            DataType = other.DataType;
            EncryptionFlags = other.EncryptionFlags;
            KeyVersion = other.KeyVersion;
            Length = other.Length;
            EnumValues = other.EnumValues.Select(item => (StringConstant)item.Clone()).ToList();
            Nullable = other.Nullable;
            IsRowId = other.IsRowId;
            DefaultValue = other.DefaultValue;
            AutoIncrement = other.AutoIncrement;
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
                && (Equals(DefaultValue, otherCD.DefaultValue)
                && (AutoIncrement == otherCD.AutoIncrement));
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
                (1 + AutoIncrement.GetHashCode()));
        }
    }
}
