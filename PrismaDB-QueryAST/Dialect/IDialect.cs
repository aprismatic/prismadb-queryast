using PrismaDB.QueryAST.DCL;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST
{
    public interface IDialect
    {
        string IdentifierToString(Identifier q);
        string ColumnDefinitionToString(ColumnDefinition q);
        string CreateIndexQueryToString(CreateIndexQuery q);
        string CreateTableQueryToString(CreateTableQuery q);
        string AlterTableQueryToString(AlterTableQuery q);
        string AlteredColumnToString(AlteredColumn q);
        string TableRefToString(TableRef q);
        string DatabaseRefToString(DatabaseRef q);
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
        string ColumnRefToString(ColumnRef q);
        string BooleanTrueToString(BooleanTrue q);
        string BooleanInToString(BooleanIn q);
        string BooleanIsToString(BooleanIs q);
        string BooleanEqualsToString(BooleanEquals q);
        string BooleanGreaterThanToString(BooleanGreaterThan q);
        string BooleanLessThanToString(BooleanLessThan q);
        string ConjunctiveNormalFormToString(ConjunctiveNormalForm q);
        string DisjunctionToString(Disjunction q);
        string ScalarFunctionToString(ScalarFunction q);
        string MySqlVariableToString(MySqlVariable q);
        string OrderByClauseToString(OrderByClause q);
        string GroupByClauseToString(GroupByClause q);
        string JoinClauseToString(JoinClause q);
        string AllColumnsToString(AllColumns q);
        string NullConstantToString(NullConstant q);
        string BooleanIsNullToString(BooleanIsNull q);
        string PaillierAdditionFunctionToString(PaillierAdditionFunction q);
        string ElGamalMultiplicationFunctionToString(ElGamalMultiplicationFunction q);
        string SumAggregationFunctionToString(SumAggregationFunction q);
        string CountAggregationFunctionToString(CountAggregationFunction q);
        string AvgAggregationFunctionToString(AvgAggregationFunction q);
        string PaillierAggregationSumFunctionToString(PaillierAggregationSumFunction q);
        string UseStatementToString(UseStatement q);

        string ExportSettingsCommandToString(ExportSettingsCommand c);

        string SqlDataTypeToString(SqlDataType q);
        string ColumnEncryptionFlagsToString(ColumnEncryptionFlags q);
        string IndexModifierToString(IndexModifier q);
        string IndexTypeToString(IndexType q);
        string OrderDirectionToString(OrderDirection q);
        string JoinTypeToString(JoinType q);
    }
}
