using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsSEORequestData : RequestData
    {
      public string BVProductId { get; set; }
      public string DisplayCode { get; set; }
      public int PageNumber { get; set; }
      public string APIKey { get; set; }

      public ReviewsSEORequestData(int pageNumber, string bvProductId, string displayCode = null, string apiKey = null)        
      {
        PageNumber = pageNumber;
        BVProductId = bvProductId;
        DisplayCode = displayCode ?? string.Empty;
        APIKey = apiKey ?? string.Empty;
      }      
      
      public override string GetCacheMD5()
      {
        return BuildHashFromStrings(BVProductId, DisplayCode, PageNumber.ToString(), APIKey ?? string.Empty);
      }
    }
}
