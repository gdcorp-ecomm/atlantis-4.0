using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetHostingCredits.Interface
{
  public class MYAGetHostingCreditsResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly List<HostingCredit> _hostingCredits;

    public bool IsSuccess { get; private set; }

    public List<HostingCredit> HostingCredits
    {
      get { return _hostingCredits; }
    }

    public MYAGetHostingCreditsResponseData(string xml)
    {
      _hostingCredits = new List<HostingCredit>();

      if (!string.IsNullOrEmpty(xml))
      {
        var xdoc = new XmlDocument();
        xdoc.LoadXml(xml);
        XmlNodeList hostingCreditNodes = xdoc.SelectNodes("credits/credit");

        if (hostingCreditNodes != null)
        {
          foreach (XmlNode node in hostingCreditNodes)
          {
            var hc = new HostingCredit
                       {
                         Id = Convert.ToInt32(node.Attributes["id"].Value),
                         Count = Convert.ToInt32(node.Attributes["count"].Value)
                       };

            _hostingCredits.Add(hc);
          }
        }
      }
      IsSuccess = true;

    }

    public MYAGetHostingCreditsResponseData(List<HostingCredit> hostingCredits)
    {
      _hostingCredits = hostingCredits;
      IsSuccess = true;
    }

     public MYAGetHostingCreditsResponseData(AtlantisException atlantisException)
     {
       IsSuccess = false;
       _exception = atlantisException;
     }

    public MYAGetHostingCreditsResponseData(RequestData requestData, Exception exception)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData,
                                         "MYAGetHostingCreditsResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      var resultDoc = new XDocument();
      var resultRoot = new XElement("credits");
      resultDoc.Add(resultRoot);

      foreach (HostingCredit hc in HostingCredits)
      {
        resultRoot.Add(new XElement("credit",
          new XAttribute("id", hc.Id.ToString()),
          new XAttribute("count", hc.Count.ToString())));
      }
      return resultDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
