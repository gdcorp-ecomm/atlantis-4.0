﻿using System;
using System.Web.Razor.Parser.SyntaxTree;

namespace Atlantis.Framework.RazorEngine.Templating
{
  /// <summary>
  /// Defines an exception that occurs during parsing of a template.
  /// </summary>
  internal class TemplateParsingException : Exception
  {
    /// <summary>
    /// Initialises a new instance of <see cref="TemplateParsingException"/>
    /// </summary>
    internal TemplateParsingException(RazorError error) : base(error.Message)
    {
      Column = error.Location.CharacterIndex;
      Line = error.Location.LineIndex;
    }

    /// <summary>
    /// Gets the column the parsing error occured.
    /// </summary>
    internal int Column { get; private set; }

    /// <summary>
    /// Gets the line the parsing error occured.
    /// </summary>
    internal int Line { get; private set; }
  }
}