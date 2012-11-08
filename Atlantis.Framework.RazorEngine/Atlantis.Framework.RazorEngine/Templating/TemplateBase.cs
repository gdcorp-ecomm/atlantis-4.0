
namespace Atlantis.Framework.RazorEngine.Templating
{
  using System.IO;
  using System.Text;

  public abstract class TemplateBase : ITemplate
  {
    protected TemplateBase()
    {
      Builder = new StringBuilder();
    }

    public StringBuilder Builder { get; private set; }

    public string Result { get { return Builder.ToString(); } }

    public TemplateService Service { get; set; }

    public void Clear()
    {
      Builder.Clear();
    }

    public virtual void Execute() { }

    public void Write(object @object)
    {
      if (@object == null)
        return;

      Builder.Append(@object);
    }

    public void WriteLiteral(string @string)
    {
      if (@string == null)
        return;

      Builder.Append(@string);
    }

    public static void WriteLiteralTo(TextWriter writer, string literal)
    {
      if (literal == null)
        return;

      writer.Write(literal);
    }

    public static void WriteTo(TextWriter writer, object obj)
    {
      if (obj == null)
        return;

      writer.Write(obj);
    }
  }
}