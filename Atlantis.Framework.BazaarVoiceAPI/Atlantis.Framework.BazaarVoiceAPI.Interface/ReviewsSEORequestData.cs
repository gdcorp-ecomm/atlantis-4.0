using Atlantis.Framework.Interface;
using System.Collections.Specialized;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsSEORequestData : RequestData
  {
    public string BazaarVoiceProductId { get; set; }
    public string DisplayCode { get; set; }
    public int PageNumber { get; set; }
    public string APIKey { get; set; }
       
    public ReviewsSEORequestData(string bazaarVoiceProductId, NameValueCollection queryString = null, int pageNumber = 1, string displayCode = null, string apiKey = null)
    {

      if (queryString == null)
      {       
        PageNumber = (pageNumber < 1) ? 1 : pageNumber;
        BazaarVoiceProductId = bazaarVoiceProductId;
        DisplayCode = displayCode ?? string.Empty;
        APIKey = apiKey ?? string.Empty;
      }
      else if (string.IsNullOrEmpty(queryString["bvrrp"]))
      {
        PageNumber = (pageNumber < 1) ? 1 : pageNumber;
        BazaarVoiceProductId = bazaarVoiceProductId;
        DisplayCode = displayCode ?? string.Empty;
        APIKey = apiKey ?? string.Empty;
      } 
      else
      {
        string[] urlValues = queryString["bvrrp"].Split('/');
        try
        {
          DisplayCode = urlValues[0];
          APIKey = apiKey ?? string.Empty;
          BazaarVoiceProductId = bazaarVoiceProductId;
          
          bool isPageNumberInt = int.TryParse(urlValues[3], out pageNumber);
          if (isPageNumberInt)
          {
            PageNumber = pageNumber;
          }
          else
          {
            PageNumber = 1;            
          }          
        }
        catch 
        {
          PageNumber = (pageNumber < 1) ? 1 : pageNumber;
          BazaarVoiceProductId = bazaarVoiceProductId;
          DisplayCode = displayCode ?? string.Empty;
          APIKey = apiKey ?? string.Empty;
        }
      }
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(BazaarVoiceProductId, DisplayCode, PageNumber.ToString(), APIKey ?? string.Empty);
    }
  }
}
