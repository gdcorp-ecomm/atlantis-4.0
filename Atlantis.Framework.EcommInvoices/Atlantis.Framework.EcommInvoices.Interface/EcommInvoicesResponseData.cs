using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  class EcommInvoicesResponseData : IResponseData
  {

    public EcommInvoicesResponseData()
    {

    }

    public EcommInvoicesResponseData(RequestData oRequestData, Exception ex)
    {
     // _success = false;
    //  _atlException = new AtlantisException(oRequestData, "EcommInvoicesResponseData", ex.Message, string.Empty);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      throw new NotImplementedException();
    }
  }
}

