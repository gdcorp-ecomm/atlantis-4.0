using System;
using System.IO;
using System.Text;

namespace Atlantis.Framework.Render.MarkupParser
{
  internal class Parser : IDisposable
  {
    private const string CARRAIGE_RETURN = "\r\n";

    private string _preProcessorPrefix;
    private StringBuilder _outputMarkup;
    private TextReader _markupReader;
    private MarkupLanguageSymbol _currentSymbol;
    private int _currentLineNumber;
    private string _currentLine;
    private string _currentLineTrimmed;
    private int _currentIfStatementCount;
    private MarkupParserManager.EvaluateExpressionDelegate _evaluateExpressionDelegate;

    internal string ParseAndEvaluate(string markup, string preProcessorPrefix, MarkupParserManager.EvaluateExpressionDelegate evaluateExpressionDelegate)
    {
      ResetParser(markup, preProcessorPrefix, evaluateExpressionDelegate);
      BeginMarkupParsing();

      return _outputMarkup.ToString();
    }

    private void ResetParser(string markup, string preProcessorPrefix, MarkupParserManager.EvaluateExpressionDelegate evaluateExpressionDelegate)
    {
      _preProcessorPrefix = preProcessorPrefix;
      _markupReader = new StringReader(markup ?? string.Empty);
      _outputMarkup = new StringBuilder(markup == null ? 0 : markup.Length);
      _currentSymbol = MarkupLanguageSymbol.Text;
      _currentLineNumber = 0;
      _currentIfStatementCount = 0;
      _evaluateExpressionDelegate = evaluateExpressionDelegate;
    }

    private void BeginMarkupParsing()
    {
      GetLine();
      while (_currentSymbol != MarkupLanguageSymbol.None)
      {
        ParseFirstLineSequence();
      }
    }

    private void GetLine()
    {
      _currentLine = _markupReader.ReadLine();

      if (_currentLine != null)
      {
        _currentLineTrimmed = _currentLine.Trim();
        DetermineCurrentSymbol();
        _currentLineNumber++;
      }
      else
      {
        _currentLineTrimmed = null;
        _currentSymbol = MarkupLanguageSymbol.None;
      }
    }

    private void DetermineCurrentSymbol()
    {
      if (IsPreProcessorSymbol())
      {
        _currentSymbol = LanguageSymbolFactory.GetPreProcessorSymbol(_currentLineTrimmed, _preProcessorPrefix);

        switch (_currentSymbol)
        {
          case MarkupLanguageSymbol.None:
            ExceptionHelper.ThrowParseError("Invalid pre-preocessor symbol: " + _currentLineTrimmed, _currentLineNumber, _currentLine);
            break;
          case MarkupLanguageSymbol.Else:
          case MarkupLanguageSymbol.ElseIf:
          case MarkupLanguageSymbol.EndIf:
            if (_currentIfStatementCount <= 0)
            {
              ExceptionHelper.ThrowParseError(string.Format("Missing starting {0}if condition.", _preProcessorPrefix), _currentLineNumber, _currentLine);
            }
            break;
        }
      }
      else
      {
        _currentSymbol = MarkupLanguageSymbol.Text;
      }
    }

    private bool IsPreProcessorSymbol()
    {
      return _currentLineTrimmed.Length > 1 &&
             _currentLineTrimmed.Substring(0, 2).Equals(_preProcessorPrefix, StringComparison.Ordinal);
    }

    private void ParseFirstLineSequence()
    {
      ParseLineSequence(true);
    }

    private void ParseLineSequence(bool isParentConditionTrue)
    {
      while (_currentSymbol == MarkupLanguageSymbol.Text || _currentSymbol == MarkupLanguageSymbol.If)
      {
        switch (_currentSymbol)
        {
          case MarkupLanguageSymbol.Text:
            ProcessTextLine(isParentConditionTrue);
            break;
          case MarkupLanguageSymbol.If:
            ProcessIfStatement(isParentConditionTrue);
            break;
        }
      }
    }

    private void ProcessTextLine(bool writeTextToOutput)
    {
      if (writeTextToOutput)
      {
        if (_outputMarkup.Length == 0)
        {
          _outputMarkup.Append(_currentLine);
        }
        else
        {
          _outputMarkup.Append(CARRAIGE_RETURN + _currentLine);
        }
      }

      GetLine();
    }

    private void ProcessIfStatement(bool isParentConditionTrue)
    {
      _currentIfStatementCount++;

      bool hasConditionBeenMet = ProcessIfLine(isParentConditionTrue);
      bool wasElseConditonParsed = false;

      while (_currentSymbol == MarkupLanguageSymbol.ElseIf || _currentSymbol == MarkupLanguageSymbol.Else)
      {
        switch (_currentSymbol)
        {
          case MarkupLanguageSymbol.ElseIf:
            bool wasIfElseConditonMet = ProcessElseIfLine(!hasConditionBeenMet && isParentConditionTrue);
            if (wasIfElseConditonMet)
            {
              hasConditionBeenMet = true;
            }
            break;
          case MarkupLanguageSymbol.Else:
            ProcessElseLine(!hasConditionBeenMet && isParentConditionTrue);
            wasElseConditonParsed = true;
            break;
        }

        if (wasElseConditonParsed)
        {
          break;
        }
      }

      if (_currentSymbol != MarkupLanguageSymbol.EndIf)
      {
        ExceptionHelper.ThrowParseError("\"" + _preProcessorPrefix + "endif\" expected.", _currentLineNumber, _currentLine);
      }

      _currentIfStatementCount--;

      GetLine();
    }

    private bool ProcessIfLine(bool evaluateExpression)
    {
      bool expressionResult = false;

      if (evaluateExpression)
      {
        string expression = ParseExpression(_preProcessorPrefix + "if");
        expressionResult = EvaluateExpression(expression);
      }

      GetLine();
      ParseLineSequence(expressionResult);

      return expressionResult;
    }

    private bool ProcessElseIfLine(bool evaluateExpression)
    {
      bool expressionResult = false;

      if (evaluateExpression)
      {
        string expression = ParseExpression(_preProcessorPrefix + "elseif");
        expressionResult = EvaluateExpression(expression);
      }

      GetLine();
      ParseLineSequence(expressionResult);

      return expressionResult;
    }

    private void ProcessElseLine(bool evaluateExpression)
    {
      GetLine();
      ParseLineSequence(evaluateExpression);
    }

    private bool EvaluateExpression(string expression)
    {
      bool expressionResult = false;

      try
      {
        expressionResult = _evaluateExpressionDelegate.Invoke(expression);
      }
      catch (Exception ex)
      {
        ExceptionHelper.ThrowParseError(string.Format("Error evaluating expression. " + ex.Message), _currentLineNumber, _currentLine);
      }

      return expressionResult;
    }

    private string ParseExpression(string preProcessorSymbol)
    {
      return _currentLineTrimmed.Replace(preProcessorSymbol, string.Empty);
    }

    public void Dispose()
    {
      if (_markupReader != null)
      {
        _markupReader.Dispose();
      }
    }
  }
}