using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetSurveyTakenInfo.Interface
{
  public class GetSurveyTakenInfoRequestData : RequestData
  {
    public int SurveyId { get; set; }

    public GetSurveyTakenInfoRequestData(string shopperId
                                          , string sourceUrl
                                          , string orderId
                                          , string pathway
                                          , int pageCount
                                          , int surveyId)
      : base(shopperId, sourceUrl, orderId, pathway, 0)
    {
      SurveyId = surveyId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}", ShopperID, SurveyId));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
