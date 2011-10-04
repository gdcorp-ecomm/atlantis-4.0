using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetAreaBySectionXml.Interface
{

  /// <summary>
  /// Request Data class for AuctionGetAreaBySectionXml
  /// </summary>
  public class AuctionGetAreaBySectionXmlRequestData : RequestData
  {
    /// <summary>
    /// built request xml
    /// </summary>
    private string _requestXml = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionGetAreaBySectionXmlRequestData"/> class.
    /// </summary>
    /// <param name="includeBidDetail">if set to <c>true</c> [include bid detail]. False recommended for Mobile Applications</param>
    /// <param name="memberAreas">List of the requested member areas.</param>
    /// <param name="minDate">The min date of data to display.</param>
    /// <param name="maxDate">The max date of data to display.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowsPerPage">The rows per page.</param>
    /// <param name="sortColumn">The sort column to use.</param>
    /// <param name="sortOrder">The sort order (Asc or Desc)</param>
    /// <param name="shopperId">The shopper id .</param>
    /// <param name="sourceUrl">The source URL.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="pathway">The pathway.</param>
    /// <param name="pageCount">The page count.</param>
    public AuctionGetAreaBySectionXmlRequestData(bool includeBidDetail, List<string> memberAreas, DateTime? minDate, DateTime? maxDate, int pageNumber, int rowsPerPage,
                                                  string sortColumn, SortOrder sortOrder, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      IncludeBidDetail = includeBidDetail;
      MemberAreas = memberAreas;
      MinDate = minDate ?? DateTime.Now.AddDays(-90);
      MaxDate = maxDate ?? DateTime.Now;
      PageNumber = pageNumber;
      RowsPerPage = rowsPerPage;
      SortColumn = sortColumn;
      SortOrder = sortOrder;
    	RequestTimeout = _requestTimeout;
      BuildRequestXml(memberAreas, out _requestXml);
    }

    /// <summary>
    /// Returns Bid History Data
    /// Suggest false for the Phone Apps. Use GetBidHistory instead.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [include bid detail]; otherwise, <c>false</c>.
    /// </value>
    public bool IncludeBidDetail { get; set; }

    /// <summary>
    /// Gets or sets the member areas.
    /// </summary>
    /// <value>
    /// The member areas.
    /// </value>
    public List<string> MemberAreas { get; set; }

    /// <summary>
    /// Gets or sets the min date.
    /// </summary>
    /// <value>
    /// The min date.
    /// </value>
    public DateTime MinDate { get; set; }

    /// <summary>
    /// Gets or sets the max date.
    /// </summary>
    /// <value>
    /// The max date.
    /// </value>
    public DateTime MaxDate { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    /// <value>
    /// The page number.
    /// </value>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the rows per page.
    /// </summary>
    /// <value>
    /// The rows per page.
    /// </value>
    public int RowsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the sort column.
    /// </summary>
    /// <value>
    /// The sort column.
    /// </value>
    public string SortColumn { get; set; }

    /// <summary>
    /// Gets or sets the sort order.
    /// </summary>
    /// <value>
    /// The sort order.
    /// </value>
    public SortOrder SortOrder { get; set; }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Builds the request XML.
    /// </summary>
    /// <param name="requestedAreas">The requested areas.</param>
    /// <param name="requestXml">The request XML.</param>
    /// <returns> boolean indicating success or failure.</returns>
    protected bool BuildRequestXml(List<string> requestedAreas, out string requestXml)
    {
      requestXml = string.Empty;

      if (requestedAreas != null)
      {
        var sb = new StringBuilder();

        sb.AppendFormat("<MemberAreaRequest ShopperId=\"{0}\">", ShopperID);
        if (requestedAreas.Count > 0)
        {
          sb.Append("<MemberAreas>");
          foreach (var requestedArea in requestedAreas)
          {
            sb.AppendFormat("<MemberArea Area=\"{0}\">", requestedArea.ToLowerInvariant());
            sb.AppendFormat("<IncludeBidDetail>{0}</IncludeBidDetail>", IncludeBidDetail);
            sb.AppendFormat("<MinDate>{0}</MinDate>", MinDate.ToShortDateString());
            sb.AppendFormat("<MaxDate>{0}</MaxDate>", MaxDate.ToShortDateString());
            sb.AppendFormat("<PageNumber>{0}</PageNumber>", PageNumber);
            sb.AppendFormat("<RowsPerPage>{0}</RowsPerPage>", RowsPerPage);
            sb.AppendFormat("<SortCol>{0}</SortCol>", SortColumn.ToLowerInvariant());
            sb.AppendFormat("<SortDir>{0}</SortDir>", SortOrder.ToString().ToUpperInvariant());

            sb.Append("</MemberArea>");
          }

          sb.Append("</MemberAreas>");
        }
        sb.Append("</MemberAreaRequest>");
        requestXml = _requestXml = sb.ToString();
      }

      return true;
    }

    /// <summary>
    /// returns the XML formatted request.
    /// </summary>
    /// <returns></returns>
    public override string ToXML()
    {
      return _requestXml;
    }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionGetAreaBySectionXml is not a cacheable request.");
    }

    #endregion
  }
}
