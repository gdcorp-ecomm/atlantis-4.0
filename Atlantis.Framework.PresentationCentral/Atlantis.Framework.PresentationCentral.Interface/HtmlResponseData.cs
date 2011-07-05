using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresentationCentral.Interface
{
  public class HtmlResponseData : IResponseData
  {
    #region Private Fields

    XmlNode _presentationCentralReturnedNode = null;
    AtlantisException _exception;

    #endregion

    #region Constructors

    public HtmlResponseData(XmlNode presentationCentralReturnedNode)
    {
      _presentationCentralReturnedNode = presentationCentralReturnedNode;
      _exception = null;
    }

    public HtmlResponseData(AtlantisException ex)
    {
      _presentationCentralReturnedNode = null;
      _exception = ex;
    }

    public HtmlResponseData(RequestData oRequestData, Exception ex)
    {
      _presentationCentralReturnedNode = null;
      _exception = new AtlantisException(oRequestData,
                     "HtmlResponseData",
                     ex.Message.ToString(),
                     oRequestData.ToXML());
    }

    #endregion

    #region Public Members

    public string GetHtml(string transformName)
    {
      string result = string.Empty;

      if (_presentationCentralReturnedNode != null)
      {
        XmlNode htmlNode = _presentationCentralReturnedNode.SelectSingleNode("//" + transformName);
        if (htmlNode != null)
          result = htmlNode.InnerText;
      }

      return result;
    }

    public string GetTransformAttribute(string transformName, string attributeName)
    {
      string result = string.Empty;
      XmlElement htmlElement = _presentationCentralReturnedNode.SelectSingleNode("//" + transformName) as XmlElement;
      if (htmlElement != null)
      {
        result = htmlElement.GetAttribute(attributeName);
      }
      return result;
    }

    public bool IsSuccess
    {
      get { return _presentationCentralReturnedNode.InnerXml.IndexOf("-ok-", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    #endregion

    #region IResponseData Members

    // **************************************************************** //

    public string ToXML()
    {
      return _presentationCentralReturnedNode.InnerXml;
    }

    // **************************************************************** //

    public AtlantisException GetException()
    {
      return _exception;
    }

    // **************************************************************** //

    #endregion
  }
}
