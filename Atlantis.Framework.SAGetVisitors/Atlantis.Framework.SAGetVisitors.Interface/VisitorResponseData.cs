using System.Collections.Generic;
using Atlantis.Framework.SA.Interface;

namespace Atlantis.Framework.SAGetVisitors.Interface
{
  public class VisitorResponseData : SAResponseBase
  {
    private List<Visit> _visits;
    public List<Visit> Visits
    {
      get { return _visits ?? (_visits = new List<Visit>()); }
      set { _visits = value; }
    }
  }
}
