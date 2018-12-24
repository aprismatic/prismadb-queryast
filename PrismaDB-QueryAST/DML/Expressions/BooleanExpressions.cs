﻿using System;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.Result;

namespace PrismaDB.QueryAST.DML
{
    public abstract class BooleanExpression : Expression
    {
        public bool NOT = false;
    }

    public class BooleanTrue : BooleanExpression
    {
        public BooleanTrue() { setValue(false); }
        public BooleanTrue(bool NOT) { setValue(NOT); }
        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanTrue.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else
                throw new ArgumentException("BooleanTrue.setValue expects no more than one argument");
        }
        public override object Clone() { return new BooleanTrue(NOT); }
        public override object Eval(ResultRow r) { return !NOT; }
        public override List<ColumnRef> GetColumns() { return new List<ColumnRef>(); }
        public override List<ColumnRef> GetNoCopyColumns() { return new List<ColumnRef>(); }
        public override string ToString() { return DialectResolver.Dialect.BooleanTrueToString(this); }
        public override bool Equals(object other) { return (Alias.Equals((other as BooleanTrue)?.Alias))
                                                        && (NOT == (other as BooleanTrue)?.NOT); }
        public override int GetHashCode() { return unchecked(Alias.GetHashCode() * (NOT.GetHashCode() + 1)); }
    }

    public class BooleanLike : BooleanExpression
    {
        public ColumnRef Column;
        public StringConstant SearchValue;

        public BooleanLike()
        {
            setValue(new ColumnRef(""), new StringConstant(), false);
        }

        public BooleanLike(ColumnRef column, StringConstant value)
            : this()
        {
            setValue((ColumnRef)column.Clone(), value);
        }

        public BooleanLike(ColumnRef column, StringConstant value, bool NOT)
            : this(column, value)
        {
            setValue((ColumnRef)column.Clone(), value, NOT);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanLike.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else if (value.Length == 2)
            {
                Column = (ColumnRef)value[0];
                SearchValue = (StringConstant)value[1];
            }
            else
            {
                Column = (ColumnRef)value[0];
                SearchValue = (StringConstant)value[1];
                NOT = (bool)value[2];
            }
        }

        public override object Clone()
        {
            var column_clone = Column.Clone() as ColumnRef;
            var svalue_clone = SearchValue.Clone() as StringConstant;

            var clone = new BooleanLike(column_clone, svalue_clone, NOT);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            var svalue_store = SearchValue.strvalue;
            var col_store = r[Column].ToString();

            //Check length of search value
            if (svalue_store.Replace("%", "").Length > col_store.Length) return NOT;   

            var firstloop = true;
            for (int PercentIndex = svalue_store.IndexOf('%'); PercentIndex != -1; PercentIndex = svalue_store.IndexOf('%'))
            {
                var svalue_section = svalue_store.Substring(0, PercentIndex);
                int containsindex = -1;

                //Match leading characters
                if (firstloop)  
                {
                    if (!EqualsUnderscore(col_store.Substring(0, PercentIndex), svalue_section)) return NOT;
                    col_store = col_store.Substring(svalue_section.Length);
                    firstloop = false;
                }
                //Match intermediate characters
                else
                {
                    containsindex = ContainsUnderscore(col_store, svalue_section);
                    if (containsindex == -1) return NOT;
                    col_store = col_store.Substring(containsindex + svalue_section.Length);
                }

                svalue_store = svalue_store.Substring(PercentIndex + 1);
            }

            //Match trailing characters
            if (!EqualsUnderscore(col_store.Substring(col_store.Length - svalue_store.Length), svalue_store)) return NOT;  

            return !NOT;
        }

        private static Boolean EqualsUnderscore(String str, String search)
        {
            for (int i = 0; i < search.Length; i++)
            {
                if (!(search[i] == str[i]) && search[i] != '_') return false;
            }
            return true;
        }

        private static int ContainsUnderscore(String str, String search)
        {
            for (int i = 0; i < str.Length - search.Length + 1; i++)
            {
                if (EqualsUnderscore(str.Substring(i, search.Length), search)) return i;
            }
            return -1;
        }

        public override List<ColumnRef> GetColumns()
        {
            return Column.GetColumns();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return Column.GetNoCopyColumns();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanLikeToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanLike otherBL)) return false;

            return (this.NOT != otherBL.NOT)
                && (this.Alias.Equals(otherBL.Alias))
                && (this.Column.Equals(otherBL.Column))
                && (this.SearchValue.Equals(otherBL.SearchValue));
        }

        public override int GetHashCode()
        {
            return unchecked(
               (NOT.GetHashCode() + 1) *
               Alias.GetHashCode() *
               Column.GetHashCode() *
               SearchValue.GetHashCode());
        }
    }

    public class BooleanIn : BooleanExpression
    {
        public ColumnRef Column;
        public List<Constant> InValues;

        public BooleanIn()
        {
            Alias = new Identifier("");
            Column = new ColumnRef("");
            InValues = new List<Constant>();
            NOT = false;
        }

        public BooleanIn(ColumnRef column, params Constant[] values)
            : this()
        {
            Column = (ColumnRef)column.Clone();
            InValues.AddRange(values);
        }

        public BooleanIn(bool NOT, ColumnRef column, params Constant[] values)
            : this(column, values)
        {
            this.NOT = NOT;
        }

        public override void setValue(params object[] value)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            var res = new BooleanIn
            {
                Column = Column,
                NOT = NOT
            };
            foreach (var v in InValues)
                res.InValues.Add((Constant)v.Clone());
            return res;
        }

        public override object Eval(ResultRow r)  // TODO: Check for correctness
        {
            return InValues.Select(x => x.ToString()).Contains(r[Column].ToString()) ? !NOT : NOT;
        }

        public override List<ColumnRef> GetColumns()
        {
            return Column.GetColumns();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return Column.GetNoCopyColumns();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanInToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanIn otherBI)) return false;

            return (this.NOT != otherBI.NOT)
                && (this.Alias.Equals(otherBI.Alias))
                && (this.Column.Equals(otherBI.Column))
                && (this.InValues.All(x => otherBI.InValues.Exists(y => x.Equals(y))));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                Alias.GetHashCode() *
                Column.GetHashCode() *
                InValues.Aggregate(1, (x, y) => unchecked(x * y.GetHashCode())));
        }
    }

    public class BooleanEquals : BooleanExpression
    {
        public Expression left, right;

        public BooleanEquals(Expression left, Expression right)
        {
            setValue(left, right);
        }

        public BooleanEquals(Expression left, Expression right, bool NOT)
        {
            setValue(left, right, NOT);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanEquals.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else if (value.Length == 2)
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
            }
            else
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
                NOT = (bool)value[2];
            }
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new BooleanEquals(left_clone, right_clone, NOT);

            return clone;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanEqualsToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanEquals otherBE)) return false;

            return (this.NOT != otherBE.NOT)
                && (this.Alias.Equals(otherBE.Alias))
                && (this.left.Equals(otherBE.left))
                && (this.right.Equals(otherBE.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }

        public override object Eval(ResultRow r)
        {
            var leftEval = left.Eval(r);
            var rightEval = right.Eval(r);

            if (leftEval is String && rightEval is String)
            {
                return ((String)leftEval == (String)rightEval) ? !NOT : NOT;
            }

            // Assume data in DataRow are numeric
            return (Convert.ToDecimal(leftEval) == Convert.ToDecimal(rightEval)) ? !NOT : NOT;

            throw new ApplicationException(
                 "Left and right expressions of BooleanEquals are not of the same type.\n" +
                $"Left expression is \"{left}\" of type {left.GetType()}\n" +
                $"Left expression evaluates to \"{leftEval}\" of type {leftEval.GetType()}\n" +
                $"Right expression is \"{right}\" of type {right.GetType()}\n" +
                $"Right expression evaluates to \"{rightEval}\" of type {rightEval.GetType()}");
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }
    }

    public class BooleanGreaterThan : BooleanExpression
    {
        public Expression left, right;

        public BooleanGreaterThan(Expression left, Expression right)
        {
            setValue(left, right);
        }

        public BooleanGreaterThan(Expression left, Expression right, bool NOT)
        {
            setValue(left, right, NOT);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanGreaterThan.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else if (value.Length == 2)
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
            }
            else
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
                NOT = (bool)value[2];
            }
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new BooleanGreaterThan(left_clone, right_clone, NOT);

            return clone;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanGreaterThanToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanGreaterThan otherBG)) return false;

            return (this.NOT != otherBG.NOT)
                && (this.Alias.Equals(otherBG.Alias))
                && (this.left.Equals(otherBG.left))
                && (this.right.Equals(otherBG.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }

        public override object Eval(ResultRow r)
        {
            var leftEval = left.Eval(r);
            var rightEval = right.Eval(r);

            // Assume data in DataRow are numeric
            return (Convert.ToDecimal(leftEval) > Convert.ToDecimal(rightEval)) ? !NOT : NOT;

            throw new ApplicationException(
                 "Left and right expressions of BooleanGreaterThan are not of the same type.\n" +
                $"Left expression is \"{left}\" of type {left.GetType()}\n" +
                $"Left expression evaluates to \"{leftEval}\" of type {leftEval.GetType()}\n" +
                $"Right expression is \"{right}\" of type {right.GetType()}\n" +
                $"Right expression evaluates to \"{rightEval}\" of type {rightEval.GetType()}");
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }
    }

    public class BooleanLessThan : BooleanExpression
    {
        public Expression left, right;

        public BooleanLessThan(Expression left, Expression right)
        {
            setValue(left, right);
        }

        public BooleanLessThan(Expression left, Expression right, bool NOT)
        {
            setValue(left, right, NOT);
        }

        public override void setValue(params object[] value)
        {
            if (value.Length == 0)
                throw new ArgumentException("BooleanLessThan.setValue expects at least one argument");
            else if (value.Length == 1)
                NOT = (bool)value[0];
            else if (value.Length == 2)
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
            }
            else
            {
                left = (Expression)value[0];
                right = (Expression)value[1];
                NOT = (bool)value[2];
            }
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var right_clone = right.Clone() as Expression;

            var clone = new BooleanLessThan(left_clone, right_clone, NOT);

            return clone;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanLessThanToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanLessThan otherBL)) return false;

            return (this.NOT != otherBL.NOT)
                && (this.Alias.Equals(otherBL.Alias))
                && (this.left.Equals(otherBL.left))
                && (this.right.Equals(otherBL.right));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                Alias.GetHashCode() *
                left.GetHashCode() *
                right.GetHashCode());
        }

        public override object Eval(ResultRow r)
        {
            var leftEval = left.Eval(r);
            var rightEval = right.Eval(r);

            // Assume data in DataRow are numeric
            return (Convert.ToDecimal(leftEval) < Convert.ToDecimal(rightEval)) ? !NOT : NOT;

            throw new ApplicationException(
                 "Left and right expressions of BooleanLessThan are not of the same type.\n" +
                $"Left expression is \"{left}\" of type {left.GetType()}\n" +
                $"Left expression evaluates to \"{leftEval}\" of type {leftEval.GetType()}\n" +
                $"Right expression is \"{right}\" of type {right.GetType()}\n" +
                $"Right expression evaluates to \"{rightEval}\" of type {rightEval.GetType()}");
        }

        public override List<ColumnRef> GetColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetColumns());
            res.AddRange(right.GetColumns());
            return res;
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            var res = new List<ColumnRef>();
            res.AddRange(left.GetNoCopyColumns());
            res.AddRange(right.GetNoCopyColumns());
            return res;
        }
    }

    public class BooleanIsNull : BooleanExpression
    {
        public Expression left;

        public BooleanIsNull(Expression left, bool not = false)
        {
            setValue(not, left);
        }

        public BooleanIsNull(Expression left, bool not, Identifier columnname)
        {
            setValue(not, left, columnname);
        }

        public BooleanIsNull(Expression left, Identifier columnname)
        {
            setValue(false, left, columnname);
        }

        public override void setValue(params object[] value)
        {
            switch (value.Length)
            {
                case 1:
                    NOT = false;
                    left = (Expression) value[1];
                    break;
                case 2:
                    NOT = (bool) value[0];
                    left = (Expression) value[1];
                    break;
                case 3:
                    NOT = (bool) value[0];
                    left = (Expression) value[1];
                    Alias = (Identifier) value[2];
                    break;
                default:
                    throw new ArgumentException("BooleanIsNull.setValue expects one to three arguments");
            }
        }

        public override object Clone()
        {
            var left_clone = left.Clone() as Expression;
            var colid = Alias?.Clone();
            return new BooleanIsNull(left_clone, NOT, colid);
        }

        public override object Eval(ResultRow r)
        {
            var leftres = left.Eval(r);
            return NOT ? !(leftres is DBNull) : leftres is DBNull;
        }

        public override List<ColumnRef> GetColumns()
        {
            return left.GetColumns();
        }

        public override List<ColumnRef> GetNoCopyColumns()
        {
            return left.GetNoCopyColumns();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.BooleanIsNullToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is BooleanIsNull otherBIN)) return false;

            return (NOT != otherBIN.NOT)
                && (left.Equals(otherBIN.left))
                && (Alias.Equals(otherBIN.Alias));
        }

        public override int GetHashCode()
        {
            return unchecked(
                (NOT.GetHashCode() + 1) *
                left.GetHashCode() *
                Alias.GetHashCode());
        }
    }
}
