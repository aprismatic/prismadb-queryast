﻿using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST
{
    public interface IDialect
    {
        string IdentifierToString(Identifier q);
        string IdentifierToUnquotedString(Identifier q);
        string ColumnDefinitionToString(ColumnDefinition q);
        string CreateIndexQueryToString(CreateIndexQuery q);
        string CreateTableQueryToString(CreateTableQuery q);
        string AlterTableQueryToString(AlterTableQuery q);
        string AlteredColumnToString(AlteredColumn q);
        string TableRefToString(TableRef q);
        string WhereClauseToString(WhereClause q);
        string UpdateQueryToString(UpdateQuery q);
        string SelectQueryToString(SelectQuery q);
        string DeleteQueryToString(DeleteQuery q);
        string InsertQueryToString(InsertQuery q);
        string IntConstantToString(IntConstant q);
        string BinaryConstantToString(BinaryConstant q);
        string StringConstantToString(StringConstant q);
        string FloatingPointConstantToString(FloatingPointConstant q);
        string AdditionToString(Addition q);
        string MultiplicationToString(Multiplication q);
        string PaillierAdditionToString(PaillierAddition q);
        string ElGamalMultiplicationToString(ElGamalMultiplication q);
        string ColumnRefToString(ColumnRef q);
        string BooleanTrueToString(BooleanTrue q);
        string BooleanInToString(BooleanIn q);
        string BooleanEqualsToString(BooleanEquals q);
        string ConjunctiveNormalFormToString(ConjunctiveNormalForm q);
        string DisjunctionToString(Disjunction q);
        string ScalarFunctionToString(ScalarFunction q);

        string SQLDataTypeToString(SQLDataType q);
        string ColumnEncryptionFlagsToString(ColumnEncryptionFlags q);
        string IndexModifierToString(IndexModifier q);
        string IndexTypeToString(IndexType q);
        string MySQLVariableToString(MySQLVariable q);
    }
}
