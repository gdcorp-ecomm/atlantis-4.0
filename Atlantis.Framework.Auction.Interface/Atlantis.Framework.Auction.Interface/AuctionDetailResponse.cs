using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{
  [DataContract]
  public class AuctionDetailResponse
  {
    [DataMember]
    public bool IsDetailValid { get; set; }

    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public AuctionDetail AuctionDetail { get; set; }

    public AuctionDetailResponse() { }

    public AuctionDetailResponse(bool isValid, string errorMessage)
    {
      IsDetailValid = isValid;
      ErrorMessage = errorMessage;
      AuctionDetail = null;
    }

    public AuctionDetailResponse(bool isValid, string errorMessage, AuctionDetail auctiondetail)
    {
      IsDetailValid = isValid;
      ErrorMessage = errorMessage;
      AuctionDetail = auctiondetail;
    }
  }
}
