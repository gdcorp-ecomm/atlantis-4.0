using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SurveyService.Interface
{
  public class SurveyServiceRequestData : RequestData
  {
    // **************************************************************** //

    public SurveyServiceRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
    }

    public string IPAddress { get; set; }
    public int AdVersion { get; set; }
    public int AgeGroupID { get; set; }
    public int PoliticalID { get; set; }
    public string Answers { get; set; }

    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      throw new Exception("SurveyService is not a cacheable request.");
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
