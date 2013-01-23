using System.Diagnostics;
using Atlantis.Framework.AuthRetrieve.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthRetrieve.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AuthRetrieve.Impl.dll")]
  public class AuthRetrieveTest
  {
    [TestMethod]    
    public void AuthRetrieveDefaultTest()
    {
      const string shopperid = "871984";
      const int requestType = 533;

      var request = new AuthRetrieveRequestData(shopperid
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "GDCARTNET-G1DWCARTWEB001"
        , "WRSzDbPuVNmvDhadLPojnSPTJMFNSfXH");
      var response = (AuthRetrieveResponseData)Engine.Engine.ProcessRequest(request, requestType);
      /* Sample artifact call
    https://cart.test.godaddy-com.ide/sso/redirectlogin.aspx?artifact=wbcaaNQKYaJCcdVzwNxCoLnZnNAeBTnj&transferCart=true&shopper_id_old=75866&page=Basket
       * */
      // Cache call
      //AuthRetrieveResponseData response = (AuthRetrieveResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //https://cart.dev.godaddy-com.ide/Basket.aspx
      //https://cart.dev.godaddy-com.ide/sso/redirectlogin.aspx?artifact=WRSzDbPuVNmvDhadLPojnSPTJMFNSfXH&transferCart=true&shopper_id_old=871984&page=Basket
      //
      // TODO: Add test logic here
      //

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
