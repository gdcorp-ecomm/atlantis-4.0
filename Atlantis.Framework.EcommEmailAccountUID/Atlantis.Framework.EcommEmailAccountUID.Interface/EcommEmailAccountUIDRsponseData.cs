using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommEmailAccountUID.Interface
{

  public class EcommEmailAccountUIDRsponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    List<EmailResults> _emailResults = new List<EmailResults>();

    public List<EmailResults> EmailResults
    {
      get
      {
        return _emailResults;
      }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public EcommEmailAccountUIDRsponseData(List<EmailResults> results)
    {
      _emailResults = results;
      _success = true;
    }

    public EcommEmailAccountUIDRsponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public EcommEmailAccountUIDRsponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "EcommEmailAccountUIDRsponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("success", _success.ToString());
      xtwRequest.WriteEndElement();

      return sbResult.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }

  public class EmailResults
  {

    public int ResourceID;
    public int EmailPFid;
    public string ExternalResourceID;

    public EmailResults()
    {

    }

    public EmailResults(int resourceID, int emailPfid, string externalResourceID)
    {
      ResourceID = resourceID;
      EmailPFid = emailPfid;
      ExternalResourceID = externalResourceID;
    }


  }

}
