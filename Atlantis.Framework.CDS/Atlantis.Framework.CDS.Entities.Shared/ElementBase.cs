﻿namespace Atlantis.Framework.CDS.Entities.Shared
{
  public class ElementBase
  {
    public ElementBase()
    {
    }

    public string Text { get; set; }
    public string DataCenters { get; set; }
    public string Split { get; set; }
    public int ProductGroupOffered { get; set; }
    public bool ManagerOnly { get; set; }
    public bool? TechSupportIs24X7 { get; set; }
  }
}
