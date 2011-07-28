using System;
using System.Collections.Generic;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging
{
  public class MinimalSummaryOnlyPaging : IDomainPaging
  {
    public string SearchTerm
    {
      get { return null; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int RowsPerPage
    {
      get { return 0; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string SortOrderField
    {
      get { return "domainName"; }
    }

    public SortOrderType SortOrder
    {
      get { return SortOrderType.Ascending; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string BoundaryField
    {
      get { throw new NotImplementedException(); }
    }

    public string BoundaryFieldValue
    {
      get { return null; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string BoundaryUniquifierField
    {
      get { throw new NotImplementedException(); }
    }

    public string BoundaryUniquifierFieldValue
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool IncludeBoundary
    {
      get { return false; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool NavigatingForward
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int? ExpirationDays
    {
      get { return null; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int? StatusType
    {
      get { return null; }
      set
      {
        throw new NotImplementedException();
      }
    }

    public List<int> TldIdList
    {
      get { return new List<int>(0); }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool SummaryOnly
    {
      get
      {
        return true;
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
