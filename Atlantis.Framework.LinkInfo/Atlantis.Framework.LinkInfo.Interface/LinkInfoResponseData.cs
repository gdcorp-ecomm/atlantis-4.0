using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LinkInfo.Interface
{
  public class LinkInfoResponseData : IResponseData
  {
    Dictionary<string, string> _resultDictionary;
    AtlantisException _exception;

    public LinkInfoResponseData(Dictionary<string, string> dictLinkInfo)
    {
      _resultDictionary = dictLinkInfo;
      _exception = null;
    }

    public LinkInfoResponseData(Dictionary<string, string> dictResult, AtlantisException exAtlantis)
    {
      _resultDictionary = dictResult;
      _exception = exAtlantis;
    }

    public LinkInfoResponseData(Dictionary<string, string> dictResult, RequestData oRequestData, Exception ex)
    {
      _resultDictionary = dictResult;
      _exception = new AtlantisException(oRequestData,
                                   "GetLinkInfoResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    public Dictionary<string, string> Links
    {
      get { return _resultDictionary; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      StringBuilder result = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(result));

      xtwResult.WriteStartElement("Links");

      if (_resultDictionary != null)
      {
        foreach (KeyValuePair<string, string> oPair in _resultDictionary)
        {
          xtwResult.WriteStartElement("Link");

          xtwResult.WriteAttributeString("type", oPair.Key);
          xtwResult.WriteValue(oPair.Value);

          xtwResult.WriteEndElement();
        }
      }

      xtwResult.WriteEndElement();

      return result.ToString();
    }

    #endregion

  }
}
