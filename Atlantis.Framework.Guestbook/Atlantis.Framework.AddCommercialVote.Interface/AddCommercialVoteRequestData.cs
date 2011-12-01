using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddCommercialVote.Interface
{
  public class AddCommercialVoteRequestData : RequestData
  {
    private string _commercial = string.Empty;
    private string _clientip = string.Empty;

    public string Commercial
    {
      get { return _commercial; }
    }

    public string ClientIp
    {
      get { return _clientip; }
    }

    public AddCommercialVoteRequestData(
    string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, 
    string commercial, string clientIp)
    : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _commercial = commercial;
      _clientip = clientIp;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AddCommercialVote is not cacheable.");
    }
}
}
