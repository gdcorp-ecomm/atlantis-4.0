using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegGetCAAgreement.Interface
{
  public class RegGetCAAgreementResponseData : IResponseData
  {
    private string _responseXML;
    private AtlantisException _atlException;

    public string CheckSum { get; private set; }
    public string Version { get; private set; }
    public string AgreementText { get; private set; }
    public string IntroText { get; private set; }
    public string TextAboveIAgree { get; private set; }

    public RegGetCAAgreementResponseData(string responseXML)
    {
      _responseXML = responseXML;
      PopulateFromXML();
    }

    public RegGetCAAgreementResponseData(AtlantisException exAtlantis)
    {
      _responseXML = "";
      _atlException = exAtlantis;
      PopulateFromXML();
    }

    public RegGetCAAgreementResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXML = sResponseXML;
      _atlException = new AtlantisException(oRequestData, "RegGetCAAgreementResponseData", ex.Message, string.Empty);

      PopulateFromXML();
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return _responseXML;
    }

    #endregion

    #region Private Methods

    private void PopulateFromXML()
    {
      CheckSum = string.Empty;
      Version = string.Empty;
      AgreementText = string.Empty;
      IntroText = string.Empty;
      TextAboveIAgree = string.Empty;

      if (!string.IsNullOrEmpty(_responseXML))
      {
        XmlDocument responseDoc = new XmlDocument();
        responseDoc.LoadXml(_responseXML);
        XmlNodeList nodes = responseDoc.SelectNodes("//results/success");

        if (nodes.Count > 0)
        {
          if (nodes[0].HasChildNodes)
          {
            XmlNodeList childNodes = nodes[0].ChildNodes;
            if (childNodes.Count > 0)
            {
              CheckSum = childNodes[0].InnerText;
              if (childNodes[1].HasChildNodes)
              {
                XmlNodeList siblingNodes = childNodes[1].ChildNodes;

                if (siblingNodes.Count > 0)
                {
                  foreach (XmlNode node in siblingNodes)
                  {
                    if (node.Name.ToLower() == "version")
                    {
                      Version = node.InnerText;
                    }
                    else if (node.Name.ToLower() == "agreement_text")
                    {
                      AgreementText = node.InnerText;
                    }
                    else if (node.Name.ToLower() == "intro_text")
                    {
                      IntroText = node.InnerText;
                    }
                    else if (node.Name.ToLower() == "text_above_i_agree")
                    {
                      TextAboveIAgree = node.InnerText;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    #endregion
  }
}
