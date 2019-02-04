using System;
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
        public char? EscapeChar;

        public BooleanLike()
        {
            setValue(new ColumnRef(""), new StringConstant(), null, false);
        }

        public BooleanLike(ColumnRef column, StringConstant value)
            : this()
        {
            setValue((ColumnRef)column.Clone(), value);
        }

        public BooleanLike(ColumnRef column, StringConstant value, char? escape)
            : this()
        {
            setValue((ColumnRef)column.Clone(), value, escape);
        }

        public BooleanLike(ColumnRef column, StringConstant value, char? escape, bool NOT)
            : this(column, value)
        {
            setValue((ColumnRef)column.Clone(), value, escape, NOT);
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
            else if (value.Length == 3)
            {
                Column = (ColumnRef)value[0];
                SearchValue = (StringConstant)value[1];
                EscapeChar = (char?)value[2];
            }
            else
            {
                Column = (ColumnRef)value[0];
                SearchValue = (StringConstant)value[1];
                EscapeChar = (char?)value[2];
                NOT = (bool)value[3];
            }
        }

        public override object Clone()
        {
            var column_clone = Column.Clone() as ColumnRef;
            var svalue_clone = SearchValue.Clone() as StringConstant;
            var esc_clone = EscapeChar;

            var clone = new BooleanLike(column_clone, svalue_clone, esc_clone, NOT);

            return clone;
        }

        public override object Eval(ResultRow r)
        {
            var svalue_store = SearchValue.strvalue;
            var esc_store = EscapeChar;
            if(EscapeChar == null) esc_store = '\\';
            var col_store = r[Column].ToString();

            EvalState state = EvalState.Start;
            var search_index = 0;
            var column_index = 0;
            var match = true;

            //Check for invalid placement of escape character
            if (esc_store == svalue_store[svalue_store.Length - 1]) return NOT;

            while (state != EvalState.End)
            {
                //If column string length is shorter than search value length
                if (column_index >= col_store.Length) return CheckTrailingPercent(svalue_store.Substring(search_index));

                state = IdentifyState(svalue_store[RestrainSearchIndex(search_index, svalue_store.Length)], esc_store, search_index.Equals(svalue_store.Length));

                switch (state)
                {
                    case EvalState.Last:
                        if (column_index == col_store.Length) return !NOT;
                        state = EvalState.End;
                        break;

                    case EvalState.Character:
                        match = CaseInsensitiveCompare(svalue_store[search_index], col_store[column_index]);
                        search_index++;
                        column_index++;
                        break;

                    case EvalState.Escape:
                        search_index++;
                        match = CaseInsensitiveCompare(svalue_store[search_index], col_store[column_index]);
                        search_index++;
                        column_index++;
                        break;

                    case EvalState.Percent:
                        search_index = search_index + NextNonPercentIndex(svalue_store.Substring(search_index));
                        if (search_index >= svalue_store.Length) return !NOT; //Trailing characters of search value consist only of %
                        int search_chars = NextPercentIndex(svalue_store.Substring(search_index), esc_store);
                        if (search_chars == -1)
                        {
                            if (CompareTrailingCharacters(svalue_store.Substring(search_index), col_store.Substring(column_index), esc_store)) return !NOT;
                            else return NOT;
                        }
                        int matchingindex = FindNextMatch(svalue_store.Substring(search_index, search_chars), col_store.Substring(column_index), esc_store);
                        if (matchingindex == -1) return NOT;  //Substring of search value does not exist in column value
                        search_index = search_index + search_chars;
                        column_index = column_index + matchingindex + 1;
                        break;

                    case EvalState.Underscore:
                        search_index++;
                        column_index++;
                        break;
                }
                if (!match) return NOT;
            }
            return NOT;
        }
 
        private enum EvalState
        {
            Start,
            Character,
            Escape,
            Percent,
            Underscore,
            Last,
            End
        }

        private Boolean CaseInsensitiveCompare(char a, char b)
        {
            return Char.ToUpper(a) == Char.ToUpper(b);
        }

        private int NextPercentIndex(String search, char? escape)
        {
            for (int i = 0; i < search.Length; i++)
            {
                if (search[i] == '%')
                {
                    var escaped = false;
                    if (search[i - 1] == escape) escaped = true;
                    if (!escaped) return i;
                }
            }
            return -1;
        }

        private int RestrainSearchIndex(int search_index, int search_length)
        {
            if (search_index >= search_length) return search_length - 1;
            return search_index;
        }

        private EvalState IdentifyState(char c, char? escape, Boolean lastchar)
        {
            if (lastchar) return EvalState.Last;
            if (c == '%') return EvalState.Percent;
            if (c == '_') return EvalState.Underscore;
            if (escape == c) return EvalState.Escape;
            return EvalState.Character;
        }

        private Boolean CompareTrailingCharacters(String search, String column, char? escape)
        {
            var search_noesc = search.Replace(escape.ToString(), "");

            if (column.Length < search_noesc.Length) return false;

            column = column.Substring(column.Length - search_noesc.Length);

            var search_index = 0;
            var column_index = 0;

            while (search_index < search.Length)
            {
                var esc = false;
                if (search[search_index] == escape)
                {
                    esc = true;
                    search_index++;
                }
                if (esc)
                {
                    if (search[search_index] != column[column_index]) return false;
                }
                else
                {
                    if (!CaseInsensitiveCompare(search[search_index], column[column_index]) && search[search_index] != '_') return false;
                }
                search_index++;
                column_index++;
            }
            return true;
        }

        private int FindNextMatch(String search, String column, char? escape)
        {
            for (int i = 0; i < column.Length; i++)
            {
                var wildcards = 0;
                for (int j = 0; j < search.Length; j++)
                {
                    var notwildcard = true;
                    if (search[j] == escape)
                    {
                        j++;
                        if (j >= search.Length) return -1;
                        notwildcard = false;
                        wildcards++;
                    }
                    if (i + j - wildcards >= column.Length) return -1;
                    if (!CaseInsensitiveCompare(search[j], column[i + j - wildcards]) && (search[j] != '_' && notwildcard || !notwildcard)) j = search.Length;
                    if (j == search.Length - 1) return i + j - wildcards;
                }
            }
            return -1;
        }

        private Boolean CheckTrailingPercent(String search)
        {
            for (int i = 0; i < search.Length; i++)
            {
                if (search[i] != '%') return false;
            }
            return true;
        }

        private int NextNonPercentIndex(String search)
        {
            for (int i = 0; i < search.Length; i++)
            {
                if (search[i] != '%') return i;
            }
            return search.Length;
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
                && (this.SearchValue.Equals(otherBL.SearchValue))
                && (this.EscapeChar.Equals(otherBL.EscapeChar));
        }

        public override int GetHashCode()
        {
            return unchecked(
               (NOT.GetHashCode() + 1) *
               Alias.GetHashCode() *
               Column.GetHashCode() *
               SearchValue.GetHashCode()) *
               EscapeChar.GetHashCode();
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
