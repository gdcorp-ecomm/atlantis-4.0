using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Util;

namespace Atlantis.Framework.WebSecurity
{
  public class CustomRequestValidator : RequestValidator
  {
    protected override bool IsValidRequestString(HttpContext context, string value,
        RequestValidationSource requestValidationSource, string collectionKey,
        out int validationFailureIndex)
    {
      validationFailureIndex = -1;  

      bool? isValid = null;

      if (requestValidationSource == RequestValidationSource.Form)
      {
        var pageSection = (RequestValidationPageSection) ConfigurationManager.GetSection("atlantis/security");
        if (pageSection != null)
        {
          for (int i = 0; i < pageSection.Pages.Count; i++)
          {
            var currentPage = Path.GetFileName(context.Request.PhysicalPath) ?? string.Empty;
            var path = pageSection.Pages[i].Path;

            if (!string.IsNullOrEmpty(path) && currentPage.Equals(path, StringComparison.OrdinalIgnoreCase) && pageSection.Pages[i].Name == collectionKey)
            {
              isValid = true;
              break;
            }
          }
        }
      }

      if (isValid == null)
      {
        isValid = base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
      }

      return isValid.Value;
    }
  }
}