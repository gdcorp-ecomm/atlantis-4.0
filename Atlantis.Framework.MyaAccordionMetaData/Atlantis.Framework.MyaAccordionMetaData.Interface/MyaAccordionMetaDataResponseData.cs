using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class MyaAccordionMetaDataResponseData : IResponseData
  {
    #region Properties
    private AtlantisException _exception = null;
    private IList<AccordionMetaData> AccordionMetaDataList { get; set; }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    #endregion 

    public MyaAccordionMetaDataResponseData(IList<AccordionMetaData> metaDataList)
    {
      AccordionMetaDataList = metaDataList;
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
    public IList<AccordionMetaData> GetMyAccordionIds(List<string> namespaces)
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
    #endregion

    #region Private Methods
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

        return a.AccordionId == null ? 0 : a.AccordionId.GetHashCode();
      }
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("MyaAccordionMetaDataResponseData does not implement ToXML()");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
