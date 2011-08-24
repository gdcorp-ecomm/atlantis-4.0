using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCValidateNS.Interface
{
  public class DCCValidateNSRequestData : RequestData
  {
    public int PrivateLabelId { get; set; }

   
    private List<string> _nameServers = new List<string>();
    public List<string> NameServers
    {
      get
      {
        return _nameServers;
      }
      set
      {
        _nameServers = value;
      }
    }

    public DCCValidateNSRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId,List<string> nameServers)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      _nameServers = nameServers;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCValidateNSRequest is not a cacheable request.");
    }
  }
}
