using System;

namespace PrismaDB.QueryAST.DDL
{
    public class AlteredColumn : ICloneable
    {
        public ColumnDefinition ColumnDefinition;
        public bool First;

        public AlteredColumn() : this(new ColumnDefinition())
        { }

        public AlteredColumn(ColumnDefinition columnDef, bool first = false)
        {
            this.ColumnDefinition = columnDef;
            this.First = first;
        }

        public AlteredColumn(AlteredColumn other) : this((ColumnDefinition)other.ColumnDefinition.Clone(), other.First)
        { }

        public override string ToString()
        {
            return DialectResolver.Dialect.AlteredColumnToString(this);
        }

        public object Clone()
        {
            return new AlteredColumn(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is AlteredColumn otherAC)) return false;

            return (ColumnDefinition.Equals(otherAC.ColumnDefinition))
                && (First == otherAC.First);
        }

        public override int GetHashCode()
        {
            return unchecked(
                ColumnDefinition.GetHashCode() * First.GetHashCode());
        }
    }
}
