using System;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Atlantis.Framework.SessionCache.Tests
{
  public class TestTripletRequestData: RequestData
  {
    int _testNumber = 0;
    public TestTripletRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int testNumber)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _testNumber = testNumber;
    }

    public int TestNumber
    {
      get { return _testNumber; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_testNumber.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }

  public class TestTripletResponseData : IResponseData, ISessionSerializableResponse
  {
    private string _result = null;
    private DateTime _resultTime;
    private AtlantisException _exception;

    public TestTripletResponseData() { }

    public TestTripletResponseData(string result)
    {
      _result = result;
      _resultTime = DateTime.Now;
    }

    public TestTripletResponseData(RequestData request, Exception ex)
    {
      _exception = new AtlantisException(request, "TestTripletResponseData", ex.Message, ex.StackTrace, ex);
    }

    public string Result
    {
      get { return _result; }
    }

    public DateTime ResultTime
    {
      get { return _resultTime; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      XDocument xmlDoc = new XDocument();
      XElement root = new XElement("TestTripletResponse");
      XAttribute attResult = new XAttribute("result", _result);
      XAttribute attResultTime = new XAttribute("resultTime", _resultTime);
      root.Add(attResult);
      root.Add(attResultTime);
      xmlDoc.Add(root);

      return xmlDoc.ToString(SaveOptions.DisableFormatting);
    }

    public void DeserializeSessionData(string sessionData)
    {
      XDocument xmlDoc = XDocument.Parse(sessionData);
      XElement responseData = xmlDoc.Element("TestTripletResponse");

      _result = responseData.Attribute("result").Value;
      _resultTime = Convert.ToDateTime(responseData.Attribute("resultTime").Value);
    }

    #endregion
  }

  public class TestTripletRequest : IRequest
  {
    private static int _timesCalledTestNumber2 = 0;

    public static void Reset()
    {
      _timesCalledTestNumber2 = 0;
    }

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      TestTripletRequestData ttRequest = (TestTripletRequestData)oRequestData;
      if (ttRequest.TestNumber == 2)
      {
        _timesCalledTestNumber2++;
      }

      if (_timesCalledTestNumber2 > 1)
      {
        throw new Exception("You have called this too many times!");
      }

      TestTripletResponseData response = new TestTripletResponseData(Guid.NewGuid().ToString());
      return response;
    }

    #endregion
  }


}
