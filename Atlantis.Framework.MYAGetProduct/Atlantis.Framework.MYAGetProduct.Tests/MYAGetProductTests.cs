using System.Diagnostics;
using Atlantis.Framework.MYAGetProduct.Interface;
using Atlantis.Framework.MYAGetProduct.Interface.PageHelper;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;
using Atlantis.Framework.MYAGetProduct.Interface.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MYAGetProduct.Tests
{
  /// <summary>
  /// Summary description for UnitTest
  /// </summary>
  [TestClass]
  public class MYAGetProductTests
  {
    private const string _shopperId = "840820";  //840820 842749; 
    private const int _getProductsListLiteRequestType = 122;
	
    public MYAGetProductTests()
    {}

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MYAGetProductTest()
    {
      int productTypeId = MyaProductTypeId.CustomerManager;
      //int productTypeId = 14;

      PagingInfo pagingInfo = new PagingInfo();
      //pagingInfo.CurrentPage = 2;

      MYAGetProductRequestData request = new MYAGetProductRequestData(_shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        0,
        pagingInfo,
        productTypeId);

      MYAGetProductResponseData response = (MYAGetProductResponseData)Engine.Engine.ProcessRequest(request, _getProductsListLiteRequestType);

      Debug.WriteLine(string.Format("NumberOfPages: {0}", request.PagingInfo.NumberOfPages));
      Debug.WriteLine(string.Format("CurrentPage: {0}", request.PagingInfo.CurrentPage));
      Debug.WriteLine(string.Format("NumberOfRecords: {0}", request.PagingInfo.NumberOfRecords));

      foreach (MyaProduct mp in response.MyaProducts)
      {
        Debug.WriteLine("*************************");
        Debug.WriteLine(string.Format("AutoRenewFlag: {0}", mp.AutoRenewFlag));
        Debug.WriteLine(string.Format("AccountExpirationDate: {0}", mp.AccountExpirationDate));
        Debug.WriteLine(string.Format("ActiveQuota: {0}", mp.ActiveQuota));
        Debug.WriteLine(string.Format("AddOnQuota: {0}", mp.AddOnQuota));
        Debug.WriteLine(string.Format("ApplicationDescription: {0}", mp.ApplicationDescription));
        Debug.WriteLine(string.Format("AuthenticationGUID: {0}", mp.AuthenticationGUID));
        Debug.WriteLine(string.Format("BillingResourceId: {0}", mp.BillingResourceId));
        Debug.WriteLine(string.Format("BundlePfId: {0}", mp.BundlePfId));
        //Debug.WriteLine(string.Format("CanRenewFreeProducts: {0}", mp.CanRenewFreeProducts));
        Debug.WriteLine(string.Format("CommonName: {0}", mp.CommonName));
        Debug.WriteLine(string.Format("CreateDate: {0}", mp.CreateDate));
        Debug.WriteLine(string.Format("CustomerId: {0}", mp.CustomerId));
        Debug.WriteLine(string.Format("CustomPageLayoutPfId: {0}", mp.CustomPageLayoutPfId));
        Debug.WriteLine(string.Format("Credits: {0}", mp.Credits));
        Debug.WriteLine(string.Format("DateSubmitted: {0}", mp.DateSubmitted));
        Debug.WriteLine(string.Format("Description: {0}", mp.Description));
        Debug.WriteLine(string.Format("DesignPfId: {0}", mp.DesignPfId));
        Debug.WriteLine(string.Format("DesignStatus: {0}", mp.DesignStatus));
        Debug.WriteLine(string.Format("EntityId: {0}", mp.EntityId));
        Debug.WriteLine(string.Format("ExpirationDate: {0}", mp.ExpirationDate));
        Debug.WriteLine(string.Format("HasMaintenance: {0}", mp.HasMaintenance));
        Debug.WriteLine(string.Format("IsFree: {0}", mp.IsFree));
        Debug.WriteLine(string.Format("IsPastDue: {0}", mp.IsPastDue));
        Debug.WriteLine(string.Format("LastExpirationDate: {0}", mp.LastExpirationDate));
        Debug.WriteLine(string.Format("MerchantAccountId: {0}", mp.MerchantAccountId));
        Debug.WriteLine(string.Format("Namespace: {0}", mp.Namespace));
        Debug.WriteLine(string.Format("NumberOfPeriods: {0}", mp.NumberOfPeriods));
        Debug.WriteLine(string.Format("ObsoleteResourceId: {0}", mp.ObsoleteResourceId));
        Debug.WriteLine(string.Format("OrderId: {0}", mp.OrderId));
        Debug.WriteLine(string.Format("OrionResourceId: {0}", mp.OrionResourceId));
        Debug.WriteLine(string.Format("ParentBundleId: {0}", mp.ParentBundleId));
        Debug.WriteLine(string.Format("ParentBundleProductTypeId: {0}", mp.ParentBundleProductTypeId));
        Debug.WriteLine(string.Format("PendingQuota: {0}", mp.PendingQuota));
        Debug.WriteLine(string.Format("PpShopperProfileId: {0}", mp.PpShopperProfileId));
        Debug.WriteLine(string.Format("PrivateLabelGroupType: {0}", mp.PrivateLabelGroupType));
        Debug.WriteLine(string.Format("ProductId: {0}", mp.ProductId));
        Debug.WriteLine(string.Format("ProductTypeId: {0}", mp.ProductTypeId));
        Debug.WriteLine(string.Format("RecurringPayment: {0}", mp.RecurringPayment));
        Debug.WriteLine(string.Format("ReferralId: {0}", mp.ReferralId));
        Debug.WriteLine(string.Format("RenewalPfId: {0}", mp.RenewalPfId));
        Debug.WriteLine(string.Format("RowId: {0}", mp.RowId));
        Debug.WriteLine(string.Format("ServiceFeePfId: {0}", mp.ServiceFeePfId));
        Debug.WriteLine(string.Format("StartDate: {0}", mp.StartDate));
        Debug.WriteLine(string.Format("SupportPhone: {0}", mp.SupportPhone));
        Debug.WriteLine(string.Format("UserWebsiteId: {0}", mp.UserWebsiteId));
      }
      Debug.WriteLine("*************************");
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
