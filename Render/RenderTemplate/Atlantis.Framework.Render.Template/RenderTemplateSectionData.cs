using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class RenderTemplateSectionData
  {
    private const int MAX_LINES = 256000;
    private const string SECTION_END = "</template-section>";
    private const string TEMPLATE_DATA = "<template-data";
    private const string CARRAIGE_RETURN = "\r\n";

    private static readonly Regex _sectionBeginRegex = new Regex(@"<template-section\s+name=""(?<name>\w+)""\s*>", RegexOptions.Compiled);
    private static readonly IDictionary<string, string> _emptyAttributeDictionary = new Dictionary<string, string>(0); 

    private readonly IDictionary<string, IRenderTemplateSection> _renderSectionDictionary = new Dictionary<string, IRenderTemplateSection>(32);

    private TextReader _markupReader;
    private int _currentLineNumber;

    private bool HasHitMaxLinesLimit
    {
      get { return _currentLineNumber > MAX_LINES; }
    }

    private bool IsLineTemplateData(string currentLine)
    {
      return currentLine.Contains(TEMPLATE_DATA);
    }

    private bool IsLineBeginSection(string currentLine, out string sectionName)
    {
      sectionName = string.Empty;
      Match beginMatch = _sectionBeginRegex.Match(currentLine);

      if (beginMatch.Success)
      {
        sectionName = beginMatch.Groups["name"].Value;
      }

      return beginMatch.Success;
    }

    private bool IsLineEndSection(string currentLine)
    {
      return currentLine.Contains(SECTION_END);
    }

    private void ProcessSections()
    {
      string currentLine;
      LineData lineData = GetNextLine(out currentLine);

      while (lineData.Type != LineType.EndContent && !HasHitMaxLinesLimit)
      {
        if (lineData.Type != LineType.BeginSection)
        {
          lineData = GetNextLine(out currentLine);
        }
        else
        {
          string sectionName = lineData.Attributes["name"];
          
          StringBuilder sectionContentBuilder = new StringBuilder();

          lineData = GetNextLine(out currentLine);

          while (lineData.Type == LineType.Literal && !HasHitMaxLinesLimit)
          {
            if (sectionContentBuilder.Length == 0)
            {
              sectionContentBuilder.Append(currentLine);
            }
            else
            {
              sectionContentBuilder.Append(CARRAIGE_RETURN + currentLine);
            }

            lineData = GetNextLine(out currentLine);
          }

          if (sectionContentBuilder.Length > 0)
          {
            _renderSectionDictionary[sectionName] = new RenderTemplateSection { Name = sectionName, Content = sectionContentBuilder.ToString() }; 
          }
        }
      }
    }

    private LineData GetNextLine(out string currentLine)
    {
      LineData lineData;

      currentLine = _markupReader.ReadLine();
      _currentLineNumber++;

      if (currentLine == null)
      {
        lineData = new LineData { Type = LineType.EndContent, Attributes = _emptyAttributeDictionary };
      }
      else
      {
        string sectionName;
        if (IsLineBeginSection(currentLine, out sectionName))
        {
          lineData = new LineData { Type = LineType.BeginSection, Attributes = new Dictionary<string, string> { { "name", sectionName } } };
        }
        else if (IsLineEndSection(currentLine))
        {
          lineData = new LineData { Type = LineType.EndSection, Attributes = _emptyAttributeDictionary };
        }
        else if(IsLineTemplateData(currentLine))
        {
          lineData = new LineData { Type = LineType.TemplateData, Attributes = _emptyAttributeDictionary };
        }
        else
        {
          lineData = new LineData { Type = LineType.Literal, Attributes = _emptyAttributeDictionary };
        }
      }

      return lineData;
    }

    private bool InitializeMarkupReader(IRenderContent renderContent)
    {
      if (!string.IsNullOrEmpty(renderContent.Content))
      {
        _markupReader = new StringReader(renderContent.Content);
      }

      return _markupReader != null;
    }

    internal bool TryGetSection(string sectionName, out IRenderTemplateSection renderTemplateSection)
    {
      return _renderSectionDictionary.TryGetValue(sectionName, out renderTemplateSection);
    }

    internal RenderTemplateSectionData(IRenderContent renderContent)
    {
      if (InitializeMarkupReader(renderContent))
      {
        ProcessSections();
      }
    }
  }
}