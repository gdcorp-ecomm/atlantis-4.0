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

    private Dictionary<int, AccordionMetaData> _accordionMetaDataDictionary;
    private List<AccordionMetaData> _accordionMetaDataList;

    private string _resultXml = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public bool NoXmlParsingErrors
    {
      get
      {
        bool isValid = true;
        foreach (AccordionMetaData amd in _accordionMetaDataList)
        {
          if (amd.IsAllInnerXmlValid.HasValue)
          {
            isValid = false;
            break;
          }
        }
        return isValid;
      }
    }

    public IEnumerable<AccordionMetaData> AccordionMetaDataItems
    {
      get { return _accordionMetaDataList; }
    }

    #endregion 

    public MyaAccordionMetaDataResponseData(string metaDataXml)
    {
      _resultXml = metaDataXml;
      DeserializeAccordionMetaData(metaDataXml);
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

    public AccordionMetaData GetAccordionById(int accordionId)
    {
      AccordionMetaData result = null;
      _accordionMetaDataDictionary.TryGetValue(accordionId, out result);
      return result;
    }

    #endregion

    #region Private Methods

    #region Deserialization

    private void DeserializeAccordionMetaData(string metaDataXml)
    {
      _accordionMetaDataList = new List<AccordionMetaData>();
      _accordionMetaDataDictionary = new Dictionary<int, AccordionMetaData>();

      if (!string.IsNullOrWhiteSpace(metaDataXml))
      {
        XDocument xDoc = XDocument.Parse(metaDataXml);
        _accordionMetaDataList = (from accordion in xDoc.Element("data").Elements()
                      select new AccordionMetaData(accordion) {}
                    ).ToList<AccordionMetaData>();
      }

      _accordionMetaDataList.Sort();

      _accordionMetaDataList.ForEach(
        delegate(AccordionMetaData metadata)
        {
          _accordionMetaDataDictionary[metadata.AccordionId] = metadata;
        }
      );
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
