using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Render.MarkupParser
{
  internal class Parser : IDisposable
  {
    private static readonly Regex _removeWhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private string _preProcessorPrefix;
    private StringBuilder _outputMarkup;
    private TextReader _markupReader;
    private MarkupLanguageSymbol _currentSymbol;
    private int _currentLineNumber;
    private string _currentLine;
    private int _currentIfStatementCount;
    private MarkupParserManager.EvaluateExpressionDelegate _evaluateExpressionDelegate;

    internal StringBuilder ParseAndEvaluate(string markup, string preProcessorPrefix, MarkupParserManager.EvaluateExpressionDelegate evaluateExpressionDelegate)
    {
      ResetParser(markup, preProcessorPrefix, evaluateExpressionDelegate);
      BeginMarkupParsing();

      return _outputMarkup;
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
      ParseNextLine();
      while (_currentSymbol != MarkupLanguageSymbol.None)
      {
        ParseFirstLineSequence();
      }
    }

    private void ParseNextLine()
    {
      string currentLine = GetLine();

      if (currentLine == null)
      {
        _currentSymbol = MarkupLanguageSymbol.None;
        _currentLine = null;
      }
      else
      {
        _currentLine = currentLine;
        string currentLineNoWhiteSpace = _removeWhiteSpaceRegex.Replace(currentLine, string.Empty);

        if (currentLineNoWhiteSpace.Length > 1 && currentLineNoWhiteSpace.Substring(0, 2).Equals(_preProcessorPrefix, StringComparison.Ordinal))
        {
          _currentSymbol = LanguageSymbolFactory.GetPreProcessorSymbol(currentLineNoWhiteSpace, _preProcessorPrefix);
          switch (_currentSymbol)
          {
            case MarkupLanguageSymbol.None:
              ExceptionHelper.ThrowParseError("Invalid pre-preocessor symbol " + currentLineNoWhiteSpace, _currentLineNumber, _currentLine);
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
    }

    private void ParseFirstLineSequence()
    {
      ParseLineSequence(true);
    }

    private string GetLine()
    {
      _currentLine = _markupReader.ReadLine();
      if (_currentLine != null)
      {
        _currentLineNumber++;
      }

      return _currentLine;
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
        _outputMarkup.AppendLine(_currentLine);
      }
      ParseNextLine();
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

      ParseNextLine();
    }

    private bool ProcessIfLine(bool evaluateExpression)
    {
      bool expressionResult = false;

      if (evaluateExpression)
      {
        string expression = ParseExpression(_preProcessorPrefix + "if");
        expressionResult = EvaluateExpression(expression);
      }

      ParseNextLine();
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

      ParseNextLine();
      ParseLineSequence(expressionResult);

      return expressionResult;
    }

    private void ProcessElseLine(bool evaluateExpression)
    {
      ParseNextLine();
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
        ExceptionHelper.ThrowParseError(string.Format("Error evaluation expression. " + ex.Message), _currentLineNumber, _currentLine);
      }

      return expressionResult;
    }

    private string ParseExpression(string preProcessorSymbol)
    {
      return _removeWhiteSpaceRegex.Replace(_currentLine, string.Empty).Replace(preProcessorSymbol, string.Empty);
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
