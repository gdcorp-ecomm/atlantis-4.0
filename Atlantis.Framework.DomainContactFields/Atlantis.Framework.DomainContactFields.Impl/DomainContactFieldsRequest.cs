using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DomainContactFields.Impl.Data;

namespace Atlantis.Framework.DomainContactFields.Impl
{
  public class DomainContactFieldsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DomainContactFieldsResponseData responseData = null;
      var request = requestData as DomainContactFieldsRequestData;

      try 
      {
        if (request != null)
        {
          string fieldsXml = DomainContactData.DomainContactFieldsXml;

          if (!string.IsNullOrEmpty(fieldsXml))
            responseData = new DomainContactFieldsResponseData(fieldsXml);
        }
      }
      catch (Exception ex)
      {
        responseData = new DomainContactFieldsResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
