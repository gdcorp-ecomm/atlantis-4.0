using Atlantis.Framework.AuthRetrieve.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Sso
{
  public abstract class ClusterSsoProvider : SsoProviderBase
  {
    public ClusterSsoProvider(IProviderContainer container)
      : base(container)
    { }


    public override bool ParseArtifact(string artifact, out string shopperId, out int failureCount)
    {
      bool result = false;
      failureCount = 0;
      shopperId = string.Empty;

      AuthRetrieveResponseData response = null;

      try
      {
        string url = HttpRequestExists ? HttpContext.Current.Request.Url.ToString() : string.Empty;

        AuthRetrieveRequestData authReq = new AuthRetrieveRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, string.Empty, 0, SpKey, artifact);
        response = (AuthRetrieveResponseData)Engine.Engine.ProcessRequest(authReq, SsoProviderEngineRequests.AuthRequestRetrieve);

        if (response.IsSuccess)
        {
          if (!string.IsNullOrEmpty(response.ShopperId))
          {
            shopperId = response.ShopperId;
            result = true;
          }
        }
      }
      catch (Exception ex)
      {
        string responseXml = String.Empty;
        if (response != null)
        {
          responseXml = response.ToXML();
        }

        AtlantisException atlEx = new AtlantisException(ex.Message, string.Format("Artifact:'{0}', ResponseXml: '{1}'", artifact, responseXml), "ClusterSsoProvider::ParseArtifact", string.Empty, SiteContext, null);
        Engine.Engine.LogAtlantisException(atlEx);
      }

      if (!result)
      {
        failureCount = AddFailedLoginTransaction();
      }
      else
      {
        ResetFailedLoginTransaction();
      }

      return result;
    }

    public override bool ParseArtifact(string artifact, out string shopperId)
    {
      int failureCount;
      return ParseArtifact(artifact, out shopperId, out failureCount);
    }
  }
}
