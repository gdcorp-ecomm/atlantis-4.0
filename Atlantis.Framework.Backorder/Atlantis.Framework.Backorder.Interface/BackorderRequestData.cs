using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Backorder.Interface
{
  public class BackorderRequestData : RequestData
  {
    #region Properties

    List<string> m_lstDomainNames = new List<string>();
    private int _privateLabelId = 0;

    #endregion

    #region Constructors

    public BackorderRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount,
                                int privateLabelId)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _privateLabelId = privateLabelId;
    }

    public BackorderRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount,
                                string sDomainName, int privateLabelId)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      AddDomainName(sDomainName);
      _privateLabelId = privateLabelId;
    }

    public BackorderRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount,
                                IEnumerable<string> oDomainNames, int privateLabelId)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      AddDomainNames(oDomainNames);
      _privateLabelId = privateLabelId;
    }

    #endregion

    #region Public Methods

    public void AddDomainName(string sDominName)
    {
      m_lstDomainNames.Add(sDominName.ToUpper());
    }

    public void AddDomainNames(IEnumerable<string> oDomainNames)
    {
      m_lstDomainNames.AddRange(oDomainNames);
    }

    #region RequestData Members

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("ismultipledomainbackorderallowed"); 

      xtwResult.WriteElementString("username","FOS"); //username

      xtwResult.WriteStartElement("domains");

      foreach (string sDomainName in m_lstDomainNames)
      {
        xtwResult.WriteStartElement("domain");
        xtwResult.WriteAttributeString("name", sDomainName);
        xtwResult.WriteAttributeString("privatelabelid", _privateLabelId.ToString());
        xtwResult.WriteEndElement(); // domain
      }

      xtwResult.WriteEndElement(); // domains

      xtwResult.WriteEndElement(); //ismultipledomainbackorderallowed

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new Exception("Backorder is not a cacheable request.");
    }

    #endregion

    #endregion
  }
}
