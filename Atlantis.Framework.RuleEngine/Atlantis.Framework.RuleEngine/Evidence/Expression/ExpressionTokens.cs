using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.RuleEngine.Evidence
{
    public static class ExpressionTokens
    {
        // Operators
        public static string OperatorAddition = "+";
        public static string OperatorSubtraction = "-";
        public static string OperatorDivision = "/";
        public static string OperatorModulus = "%";
        public static string OperatorMultiplication = "*";
        public static string OperatorExclusiveOr = "^";
        public static string OperatorEquals = "==";
        public static string OperatorNEquals = "!=";
        public static string OperatorGreaterThanEqual = ">=";
        public static string OperatorLessThanEqual = "<=";
        public static string OperatorGreaterThan = ">";
        public static string OperatorLessThan = "<";
        public static string OperatorAnd = "AND";
        public static string OperatorOr = "OR";
        public static string OperatorNot = "NOT";
        public static string OperatorXor = "XOR";
        private static HashSet<string> expressionOperators = new HashSet<string>()
            {
                OperatorAddition,
                OperatorSubtraction,
                OperatorDivision,
                OperatorMultiplication,
                OperatorExclusiveOr,
                OperatorEquals,
                OperatorNEquals,
                OperatorGreaterThanEqual,
                OperatorLessThanEqual,
                OperatorGreaterThan,
                OperatorLessThan,
                OperatorAnd,
                OperatorOr,
                OperatorNot,
                OperatorXor
            };
        public static bool IsExpressionOperator(string token)
        {
            return expressionOperators.Contains(token);
        }

        // Functions
        public static string FunctionRegEx = "@REGEX";
        public static string FunctionMinLength = "@MINLENGTH";
        public static string FunctionMaxLength = "@MAXLENGTH";
        public static string FunctionSubString = "@SUBSTRING";
        public static string FunctionIsValidDate = "ISVALIDDATE";
        public static string FunctionIsNullOrEmpty = "ISNULLOREMPTY";
        private static HashSet<string> expressionFunctions = new HashSet<string>()
            {
                FunctionRegEx,
                FunctionMinLength,
                FunctionMaxLength,
                FunctionSubString,
                FunctionIsValidDate,
                FunctionIsNullOrEmpty
            };
        public static bool IsExpressionFunction(string token)
        {
            return expressionFunctions.Contains(token);
        }

        // Evidence collector
        public static string FunctionFact = "FACT";

        // Delimiters
        public static string DelimiterOpenParanthesis = "(";
        public static string DelimiterCloseParanthesis = ")";
        private static HashSet<string> expressionDelimiters = new HashSet<string>()
            {
                DelimiterOpenParanthesis,
                DelimiterCloseParanthesis
            };
        public static bool IsExpressionDelimiter(string token)
        {
            return expressionDelimiters.Contains(token);
        }

        public static bool IsExpressionToken(string token)
        {
            return (IsExpressionOperator(token) || IsExpressionFunction(token) || IsExpressionDelimiter(token) || FunctionFact.Equals(token));
        }
    }
}
