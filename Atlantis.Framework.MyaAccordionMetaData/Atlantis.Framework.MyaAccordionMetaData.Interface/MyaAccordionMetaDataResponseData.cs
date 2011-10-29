using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class MyaAccordionMetaDataResponseData : IResponseData
  {
    #region Properties
    private AtlantisException _exception = null;
    private IList<AccordionMetaData> AccordionMetaDataList { get; set; }
    private string _resultXml = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    #endregion 

    public MyaAccordionMetaDataResponseData(string metaDataXml)
    {
      _resultXml = metaDataXml;
      AccordionMetaDataList = DeserializeAccordionMetaData(metaDataXml);
    }

    public MyaAccordionMetaDataResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MyaAccordionMetaDataResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "MyaAccordionMetaDataResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region Public Methods
    public IList<AccordionMetaData> GetMyAccordions(List<string> namespaces)
    {
      List<AccordionMetaData> allAccordions = new List<AccordionMetaData>(AccordionMetaDataList);
      List<AccordionMetaData> ownedAccordions = new List<AccordionMetaData>();

      foreach (string nameSpace in namespaces)
      {
        AccordionMetaData accordion = allAccordions.Find(new Predicate<AccordionMetaData>(ad => ad.Namespaces.Contains(nameSpace)));
        if (accordion != null)
        {
          ownedAccordions.Add(accordion);
        }
      }

      return SortedAndFilteredAccordionMetaDataList(ownedAccordions);
    }

    public AccordionMetaData GetAccordionById(int accordionId)
    {
      List<AccordionMetaData> allAccordions = new List<AccordionMetaData>(AccordionMetaDataList);
      return allAccordions.Find(new Predicate<AccordionMetaData>(ad => ad.AccordionId.Equals(accordionId)));
    }
    #endregion

    #region Private Methods

    #region Deserialization
    private IList<AccordionMetaData> DeserializeAccordionMetaData(string metaDataXml)
    {
      var accordions = new List<AccordionMetaData>();

      if (!string.IsNullOrWhiteSpace(metaDataXml))
      {
        XDocument xDoc = XDocument.Parse(metaDataXml);
        accordions = (from accordion in xDoc.Element("data").Elements()
                      select new AccordionMetaData()
                      {        
                        AccordionId = Convert.ToInt32(accordion.Attribute("accordionId").Value),
                        AccordionTitle = accordion.Attribute("accordionTitle").Value,
                        CiExpansion = accordion.Attribute("ciExpansion").Value,
                        CiRenewNow = accordion.Attribute("ciRenewNow").Value,
                        CiSetup = accordion.Attribute("ciSetup").Value,
                        ContentXml = accordion.Attribute("contentXml").Value,
                        ControlPanelXml = accordion.Attribute("controlPanelXml").Value,
                        ControlPanelRequiresAccount = string.Compare(accordion.Attribute("controlPanelRequiresAccount").Value, "1") == 0,
                        DefaultSortOrder = Convert.ToInt32(accordion.Attribute("defaultSortOrder").Value),
                        IconnCssCoordinates = SetCoordinates(accordion.Attribute("iconcsscoordinate").Value),
                        IsProductOfferedFree = string.Compare(accordion.Attribute("isProductOfferedFree").Value, "1") == 0,
                        Namespaces = Convert.ToString(accordion.Attribute("namespaces").Value).ToLowerInvariant().Replace(" ", "").Split(',').ToList<string>(),
                        ShowSetupForManagerOnly = string.Compare(accordion.Attribute("showSetupForManagerOnly").Value, "1") == 0,
                        WorkspaceLoginXml = accordion.Attribute("workspaceLoginXml").Value
                      }
                    ).ToList<AccordionMetaData>();
      }

      return accordions;
    }

    private AccordionMetaData.CssSpriteCoordinate SetCoordinates(string iconnCssCoordinates)
    {
      string[] coordintatePoints = iconnCssCoordinates.Split(',');

      try
      {
        return new AccordionMetaData.CssSpriteCoordinate(coordintatePoints[0], coordintatePoints[1], coordintatePoints[2], coordintatePoints[3]);
      }
      catch
      {
        return new AccordionMetaData.CssSpriteCoordinate("0px", "0px", "0px", "0px");
      }
    }

    #endregion

    #region Sorting & Filtering
    private IList<AccordionMetaData> SortedAndFilteredAccordionMetaDataList(List<AccordionMetaData> ownedAccordions)
    {
      List<int> sortedAccordionIds = new List<int>();
      IEnumerable<AccordionMetaData> filteredOwnedAccordionData;

      ownedAccordions.Sort(SortBySortOrder);

      filteredOwnedAccordionData = ownedAccordions.Distinct(new AccordionFilter());

      return filteredOwnedAccordionData.ToList();
    }

    private static int SortBySortOrder(AccordionMetaData a, AccordionMetaData b)
    {
      int retval = 0;
      if (a != null && b != null)
      {
        retval = a.DefaultSortOrder.CompareTo(b.DefaultSortOrder);
      }

      return retval;
    }

    private class AccordionFilter : IEqualityComparer<AccordionMetaData>
    {
      public bool Equals(AccordionMetaData a, AccordionMetaData b)
      {
        if (Object.ReferenceEquals(a, b))
        {
          return true;
        }
        if (Object.ReferenceEquals(a, null) || Object.ReferenceEquals(b, null))
        {
          return false;
        }

        return a.AccordionId.Equals(b.AccordionId);
      }

      public int GetHashCode(AccordionMetaData a)
      {
        if (Object.ReferenceEquals(a, null))
        {
          return 0;
        }

        return a.AccordionId.GetHashCode();
      }
    }
    #endregion
    
    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
