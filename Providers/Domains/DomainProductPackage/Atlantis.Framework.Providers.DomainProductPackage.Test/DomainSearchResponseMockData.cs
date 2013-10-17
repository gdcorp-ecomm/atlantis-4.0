using Atlantis.Framework.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage.Test
{
  public class DomainSearchResponseMockData
  {
    public static DomainSearchResponseData GetDomainSearchResponseTestData()
    {
      const string searchResult = "{\"ExactDomains\":[{\"Extension\":\"mx\",\"DomainName\":\"iowa.mx\",\"NameWithoutExtension\":\"iowa\",\"PunyCodeExtension\":\"\",\"PunyCodeName\":\"\",\"PunyCodeNameWithoutExtension\":\"\",\"Keys\":[],\"Data\":[{\"Name\":\"isavailable\",\"Data\":\"true\"},{\"Name\":\"availcheckstatus\",\"Data\":\"full\"},{\"Name\":\"isvaliddomain\",\"Data\":\"true\"},{\"Name\":\"database\",\"Data\":\"similar\"},{\"Name\":\"hasleafpage\",\"Data\":\"false\"}]}]}";

      var response = (DomainSearchResponseData)DomainSearchResponseData.ParseRawResponse(searchResult);

      return response;
    }
  }
}

//"{"ExactDomains":[{"Extension":"mx","DomainName":"iowa.mx","NameWithoutExtension":"iowa","PunyCodeExtension":"","PunyCodeName":"","PunyCodeNameWithoutExtension":"","Keys":[],"Data":[{"Name":"isavailable","Data":"true"},{"Name":"availcheckstatus","Data":"full"},{"Name":"isvaliddomain","Data":"true"},{"Name":"database","Data":"similar"},{"Name":"isoingo","Data":"[{"Name":"isoingo","Data":"10"}]"},{"Name":"idnscript","Data":"[{"Name":"eng","Data":"35"}]"},{"Name":"isidn","Data":"false"},{"Name":"hasleafpage","Data":"false"}]}]}";
