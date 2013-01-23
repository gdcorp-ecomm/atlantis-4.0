using System.ComponentModel;

namespace Atlantis.Framework.CDS.Entities.Widgets.Dynamic
{
  public enum Tag
  {
    None,
    [Description("a")]
    Anchor,
    Div,
    [Description("select")]
    DropDown,
    Image,
    Input,
    Label,
    Link,
    [Description("li")]
    ListItem,
    Meta,
    [Description("ol")]
    OrderedList,
    Script,
    Span,
    Style,
    [Description("ul")]
    UnorderedList,
    Paragraph,
    TextArea
  }
}

