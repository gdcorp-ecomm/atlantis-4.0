using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ValidateField.Interface;
using Atlantis.Framework.ValidateField.Impl.Data;

namespace Atlantis.Framework.ValidateField.Impl
{
  public class ValidateFieldRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ValidateFieldResponseData result;
      ValidateFieldRequestData request = requestData as ValidateFieldRequestData;
      string fieldValidationXml;

      if (FieldValidationData.TryGetFieldValidationXml(request.FieldNameKey, out fieldValidationXml))
      {
        result = new ValidateFieldResponseData(fieldValidationXml);
      }
      else
      {
        result = new ValidateFieldResponseData(null);
      }

      return result;
    }

    #endregion
  }
}
