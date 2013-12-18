using Atlantis.Framework.Interface;
using System;
using System.Collections.Specialized;

namespace Atlantis.Framework.Providers.Links
{
  public static class LinkProviderCommonParameters
  {
    private static bool _handleProgId = true;
    /// <summary>
    /// Setting this to false will turn off the LinkProvider's automatic handling of ProgId
    /// </summary>
    public static bool HandleProgId
    {
      get { return _handleProgId; }
      set { _handleProgId = value; }
    }

    private static bool _handlePlId = false;
    /// <summary>
    /// Setting this to false will turn off the LinkProvider's automatic handling of PlId
    /// </summary>
    public static bool HandlePlId
    {
      get { return _handlePlId; }
      set { _handlePlId = value; }
    }

    private static bool _handleISC = true;
    /// <summary>
    /// Setting this to false will turn off the LinkProvider's automatic handling of ISC
    /// </summary>
    public static bool HandleISC
    {
      get { return _handleISC; }
      set { _handleISC = value; }
    }

    private static bool _handleManager = true;
    /// <summary>
    /// Setting this to false will turn off the LinkProvider's automatic handling of manager query string parameters
    /// </summary>
    public static bool HandleManager
    {
      get { return _handleManager; }
      set { _handleManager = value; }
    }

    /// <summary>
    /// Delegate for adding additional common parameter functionality
    /// </summary>
    /// <param name="siteContext">ISiteContext that will be passed from the LinkProvider</param>
    /// <param name="queryMap">NameValueCollection that can be modified to add or remove querystring parameters.</param>
    public delegate void AddCommonParametersHandler(IProviderContainer container, NameValueCollection queryMap);
    
    /// <summary>
    /// Event used to supply a delegate you want called during the CommonParameter processing in LinkProvider
    /// </summary>
    public static event AddCommonParametersHandler AddCommonParameters;

    internal static void OnAddCommonParameters(IProviderContainer container, NameValueCollection queryMap)
    {
      var cachedAddCommonParameters = AddCommonParameters;
      if (cachedAddCommonParameters != null)
      {
        try
        {
          cachedAddCommonParameters(container, queryMap);
        }
        catch (Exception ex)
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          var aex = new AtlantisException("LinkProviderCommonParameters.OnAddCommonParameters", 0, message, String.Empty);
          Engine.Engine.LogAtlantisException(aex);
        }

      }
    }
  }
}
