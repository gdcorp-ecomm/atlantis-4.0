using System;
using Atlantis.Framework.Brand.Impl.Data;
using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Impl
{
  public class CompanyNameRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        CompanyNameRequestData companyNameRequest = (CompanyNameRequestData)requestData;
        string companyNamesXml = CompanyNamesData.CompanyNamesXml;

        result = CompanyNameResponseData.FromCompanyNameXml(companyNamesXml, companyNameRequest.ContextId);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "CompanyNameRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = CompanyNameResponseData.FromException(exception);
      }

      return result;
    }
  }
}