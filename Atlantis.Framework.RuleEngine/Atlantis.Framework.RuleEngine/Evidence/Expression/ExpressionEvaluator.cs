/*
Simple Rule Engine
Copyright (C) 2005 by Sierra Digital Solutions Corp

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Atlantis.Framework.RuleEngine.Evidence.Expression;

namespace Atlantis.Framework.RuleEngine.Evidence
{
    /// <summary>
    /// Summary description for ExpressionEvaluator2.
    /// </summary>
    public class ExpressionEvaluator
    {
        public event EvidenceLookupHandler GetEvidence;

        #region RelatedEvidence

        public static string[] RelatedEvidence(List<Symbol> symbols)
        {
            var al = new ArrayList();

            foreach (Symbol symbol in symbols)
            {
                if (symbol.SymbolType != Type.Fact)
                    continue;

                al.Add(symbol.Name);
            }

            return (string[])al.ToArray(typeof(string));
        }

        #endregion

        #region Instance varaibles

        protected double Result = 0;
        protected List<Symbol> InfixSymbol = new List<Symbol>();
        protected List<Symbol> PostfixSymbol = new List<Symbol>();

        public enum Type
        {
            Fact,
            Value,
            Operator,
            Function,
            Result,
            OpenBracket,
            CloseBracket,
            Invalid //states the comparison could not be made and is invalid
        }

        #endregion

        #region Constructor

        #endregion

        public List<Symbol> Infix
        {
            get
            {
                return new List<Symbol>(InfixSymbol);
            }
            set
            {
                InfixSymbol = value;
            }
        }
        public List<Symbol> Postfix
        {
            get
            {
                return new List<Symbol>(PostfixSymbol);
            }
            set
            {
                PostfixSymbol = value;
            }
        }

        #region Parser

        public void Parse(string eq)
        {
            Debug.Write("Parsing to Infix: " + eq + " : ");

            //reset 
            InfixSymbol.Clear();
            PostfixSymbol.Clear();

            //tokinize string
            var rawTokins = ExpressionParser.Tokenize(eq);
            for (var x = 0; x < rawTokins.Length; x++)
            {
                var currentTokin = rawTokins[x].Trim();
                if (currentTokin != String.Empty)
                {
                    var current = ParseToSymbol(currentTokin);

                    //add the current to the collection
                    InfixSymbol.Add(current);
                    Debug.Write(current.Name + "|");
                }
            }

            Debug.WriteLine("");
        }

        private static Symbol ParseToSymbol(string unknownSymbol)
        {
            var sym = new Symbol();
            if (IsOpenParanthesis(unknownSymbol))
            {
                sym.Name = unknownSymbol;
                sym.SymbolType = Type.OpenBracket;
            }
            else if (IsCloseParanthesis(unknownSymbol))
            {
                sym.Name = unknownSymbol;
                sym.SymbolType = Type.CloseBracket;
            }
            else if (IsFunction(unknownSymbol)) //isfunction must come b4 isvariable because its an exact match where the other isnt
            {
                sym.Name = unknownSymbol;
                sym.SymbolType = Type.Function;
            }
            else if (IsOperator(unknownSymbol))
            {
                sym.Name = unknownSymbol;
                sym.SymbolType = Type.Operator;
            }
            else if (IsBoolean(unknownSymbol))
            {
                var naked = new Naked(Boolean.Parse(unknownSymbol), typeof(bool));
                sym.Name = unknownSymbol;
                sym.Value = naked;
                sym.SymbolType = Type.Value;
            }
            else if (IsFact(unknownSymbol))
            {
                sym.Name = unknownSymbol;
                sym.SymbolType = Type.Fact;
            }
            else if (IsNumber(unknownSymbol))
            {
                var naked = new Naked(Double.Parse(unknownSymbol), typeof(double));
                sym.Name = unknownSymbol;
                sym.Value = naked;
                sym.SymbolType = Type.Value;
            }
            else if (IsString(unknownSymbol))
            {
                var naked = new Naked(unknownSymbol.Substring(1, unknownSymbol.Length - 2), typeof(string));
                sym.Name = unknownSymbol;
                sym.Value = naked;
                sym.SymbolType = Type.Value;
            }
            else
            {
                //who knows what it is so throw an exception
                throw new Exception("Invalid token: " + unknownSymbol);
            }

            return sym;
        }

        private static bool IsFact(string value)
        {
            var isFact = false;
            //variables must have the first digit as a letter and the remaining as numbers and letters
            if (!string.IsNullOrEmpty(value) && Char.IsLetter(value[0]))
            {
                isFact = true;
                foreach (var character in value)
                {
                    if (!Char.IsLetter(character) && !Char.IsNumber(character))
                    {
                        isFact = false;
                        break;
                    }
                }
            }

            return isFact;
        }

        private static bool IsBoolean(string value)
        {
            bool result;
            var isBool = bool.TryParse(value, out result);

            return isBool;
        }

        private static bool IsNumber(string value)
        {
            int result;
            bool isNumeric = int.TryParse(value, out result);

            return isNumeric;
        }

        private static bool IsString(string value)
        {
            var result = false;

            if (!string.IsNullOrEmpty(value))
            {
                result = value.StartsWith(@"""") && value.EndsWith(@"""");
            }

            return result;
        }

        private static bool IsOpenParanthesis(string value)
        {
            var result = value == "(";
            return result;
        }

        private static bool IsCloseParanthesis(string value)
        {
        var result = value == ")";
        return result;
        }

        private static bool IsOperator(string value)
        {
            var result = false;

            if (!string.IsNullOrEmpty(value))
            {
                result = ExpressionTokens.IsExpressionOperator(value);
            }

            return result;
        }

        private static bool IsFunction(string value)
        {
            var result = false;

            if (!string.IsNullOrEmpty(value))
            {
                result = ExpressionTokens.IsExpressionFunction(value);
            }

            return result;
        }

        #endregion

        #region Infix to Postfix

        public void InfixToPostfix()
        {
            Debug.Write("Parsing Infix to PostFix: ");

            PostfixSymbol.Clear();

            var postfixStack = new Stack();
            foreach (var infix in InfixSymbol)
            {
                switch (infix.SymbolType)
                {
                    case Type.Fact:
                    case Type.Value:
                        Debug.Write(infix.Name + "|");
                        PostfixSymbol.Add(infix);
                        break;
                    case Type.Function:
                    case Type.Operator:
                        while (postfixStack.Count > 0 && !DeterminePrecedence(infix, (Symbol)postfixStack.Peek()))
                        {
                            Debug.Write(((Symbol)postfixStack.Peek()).Name + "|");
                            PostfixSymbol.Add((Symbol)postfixStack.Pop());
                        }
                        postfixStack.Push(infix);
                        break;
                    case Type.OpenBracket:
                        postfixStack.Push(infix);
                        break;
                    case Type.CloseBracket:
                        while (((Symbol)postfixStack.Peek()).SymbolType != Type.OpenBracket)
                        {
                            Debug.Write(((Symbol)postfixStack.Peek()).Name + "|");
                            PostfixSymbol.Add((Symbol)postfixStack.Pop());
                        }
                        postfixStack.Pop(); //discard '('
                        break;
                    default:
                        throw new Exception("Illegal symbol: " + infix.Name);
                }
            }

            //now we pop off whats left on the stack
            while (postfixStack.Count > 0)
            {
                Debug.Write(((Symbol)postfixStack.Peek()).Name + "|");
                PostfixSymbol.Add((Symbol)postfixStack.Pop());
            }

            Debug.WriteLine("");
        }

        private static bool DeterminePrecedence(Symbol higher, Symbol lower)
        {
            var s1 = Precedence(higher);
            var s2 = Precedence(lower);

            var result = s1 > s2;
            return result;
        }

        private static int Precedence(Symbol symbol)
        {
            var result = 0;

            if (symbol.Name.Equals(ExpressionTokens.OperatorMultiplication) ||
                symbol.Name.Equals(ExpressionTokens.OperatorDivision) ||
                symbol.Name.Equals(ExpressionTokens.OperatorModulus))
            {
                result = 32;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorAddition) ||
                symbol.Name.Equals(ExpressionTokens.OperatorSubtraction))
            {
                result = 16;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorGreaterThan) ||
                symbol.Name.Equals(ExpressionTokens.OperatorLessThan) ||
                symbol.Name.Equals(ExpressionTokens.OperatorGreaterThanEqual) ||
                symbol.Name.Equals(ExpressionTokens.OperatorLessThanEqual))
            {
                result = 8;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorEquals) ||
                symbol.Name.Equals(ExpressionTokens.OperatorNEquals))
            {
                result = 4;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorNot))
            {
                result = 3;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorAnd))
            {
                result = 2;
            }
            else if (symbol.Name.Equals(ExpressionTokens.OperatorXor) ||
                symbol.Name.Equals(ExpressionTokens.OperatorOr))
            {
                result = 1;
            }

            //functions have the highest priority
            if (symbol.SymbolType == Type.Function)
            {
                result = 64;
            }

            return result;
        }

        #endregion

        public Symbol Evaluate()
        {
            var operandStack = new Stack();
            var factSymbolKey = string.Empty;

            foreach (var postFix in PostfixSymbol)
            {
                switch (postFix.SymbolType)
                {
                    case Type.Value:
                        operandStack.Push(postFix);
                        break;
                    case Type.Operator:
                    {
                        var operandResult = EvaluateOperand(postFix, operandStack);
                        operandStack.Push(operandResult);
                    }
                    break;
                    case Type.Function:
                    {
                        var operandResult = EvaluateFunction(postFix, operandStack);
                        operandStack.Push(operandResult);
                    }
                    break;
                    case Type.Fact:
                    {
                        if (postFix.Name == "FACT")
                        {
                            continue;
                        }

                        var op3 = new Symbol { SymbolType = Type.Value };

                        var fact = GetEvidence(this, new EvidenceLookupArgs(postFix.Name));

                        factSymbolKey = fact.ValueObject.EvidenceValueKey;

                        op3.Value = new Naked(fact.Value, fact.ValueType);
                        operandStack.Push(op3);
                        Debug.WriteLine("ExpressionEvaluator FACT {0} = {1}", fact.Id, fact.Value);
                        continue;
                    }
                    default:
                        throw new Exception(String.Format("Invalid symbol type: {0} of type {1}", postFix.Name, postFix.SymbolType));
                }
            }
      
            var returnValue = (Symbol)operandStack.Pop();
            returnValue.ConditionKey = factSymbolKey;

            if (operandStack.Count > 0)
            {
                throw new Exception("Invalid equation? OperandStack should have a count of zero.");
            }

            return returnValue;
        }

        private Symbol EvaluateOperand(Symbol postFix, Stack operandStack)
        {
            Symbol op1;
            Symbol op2;
            Symbol operandResult;

            if (postFix.Name.Equals(ExpressionTokens.OperatorAddition))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateAddition(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorSubtraction))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateSubtraction(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorMultiplication))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateMultiplication(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorDivision))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateDivision(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorModulus))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateModulus(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorEquals))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateEquals(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorNEquals))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateNEquals(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorGreaterThan))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateGreaterThan(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorLessThan))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateLessThan(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorGreaterThanEqual))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateGreaterThanEqual(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorLessThanEqual))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateLessThanEqual(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorAnd))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateAnd(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorOr))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateOr(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.OperatorNot))
            {
                op1 = (Symbol)operandStack.Pop(); //this operation requires one parameters
                operandResult = EvaluateNot(op1);
            }
            else
            {
                throw new Exception(String.Format("Invalid operator: {0} of type {1}", postFix.Name, postFix.SymbolType));
            }

            return operandResult;
        }

        private Symbol EvaluateFunction(Symbol postFix, Stack operandStack)
        {
            Symbol op1;
            Symbol op2;
            Symbol operandResult;

            if (postFix.Name.Equals(ExpressionTokens.FunctionRegEx))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateRegex(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.FunctionMinLength))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateMinLength(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.FunctionMaxLength))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateMaxLength(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.FunctionSubString))
            {
                op2 = (Symbol)operandStack.Pop(); //this operation requires two parameters
                op1 = (Symbol)operandStack.Pop();
                operandResult = EvaluateSubstring(op1, op2);
            }
            else if (postFix.Name.Equals(ExpressionTokens.FunctionIsNullOrEmpty))
            {
                op1 = (Symbol)operandStack.Pop(); //this operation requires one parameters
                operandResult = EvaluateIsNullOrEmpty(op1);
            }
            else if (postFix.Name.Equals(ExpressionTokens.FunctionIsValidDate))
            {
                op1 = (Symbol)operandStack.Pop(); //this operation requires one parameters
                operandResult = EvaluateIsValidDate(op1);
            }
            else
            {
                throw new Exception(String.Format("Invalid function: {0} of type {1}", postFix.Name, postFix.SymbolType));
            }

            return operandResult;
        }

        #region Evaluates

        private static Symbol EvaluateAddition(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                if (o1 is string || o2 is string)
                {
                    replacement = o1 + Convert.ToString(o2);
                    success = true;
                }
                else if (o1 is double && o2 is double)
                {
                    replacement = (double)o1 + (double)o2;
                    success = true;
                }
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  if (o1 is string || o2 is string)
            //  {
            //    replacement = o1 + o2.ToString();
            //  }
            //  else if (o1 is double && o2 is double)
            //  {
            //    replacement = (double)o1 + (double)o2;
            //  }
            //  else
            //  {
            //    throw new Exception("Nothing to Add.");
            //  }
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} + {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateSubtraction(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                if (o1 is string || o2 is string)
                {
                    throw new Exception("Cannot subtract strings.");
                }

                if (o1 is double && o2 is double)
                {
                    replacement = (double)o1 - (double)o2;
                    success = true;
                }
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  if (o1 is string || o2 is string)
            //  {
            //    throw new Exception("Cannot subtract strings.");
            //  }

            //  if (o1 is double && o2 is double)
            //  {
            //    replacement = (double)o1 - (double)o2;
            //  }
            //  else
            //  {
            //    throw new Exception("Nothing to Subract.");
            //  }
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} - {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateMultiplication(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                if (o1 is string || o2 is string)
                {
                    throw new Exception("Cannot multiply strings.");
                }

                if (o1 is double && o2 is double)
                {
                    replacement = (double)o1 * (double)o2;
                    success = true;
                }
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }


            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  if (o1 is string || o2 is string)
            //  {
            //    throw new Exception("Cannot multiply strings.");
            //  }

            //  if (o1 is double && o2 is double)
            //  {
            //    replacement = (double)o1 * (double)o2;
            //  }
            //  else
            //  {
            //    throw new Exception("Nothing to Multiply.");
            //  }
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} * {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateDivision(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                if (o1 is string || o2 is string)
                {
                    throw new Exception("Cannot divide strings.");
                }

                if (o1 is double && o2 is double)
                {
                    replacement = (double)o1 / (double)o2;
                    success = true;
                }
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  if (o1 is string || o2 is string)
            //  {
            //    throw new Exception("Cannot divide strings.");
            //  }
            //  if (o1 is double && o2 is double)
            //  {
            //    replacement = (double)o1 / (double)o2;
            //  }
            //  else
            //  {
            //    throw new Exception("Nothing to Divide.");
            //  }
            //}
            //catch
            //{
            //  replacement = false;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} / {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateModulus(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                if (o1 is string || o2 is string)
                {
                    throw new Exception("Cannot modulo strings.");
                }

                if (o1 is double && o2 is double)
                {
                    replacement = (double)o1 % (double)o2;
                    success = true;
                }
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  if (o1 is string || o2 is string)
            //  {
            //    throw new Exception("Cannot divide strings.");
            //  }
            //  if (o1 is double && o2 is double)
            //  {
            //    replacement = (double)o1 / (double)o2;
            //  }
            //  else
            //  {
            //    throw new Exception("Nothing to Divide.");
            //  }
            //}
            //catch
            //{
            //  replacement = false;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} % {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateEquals(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
                o1 = op1.Value.Value;
                o2 = op2.Value.Value;

                bool result = o1.Equals(o2);
                replacement = result;
                success = true;
            }

            if (!success)
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  bool result = o1.Equals(o2);
            //  replacement = result;
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} == {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateNEquals(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            object o1 = null;
            object o2 = null;

            bool success = false;
            object replacement = op3;

            if (op1.Value != null && op2.Value != null)
            {
            o1 = op1.Value.Value;
            o2 = op2.Value.Value;

            bool result = !(o1.Equals(o2));
            replacement = result;
            success = true;
            }

            if (!success)
            {
            op3.SymbolType = Type.Invalid;
            op3.Value = null;
            replacement = op3;
            }

            //object replacement;
            //try
            //{
            //  o1 = op1.Value.Value;
            //  o2 = op2.Value.Value;

            //  var result = !(o1.Equals(o2));
            //  replacement = result;
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} != {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateAnd(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            bool b1 = false;
            bool b2 = false;

            bool success = false;
            if (op1.Value != null && op2.Value != null)
            {
                success = bool.TryParse(Convert.ToString(op1.Value.Value), out b1) && bool.TryParse(Convert.ToString(op2.Value.Value), out b2);
            }

            object replacement;
            if (success)
            {
                replacement = (b1 && b2);
            }
            else
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //see if the facts are not equal
            //object replacement;
            //try
            //{
            //  b1 = (bool)op1.Value.Value;
            //  b2 = (bool)op2.Value.Value;

            //  replacement = (b1 && b2);
            //}
            //catch
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}

            Debug.WriteLine("ExpressionEvaluator {0} AND {1} = {2}", b1, b2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateNot(Symbol op1)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            bool b1 = false;

            bool success = false;
            if (op1.Value != null)
            {
                success = bool.TryParse(Convert.ToString(op1.Value.Value), out b1);
            }

            object replacement;
            if (success)
            {
                replacement = (!b1);
            }
            else
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            //see if the facts are not equal
            //object replacement;
            //try
            //{
            //  b1 = (bool)op1.Value.Value;

            //  replacement = (!b1);
            //}
            //catch //FUTURE: only catch specific errors and throw others up the stack
            //{
            //  op3.SymbolType = Type.Invalid;
            //  op3.Value = null;
            //  replacement = op3;
            //}
            Debug.WriteLine("ExpressionEvaluator NOT {0} = {1}", b1, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateOr(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            bool b1 = false;
            bool b2 = false;

            bool success = false;
            if (op1.Value != null && op2.Value != null)
            {
                success = bool.TryParse(Convert.ToString(op1.Value.Value), out b1) && bool.TryParse(Convert.ToString(op2.Value.Value), out b2);
            }

            object replacement;
            if (success)
            {
                replacement = b1 || b2;
            }
            else
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator {0} OR {1} = {2}", b1, b2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateGreaterThan(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            IComparable o1 = null;
            IComparable o2 = null;

            //see if the facts are not equal
            object replacement;
            try
            {
                o1 = (IComparable)op1.Value.Value;
                o2 = (IComparable)op2.Value.Value;

                var result = o1.CompareTo(o2);

                if (result == 1)
                {
                    replacement = true;
                }
                else
                {
                    replacement = false;
                }
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }
            Debug.WriteLine("ExpressionEvaluator {0} > {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateLessThan(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            IComparable o1 = null;
            IComparable o2 = null;

            //see if the facts are not equal
            object replacement;
            try
            {
                o1 = (IComparable)op1.Value.Value;
                o2 = (IComparable)op2.Value.Value;

                replacement = o1.CompareTo(o2) == -1;

                //var result = o1.CompareTo(o2);
                //if (result == -1)
                //{
                //  replacement = true;
                //}
                //else
                //{
                //  replacement = false;
                //}
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }
            Debug.WriteLine("ExpressionExaluator {0} < {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateGreaterThanEqual(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            IComparable o1 = null;
            IComparable o2 = null;

            //see if the facts are not equal
            object replacement;
            try
            {
                o1 = (IComparable)op1.Value.Value;
                o2 = (IComparable)op2.Value.Value;

                var result = o1.CompareTo(o2);

                if (result == 1 || result == 0)
                {
                    replacement = true;
                }
                else
                {
                    replacement = false;
                }
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }
            Debug.WriteLine("ExpressionEvaluator {0} >= {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }
        
        private static Symbol EvaluateLessThanEqual(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            IComparable o1 = null;
            IComparable o2 = null;

            //see if the facts are not equal
            object replacement;
            try
            {
                o1 = (IComparable)op1.Value.Value;
                o2 = (IComparable)op2.Value.Value;

                var result = o1.CompareTo(o2);

                if (result == -1 || result == 0)
                {
                    replacement = true;
                }
                else
                {
                    replacement = false;
                }
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }
            Debug.WriteLine("ExpressionExaluator {0} <= {1} = {2}", o1, o2, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateIsNullOrEmpty(Symbol op1)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            string input = null;

            object replacement;
            try
            {
                input = Convert.ToString(op1.Value.Value);
                replacement = (op1.SymbolType == Type.Invalid || op1.Value.Value == null || string.IsNullOrEmpty(Convert.ToString(op1.Value.Value)));
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            op3.Value = new Naked(replacement, typeof(bool));

            Debug.WriteLine("ExpressionEvaluator ISNULLOREMPTY {0} = {1}", op1.Name, op3.Value.Value);

            return op3;
        }

        private static Symbol EvaluateRegex(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            string input = null;
            string regex = null;

            object replacement;
            try
            {
                input = Convert.ToString(op1.Value.Value);
                regex = Convert.ToString(op2.Value.Value);

                Match match = Regex.Match(input, regex);
                replacement = match.Success;
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator {0} @REGEX {1} = {2}", input, regex, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateMinLength(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            string input = null;
            int minLength = -1;

            object replacement;
            try
            {
                input = Convert.ToString(op1.Value.Value);
                minLength = Convert.ToInt32(op2.Value.Value);

                replacement = input.Length >= minLength;
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator {0} @MINLENGTH {1} = {2}", input, minLength, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateMaxLength(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            string input = null;
            int maxLength = -1;

            object replacement;
            try
            {
                input = Convert.ToString(op1.Value.Value);
                maxLength = Convert.ToInt32(op2.Value.Value);

                replacement = input.Length <= maxLength;
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator {0} @MAXLENGTH {1} = {2}", input, maxLength, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateSubstring(Symbol op1, Symbol op2)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            string candidate = null;
            string baseTester = null;

            object replacement;
            try
            {
                candidate = Convert.ToString(op1.Value.Value);
                baseTester = Convert.ToString(op2.Value.Value);

                replacement = baseTester.Contains(candidate);
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator {0} @SUBSTRING {1} = {2}", candidate, baseTester, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        private static Symbol EvaluateIsValidDate(Symbol op1)
        {
            var op3 = new Symbol { SymbolType = Type.Value };

            DateTime d1 = DateTime.MinValue;

            object replacement;
            try
            {
                string normalString = Convert.ToString(op1.Value.Value).Replace("T", " ").Replace("Z", string.Empty);
                if (normalString.IndexOf(".") >= 0)
                {
                normalString = normalString.Substring(0, normalString.IndexOf("."));
                }

                replacement = (DateTime.TryParse(normalString, out d1) && (d1 != DateTime.MinValue));
            }
            catch
            {
                op3.SymbolType = Type.Invalid;
                op3.Value = null;
                replacement = op3;
            }

            Debug.WriteLine("ExpressionEvaluator ISVALIDDATE {0} = {1}", op1.Value.Value, replacement);

            op3.Value = new Naked(replacement, typeof(bool));

            return op3;
        }

        #endregion
    }
}
