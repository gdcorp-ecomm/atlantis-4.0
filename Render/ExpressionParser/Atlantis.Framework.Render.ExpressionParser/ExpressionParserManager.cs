﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using NCalc;

namespace Atlantis.Framework.Render.ExpressionParser
{
  public class ExpressionParserManager : IExpressionParserManager
  {
    public delegate bool EvaluateFunctionDelegate(string functionName, IList<string> parameters, IProviderContainer providerContainer);

    private IProviderContainer ProviderContainer { get; set; }

    public EvaluateFunctionDelegate EvaluateExpressionHandler { get; set; }

    public ExpressionParserManager(IProviderContainer providerContainer)
    {
      ProviderContainer = providerContainer;
    }

    private void EvaluateFunction(string functionName, FunctionArgs functionArgs)
    {
      IList<string> expectedValues = GetExpectedValues(functionArgs);

      if (EvaluateExpressionHandler != null)
      {
        functionArgs.Result = EvaluateExpressionHandler.Invoke(functionName, expectedValues, ProviderContainer);
      }
      else
      {
        throw new ArgumentException("\"EvaluateFunctionHandler\" must be set.");
      }
    }

    private IList<string> GetExpectedValues(FunctionArgs functionArgs)
    {
      IList<string> expectedValues = new List<string>(functionArgs.Parameters.Length);

      foreach (Expression expression in functionArgs.Parameters)
      {
        expectedValues.Add(expression.ParsedExpression.ToString().Trim('[', ']'));
      }

      return expectedValues;
    }

    public bool EvaluateExpression(string rawExpression)
    {
      bool evaluationResult = false;

      Expression expression = new Expression(rawExpression);
      expression.EvaluateFunction += EvaluateFunction;

      object evaluationResultObject = expression.Evaluate();
      if (evaluationResultObject is bool)
      {
        evaluationResult = (bool)evaluationResultObject;
      }

      return evaluationResult;
    }
  }
}