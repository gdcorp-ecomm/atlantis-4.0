using System.Xml;
using System.Collections.Generic;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  /// <summary>
  /// Summary description for DomainItem
  /// </summary>
  public class CartDomainItem : Dictionary<string, string>
  {

    private bool _isDomain;
    XmlDocument _customXmlDoc;

    private readonly List<CartDomainEntry> _domainList = new List<CartDomainEntry>();

    public List<CartDomainEntry> DomainList
    {
      get
      {
        return _domainList;
      }
    }

    #region DomainProperites

   
    private bool _isProxy;

    public XmlDocument customXmlDoc
    {
      get { return _customXmlDoc; }
    }

    public bool IsProxy
    {
      get
      {
        return _isProxy;
      }
      set
      {
        _isProxy = value;
      }
    } 

    public bool IsDomain
    {
      get { return _isDomain; }
    }

    private string _domainType;

    public string DomainType
    {
      get
      {
        return _domainType;
      }
    }

    #endregion

    #region LoadDomainInformation

    public void LoadDomainInfo(XmlDocument customXML)
    {
      _customXmlDoc = customXML;
      //Load bulkRegistration information
      LoadBulkDomains(customXML);
      LoadProxyBulkDomains(customXML);
      LoadProximaDomains(customXML);
      LoadSmartDomains(customXML);
      LoadRenewals(customXML);
      LoadProxyBulkDomainRenewals(customXML);
      LoadDomainBackorders(customXML);
      LoadCertifiedDomains(customXML);
      LoadDomainAuctions(customXML);
      LoadDomainMembers(customXML);
      LoadDomainBrokerage(customXML);
      LoadDomainTransfers(customXML);
      LoadDomainItemFeatureTypes(customXML);
      LoadDomainHybridParking(customXML);
      LoadDomainPrivateBackorder(customXML);
      LoadDomainConsolidation(customXML);
      LoadBulkRedemptions(customXML);
      LoadExpirationProtection(customXML);
      /*
       * Possible customXML's
       * premiumListing
commissionJunction
consolidation
privateConsolidation
hybridParkingRenewal
expirationProtectionNew
expirationProtectionRenewal
       * */
    }

    private void LoadDomainConsolidation(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//consolidation");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainConsolid;
      }
    }
    private void LoadDomainBackorders(XmlDocument customXML)
    {
      bool domainLoaded = LoadBackorderDomainSet(customXML, "//domainBackorder");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainBackorder;
      }
    }

    private void LoadDomainPrivateBackorder(XmlDocument customXML)
    {
      bool domainLoaded = LoadBackorderDomainSet(customXML, "//domainPrivateBackorder");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainPrivateBackorder;
      }
    }

    private void LoadDomainHybridParking(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//hybridParking");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainHybridPark;
      }
    }

    private void LoadDomainItemFeatureTypes(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//ItemFeatureTypes");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainItemFeature;
      }
    }

    private void LoadDomainTransfers(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainBulkTransfer");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainTransfer;
      }
    }

    private void LoadDomainBrokerage(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainBrokerage");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainBrokerage;
      }
    }

