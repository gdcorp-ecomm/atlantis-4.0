using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.OpenGraphMeta.Widgets
{
  public class OpenGraphMeta : IWidgetModel
  {
    public OpenGraphMeta()
    {
      AllowRobots = true;
    }

    public bool AllowRobots
    {
      get;
      set;
    }

    public string Canonical
    {
      get;
      set;
    }

    public string Description
    {
      get;
      set;
    }

    public string Keywords
    {
      get;
      set;
    }

    public string OpenGraphDescription
    {
      get;
      set;
    }

    public string OpenGraphImageUrl
    {
      get;
      set;
    }

    public string OpenGraphItemType
    {
      get;
      set;
    }

    public string OpenGraphShareUrl
    {
      get;
      set;
    }

    public string OpenGraphSiteName
    {
      get;
      set;
    }

    public string OpenGraphTitle
    {
      get;
      set;
    }

    public string FavoriteIcon
    {
      get;
      set;
    }

    public string Title
    {
      get;
      set;
    }

  }
}
