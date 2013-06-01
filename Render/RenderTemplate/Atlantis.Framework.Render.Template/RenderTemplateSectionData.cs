using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class RenderTemplateSectionData
  {
    private const int MAX_LINES = 100000;
    private const string SECTION_END = "</template-section>";
    private const string TEMPLATE_DATA = "<template-data";

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

    private void AppendSectionContent(SectionData currentSectionData, LineData lineData)
    {
      if (currentSectionData != null)
      {
        currentSectionData.Append(lineData.Text);
      }
    }

    private void ProcessCurrentSection(SectionData currentSectionData)
    {
      IRenderTemplateSection renderTemplateSection;
      if (currentSectionData != null && currentSectionData.Flush(out renderTemplateSection))
      {
        _renderSectionDictionary[renderTemplateSection.Name] = renderTemplateSection;
      }
    }

    private SectionData CreateNextSection(LineData lineData)
    {
      SectionData sectionData = null;
      
      string sectionName;
      if (lineData.Attributes.TryGetValue("name", out sectionName))
      {
        sectionData = new SectionData(lineData.Attributes["name"]);
      }

      return sectionData;
    }

    private void ProcessSections()
    {
      LineData lineData = GetNextLine();
      SectionData currentSectionData = null;

      while (lineData.Type != LineType.EndContent && !HasHitMaxLinesLimit)
      {
        switch (lineData.Type)
        {
          case LineType.BeginSection:
            ProcessCurrentSection(currentSectionData);
            currentSectionData = CreateNextSection(lineData); 
            break;
          case LineType.EndSection:
          case LineType.EndContent:
            ProcessCurrentSection(currentSectionData);
            break;
          case LineType.Literal:
            AppendSectionContent(currentSectionData, lineData);
            break;
        }

        lineData = GetNextLine();
      }

      // Done with the loop, lets check to see if there was one last section to process
      ProcessCurrentSection(currentSectionData);
    }

    private LineData GetNextLine()
    {
      LineData lineData;

      string currentLine = _markupReader.ReadLine();
      _currentLineNumber++;

      if (currentLine == null)
      {
        lineData = new LineData { Text = string.Empty, Type = LineType.EndContent, Attributes = _emptyAttributeDictionary };
      }
      else
      {
        string sectionName;
        if (IsLineBeginSection(currentLine, out sectionName))
        {
          lineData = new LineData { Text = currentLine, Type = LineType.BeginSection, Attributes = new Dictionary<string, string> { { "name", sectionName } } };
        }
        else if (IsLineEndSection(currentLine))
        {
          lineData = new LineData { Text = currentLine, Type = LineType.EndSection, Attributes = _emptyAttributeDictionary };
        }
        else if(IsLineTemplateData(currentLine))
        {
          lineData = new LineData { Text = currentLine, Type = LineType.TemplateData, Attributes = _emptyAttributeDictionary };
        }
        else
        {
          lineData = new LineData { Text = currentLine, Type = LineType.Literal, Attributes = _emptyAttributeDictionary };
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