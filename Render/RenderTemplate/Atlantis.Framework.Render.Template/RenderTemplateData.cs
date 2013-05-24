using System.Text.RegularExpressions;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class RenderTemplateData : IRenderTemplateData
  {
    private static readonly Regex _templateDataRegex = new Regex(@"<template-data\s+type=""(?<type>\w+)""\s+location=""(?<location>.+)""\s*/>", RegexOptions.Compiled);

    private readonly string _type = string.Empty;
    public string Type { get { return _type; } }

    private readonly string _location = string.Empty;
    public string Location { get { return _location; } }

    internal RenderTemplateData(IRenderContent renderContent)
    {
      if (!string.IsNullOrEmpty(renderContent.Content))
      {
        Match templateDataMatch = _templateDataRegex.Match(renderContent.Content);

        if (templateDataMatch.Success)
        {
          _type = templateDataMatch.Groups["type"].Value;
          _location = templateDataMatch.Groups["location"].Value;
        }
      }
    }
  }
}