/*
    private void LoadDomainAppraisals(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainAppraisalBulk", (Dictionary<string, string>)DomainAppraisals, DomainAppraisals.DomainList);
      if (domainLoaded)
        _isDomain = true;
    }
*/

    private void LoadDomainMembers(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//MemberItems");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainMember;
      }
    }

    private void LoadDomainAuctions(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//ItemWinningBids");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainAuction;
      }
    }

    private void LoadCertifiedDomains(XmlDocument customXML)
    {
      bool domainLoaded=LoadDomainSet(customXML, "//certifiedDomain");
      if (domainLoaded)
      {
        _domainType = CartDomainItemTypes.CertifiedDomain;
      }
    }

    private void LoadRenewals(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainBulkRenewal");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainRenewal;
      }
    }

    private void LoadSmartDomains(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//smartDomain");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.SmartDomain;
      }
    }

    private void LoadBulkDomains(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainBulkRegistration");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.BulkDomain;
      }
    }

    private void LoadBulkRedemptions(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainBulkRedemption");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.DomainRedemption;
      }
    }

    private void LoadProxyBulkDomainRenewals(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainByProxyBulkRenewal");
      if (domainLoaded)
      {
        _domainType = CartDomainItemTypes.ProxyDomain;
        _isDomain = true;
        if (DomainList.Count > 0)
        {
          IsProxy = true;
        }
      }
    }

    private void LoadProxyBulkDomains(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//domainByProxyBulk");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.ProxyDomain;
        if (DomainList.Count > 0)
        {
          IsProxy = true;
        }
      }
    }

    private void LoadProximaDomains(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//Proxima");
      if (domainLoaded)
      {
        _isDomain = true;
        _domainType = CartDomainItemTypes.BusinessRegistration;
        if (DomainList.Count > 0)
        {
          IsProxy = true;
        }
      }
    }

    private void LoadExpirationProtection(XmlDocument customXML)
    {
      bool domainLoaded = LoadDomainSet(customXML, "//expirationProtectionNew");
      if (domainLoaded)
      {
        _domainType = CartDomainItemTypes.ExpirationProtection;
      }
    }

    private bool LoadBackorderDomainSet(XmlDocument customXML, string xpath)
    {
      XmlElement backorderDomainElement = customXML.SelectSingleNode(xpath) as XmlElement;
      if (backorderDomainElement == null)
        return false;
      CartDomainEntry newEntry = new CartDomainEntry();
      foreach (XmlAttribute regAtt in backorderDomainElement.Attributes)
      {
        newEntry[regAtt.Name] = regAtt.Value;
      }
      _domainList.Add(newEntry);      
      return true;
    }

    private bool LoadDomainSet(XmlDocument customXML, string xpath)
    {
      XmlElement backorderDomainElement = customXML.SelectSingleNode(xpath) as XmlElement;
      if (backorderDomainElement == null)
        return false;
      foreach (XmlAttribute regAtt in backorderDomainElement.Attributes)
      {

        this[regAtt.Name] = regAtt.Value;
      }
      //Load DomainSet
      foreach (XmlElement domainEntryElement in backorderDomainElement.ChildNodes)
      {
        CartDomainEntry newEntry = new CartDomainEntry();
        foreach (XmlAttribute regAtt in domainEntryElement.Attributes)
        {
          newEntry[regAtt.Name] = regAtt.Value;
        }
        _domainList.Add(newEntry);
      }
      return true;
    }   

    #endregion

    public int GetDomainCount()
    {
      int domainCount = 0;
      switch (DomainType)
      {
        case CartDomainItemTypes.BusinessRegistration:
        case CartDomainItemTypes.ProxyDomain:
        case CartDomainItemTypes.BulkDomain:
        case CartDomainItemTypes.DomainRenewal:
        case CartDomainItemTypes.SmartDomain:
        case CartDomainItemTypes.DomainAuction:
        case CartDomainItemTypes.DomainMember:
        case CartDomainItemTypes.DomainAppraisal:
        case CartDomainItemTypes.DomainTransfer:
        case CartDomainItemTypes.DomainItemFeature:
        case CartDomainItemTypes.DomainHybridPark:
        case CartDomainItemTypes.DomainBackorder:
        case CartDomainItemTypes.DomainConsolid:
          domainCount=DomainList.Count;
          break;
        case CartDomainItemTypes.DomainExpiredAuction: // explicitly bypassed because it has no domain loading logic, and the parent item contains the domain
          break;
      }
      return domainCount;
    }

    public bool FindDomain(string tld, string sld)
    {
      foreach (CartDomainEntry currentDomain in _domainList)
      {
        if ((currentDomain.SecondLevelDomain == sld) && currentDomain.TopLevelDomain == tld)
          return true;
      }
      return false;
    }
  }
}
