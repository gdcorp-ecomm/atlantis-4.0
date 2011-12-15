using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using Atlantis.Framework.MyaAccountList.Interface.Abstract;
using Atlantis.Framework.MyaAccountList.Interface.Concrete;

namespace Atlantis.Framework.MyaAccountList.Interface
{
  public class MyaAccountListRequestData : RequestData
  {
    #region Properties
    public class AccordionIds
    {
      public const int SearchEngineVisibility = 7;
      public const int ExpressEmailMarketing = 21;
    }

    public int? OverrideSEVWebsiteIdRequestType { get; set; }
    public int? OverrideEEMGetCustomerSummaryRequestType { get; set; }

    public AccordionMetaData AccordionData { get; private set; }
    public int? DaysTillExpiration { get; private set; }
    public string Filter { get; private set; }
    public IPageInfo PageInfo { get; private set; }
    public int ReturnAll { get; private set; }
    public int ReturnFreeListOnly { get; private set; }
    public string SortColumn { get; private set; }
    public string SortDirection { get; private set; }
    public string StoredProcName
    {
      get
      {
        string proc;
        try
        {
          XDocument contentXml = XDocument.Parse(AccordionData.ContentXml);
          XElement content = contentXml.Element("content");
          proc = content.Element("data").Attribute("accountlist").Value;
        }
        catch
        {
          proc = string.Empty;
        }

        return proc;
      }
    }
    
    #endregion

    public MyaAccountListRequestData(string shopperId
      , string sourceURL
      , string orderId
      , string pathway
      , int pageCount
      , AccordionMetaData accordionData
      , int pageSize = 5
      , int currentPage = 1
      , string sortDirection = "asc"
      , int? daysTillExpiration = null
      , int returnFreeListOnly = 0
      , int returnAllFlag = 0
      , string filter = null
      , string sortColumn = null)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccordionData = accordionData;
      PageInfo = new AccountListPagingInfo();
      PageInfo.PageSize = pageSize;
      PageInfo.CurrentPage = currentPage;
      SortDirection = sortDirection;
      Filter = string.IsNullOrWhiteSpace(filter) ? null : filter;
      SortColumn = string.IsNullOrWhiteSpace(sortColumn) ? null : sortColumn;
      ReturnFreeListOnly = returnFreeListOnly;
      DaysTillExpiration = daysTillExpiration;
      ReturnAll = returnAllFlag;
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MyaAccountList is not a cacheable request.");
    }
  }
}
