﻿using System.Web.UI;

namespace WebControls
{
  public class WebControlOne : Control
  {
    public string Text { get; set; }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<h1>{0}</h1>", !string.IsNullOrEmpty(Text) ? Text : "ERROR: \"Text\" parameter not found.");
    }
  }
}