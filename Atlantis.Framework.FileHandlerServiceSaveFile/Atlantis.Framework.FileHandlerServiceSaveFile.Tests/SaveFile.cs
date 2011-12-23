using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.FileHandlerServiceSaveFile.Interface;

namespace Atlantis.Framework.FileHandlerServiceSaveFile.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class SaveFile
  {
    public SaveFile()
    {
      //
      // TODO: Add constructor logic here
      //
    }

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
    public void SaveFileTest()
    {
      FileHandlerServiceSaveFileRequestData request = new FileHandlerServiceSaveFileRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0);

      FileInfo file = new FileInfo(@"c:\test.txt");
      FileStream stream = file.Open(FileMode.Open);

//      FileStream stream = new FileStream(@"c:\test.txt", FileMode.Open);
      

      string blobFormat = "<VideoReviewFileBlob><attachmentID id=\"{0}\" /><videoID id=\"{1}\" /><fileName name=\"{2}\" /></VideoReviewFileBlob>";
      string applicationData = String.Format(blobFormat, 1, 34, "test.txt");
      request.ApplicationData = applicationData;
      request.ApplicationKey = "1090";
      request.FileNameOnly = "test.txt";
      request.SettingId = 1;
      request.SubscriberId = 2;
      request.Stream = stream;
      request.Environment = "DEV";

      FileHandlerServiceSaveFileResponseData response = (FileHandlerServiceSaveFileResponseData)Engine.Engine.ProcessRequest(request, 219);
      string responseText = response.OutputMessage;
      stream.Close();

      Assert.IsTrue(response.Success);

    }
  }
}
