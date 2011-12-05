using System;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.GuestbookSurveyResults.Interface
{
  public class GuestbookSurveyResultsRequestData : RequestData
  {
    public TimeSpan RequestTimeout { get; set; }
    public int SurveyId { get; private set; }

    public GuestbookSurveyResultsRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int surveyId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(8);
      SurveyId = surveyId;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(SurveyId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
