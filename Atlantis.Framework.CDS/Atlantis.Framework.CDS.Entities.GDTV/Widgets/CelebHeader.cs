using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class CelebHeader : IWidgetModel
  {
    public string HeaderImage { get; set; }
    public string CelebSignature { get; set; }
    public string TwitterOrigReferrer { get; set; }
    public string TwitterText { get; set; }
    public string TwitterPageUrl { get; set; }
    public string FacebookLikeUrl { get; set; }
  }
}
