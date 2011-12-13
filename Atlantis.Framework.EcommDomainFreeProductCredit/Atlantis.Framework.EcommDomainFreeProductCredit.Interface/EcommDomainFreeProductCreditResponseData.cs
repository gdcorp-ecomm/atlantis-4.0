using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommDomainFreeProductCredit.Interface
{
  public class EcommDomainFreeProductCreditResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    List<DomainResults> _domainResults = new List<DomainResults>();

    public List<DomainResults> DomainResults
    {
      get
      {
        return _domainResults;
      }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public EcommDomainFreeProductCreditResponseData(List<DomainResults> results)
    {
      _domainResults = results;
      _success = true;
    }

    public EcommDomainFreeProductCreditResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public EcommDomainFreeProductCreditResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "EcommDomainEmailCreditResponseData", ex.Message, string.Empty);
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

  public class DomainResults
  {
    public DomainResults()
    {
      DomainName = string.Empty;
      EmailPfid = new List<int>();
    }

    public DomainResults(string domainName, int emailPFid, int domainResourceID)
    {
      EmailPfid = new List<int>();
      DomainName = domainName;
      EmailPfid.Add(emailPFid);
      DomainResourceID = domainResourceID;
    }

    public void AddEmailPfid(int emailPfid)
    {
      if (!EmailPfid.Contains<int>(emailPfid))
      {
        EmailPfid.Add(emailPfid);
      }
    }

    public string DomainName { get; set; }
    public List<int> EmailPfid { get; set; }
    public int DomainResourceID { get; set; }
  }
}
