using System;
using System.IO;

namespace Atlantis.Framework.RazorEngine.Templating
{
  /// <summary>
  /// Defines a template writer used for helper templates.
  /// </summary>
  internal class TemplateWriter
  {
    private readonly Action<TextWriter> _writerDelegate;

    /// <summary>
    /// Initialises a new instance of <see cref="TemplateWriter"/>.
    /// </summary>
    /// <param name="writer">The writer delegate used to write using the specified <see cref="TextWriter"/>.</param>
    internal TemplateWriter(Action<TextWriter> writer)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");

      _writerDelegate = writer;
    }

    /// <summary>
    /// Executes the write delegate and returns the result of this <see cref="TemplateWriter"/>.
    /// </summary>
    /// <returns>The string result of the helper template.</returns>
    public override string ToString()
    {
      using (var writer = new StringWriter())
      {
        _writerDelegate(writer);
        return writer.ToString();
      }
    }
  }
}