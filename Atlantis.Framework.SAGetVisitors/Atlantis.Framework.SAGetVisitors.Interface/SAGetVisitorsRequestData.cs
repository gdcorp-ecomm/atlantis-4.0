using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SAGetVisitors.Interface
{
  public class SAGetVisitorsRequestData : RequestData 
  {
    public SAGetVisitorsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string domain, DateTime startDate, DateTime endDate) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Domain = domain;
      StartDate = startDate;
      EndDate = endDate;
    }
    
    public string Domain { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      string guts = string.Concat(Domain, ShopperID, StartDate, EndDate);
      byte[] data = Encoding.UTF8.GetBytes(guts);

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;


    }

    #endregion
  }
}
