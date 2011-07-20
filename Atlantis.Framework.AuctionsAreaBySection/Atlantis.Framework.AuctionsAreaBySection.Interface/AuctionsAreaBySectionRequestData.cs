using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsAreaBySection.Interface
{
  public class AuctionsAreaBySectionRequestData : RequestData
  {
    #region Properties

    private string _membersAreaID;
    public string MembersAreaID
    {
      get { return _membersAreaID; }
      set { _membersAreaID = value; }
    }

    private string _returnBids;
    public string ReturnBids
    {
      get { return _returnBids; }
      set { _returnBids = value; }

    }
    #endregion

    public AuctionsAreaBySectionRequestData(
      string shopperID, string sourceURL, string orderID, string pathway, 
      int pageCount, string membersAreaID, string returnBids)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _membersAreaID = membersAreaID;
      _returnBids = returnBids;
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    #region RequestData Members
    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}-{2}", ShopperID, MembersAreaID, ReturnBids));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
    #endregion
  }
}
