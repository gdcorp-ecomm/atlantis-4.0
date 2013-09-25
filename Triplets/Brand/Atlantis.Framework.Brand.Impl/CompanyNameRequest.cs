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
      CompanyNameRequestData companyNameRequest = (CompanyNameRequestData)requestData;
      string companyNamesXml = CompanyNamesData.CompanyNamesXml;

      return CompanyNameResponseData.FromCompanyNameXml(companyNamesXml, companyNameRequest.ContextId);
    }
  }
}