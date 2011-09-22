using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForShopper.Interface
{
  public class ECCGetAddressesForShopperResponseData : EccResponseDataBase<List<String>>
  {
    public List<String> Addresses
    {
      get
      {
        if (Response != null)
        {
          if (Response.Item != null)
          {
            if (Response.Item.Results != null)
              return Response.Item.Results[0].ToList();
          }
        }
        return null;
      }
    }

    private HashSet<String> _domains;
    public HashSet<String> Domains
    {
      get
      {
        if (_domains == null)
        {


          if (Addresses.Count > 0)
          {
            string[] stringSeperators = new[] {"@"};
            _domains = new HashSet<String>();

            foreach (string address in Addresses)
            {
              string[] result = address.Split(stringSeperators, 2, StringSplitOptions.None);
              _domains.Add(result[1].ToUpperInvariant());
            }
          }
        }
        return _domains;
      }
    }

    public ECCGetAddressesForShopperResponseData(string jsonResponse) : base (jsonResponse)
    {
    }

    public ECCGetAddressesForShopperResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }

  }
}
