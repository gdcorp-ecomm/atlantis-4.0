using System;
using System.Configuration;
using System.Web;
using System.Web.Util;

namespace Atlantis.Framework.WebSecurity
{
  /// <summary>
  /// Custom validate pages in the web config if found; otherwise, let the base validate the request.
  /// </summary>
  public class CustomRequestValidator : RequestValidator
  {
    protected override bool IsValidRequestString(HttpContext context, string value,
        RequestValidationSource requestValidationSource, string collectionKey,
        out int validationFailureIndex)
    {
      validationFailureIndex = -1;  

      bool? isValid = null;

      // Custom validate only form content requests.
      if (requestValidationSource == RequestValidationSource.Form)
      {
        // Get the section with all the pages to validate.
        var pageSection = (RequestValidationPageSection) ConfigurationManager.GetSection("atlantis/security");
        if (pageSection != null)
        {
          for (int i = 0; i < pageSection.Pages.Count; i++)
          {
            var contextRelativePath = context.Request.Url.AbsolutePath;
            var pageSectionRelativePath = pageSection.Pages[i].RelativePath;

            // If config relative path and input name matches the request, skip the validation.
            if (!string.IsNullOrEmpty(pageSectionRelativePath) && contextRelativePath.Equals(pageSectionRelativePath, StringComparison.OrdinalIgnoreCase) && pageSection.Pages[i].Name == collectionKey)
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