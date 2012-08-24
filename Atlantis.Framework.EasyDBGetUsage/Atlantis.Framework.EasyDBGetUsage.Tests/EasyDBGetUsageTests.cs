using System;
using System.Diagnostics;
using Atlantis.Framework.EasyDBGetUsage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EasyDBGetUsage.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class EasyDBGetUsageTests
  {
    private const string _shopperId = "858346";
    private const int _privateLabelId = 1;
    private const int _requestType = 294;

    public EasyDBGetUsageTests()
    { }

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void TestMethod1()
    {
      EasyDBGetUsageRequestData request = new EasyDBGetUsageRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _privateLabelId
         , "0766fd0e-1f6c-11e0-9353-0050569575d8");

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      EasyDBGetUsageResponseData responseData;
      try
      {
        responseData = (EasyDBGetUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);

        Debug.WriteLine(string.Format("Used Disk Space: {0} {1}", responseData.UsedDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Disk Space: {0} {1}", responseData.TotalDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Used Bandwidth: {0} {1}", responseData.UsedBandwidth, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Bandwidth: {0} {1}", responseData.TotalBandwidth, responseData.MeasurementUnit));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod2()
    {
      EasyDBGetUsageRequestData request = new EasyDBGetUsageRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _privateLabelId
         , "0766fd0e-1f6c-11e0-9353-0050569575d8");

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      EasyDBGetUsageResponseData responseData;
      try
      {
        responseData = (EasyDBGetUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);

        Debug.WriteLine(string.Format("Used Disk Space: {0} {1}", responseData.UsedDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Disk Space: {0} {1}", responseData.TotalDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Used Bandwidth: {0} {1}", responseData.UsedBandwidth, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Bandwidth: {0} {1}", responseData.TotalBandwidth, responseData.MeasurementUnit));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod3()
    {
      EasyDBGetUsageRequestData request = new EasyDBGetUsageRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _privateLabelId
         , "0766fd0e-1f6c-11e0-9353-0050569575d8");

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      EasyDBGetUsageResponseData responseData;
      try
      {
        responseData = (EasyDBGetUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);

        Debug.WriteLine(string.Format("Used Disk Space: {0} {1}", responseData.UsedDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Disk Space: {0} {1}", responseData.TotalDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Used Bandwidth: {0} {1}", responseData.UsedBandwidth, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Bandwidth: {0} {1}", responseData.TotalBandwidth, responseData.MeasurementUnit));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod4()
    {
      EasyDBGetUsageRequestData request = new EasyDBGetUsageRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _privateLabelId
         , "0766fd0e-1f6c-11e0-9353-0050569575d8");

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      EasyDBGetUsageResponseData responseData;
      try
      {
        responseData = (EasyDBGetUsageResponseData)Engine.Engine.ProcessRequest(request, _requestType);

        Debug.WriteLine(string.Format("Used Disk Space: {0} {1}", responseData.UsedDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Disk Space: {0} {1}", responseData.TotalDiskSpace, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Used Bandwidth: {0} {1}", responseData.UsedBandwidth, responseData.MeasurementUnit));
        Debug.WriteLine(string.Format("Total Bandwidth: {0} {1}", responseData.TotalBandwidth, responseData.MeasurementUnit));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        Assert.Fail(ex.Message);
      }
    }
  }
}
