using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class PagingOptions
  {
    private int _rowsPerPage = -1;
    [DataMember]
    public int RowsPerPage
    {
      get { return _rowsPerPage; }
      set { _rowsPerPage = value; }
    }

    [DataMember]
    public int PageNumber { get; set; }

    [DataMember]
    public string SortColumn { get; set; }

    [DataMember]
    public string SortOrder { get; set; }
  }
}
