using System.IO;

using Atlantis.Framework.GetAccountXML.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Tests
{
  public class MockGetAccountXmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement config)
    {
      var requestData = (GetAccountXMLRequestData)oRequestData;
      var responseData = new GetAccountXMLResponseData {AccountXML = GetAccountXmlData(requestData.ResourceID)};
      return responseData;
    }

    private static string GetAccountXmlData(string filename)
    {
      using (var reader = new StreamReader(filename + ".txt"))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
