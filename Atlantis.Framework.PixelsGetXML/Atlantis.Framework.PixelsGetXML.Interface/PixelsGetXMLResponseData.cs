using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PixelsGetXML.Interface
{
  public class PixelsGetXMLResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return (_exception == null && PixelsXML != null); }
    }

    public XDocument PixelsXML { get; set; }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return PixelsXML.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
