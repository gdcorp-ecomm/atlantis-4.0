
namespace Atlantis.Framework.Providers.CDSContent
{
  public static class CdsContentProviderGlobalSettings
  {
    #region Constructor

    static CdsContentProviderGlobalSettings()
    {
      AllowEmptyContent = false;
    }

    #endregion //Constructor



    #region Public properties

    public static bool AllowEmptyContent { get; set; }

    #endregion //Public properties
  }
}
