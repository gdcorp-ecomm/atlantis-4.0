using System.Collections.Generic;
using Atlantis.Framework.WSTService.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.WSTService.Tests
{
  /// <summary>
  /// Summary description for WSTServiceTests
  /// </summary>
  [TestClass]
  public class WSTServiceTests
  {
    [TestMethod]
    public void GetAllWSTCategories()
    {
      WSTCategoryInfoRequestData request = new WSTCategoryInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      WSTCategoryInfoResponseData response = (WSTCategoryInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 578);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.Categories.Count > 0);
    }
    [TestMethod]
    public void GetActiveWSTThemesByCategory()
    {
      WSTTemplateInfoRequestData request = new WSTTemplateInfoRequestData(36, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      WSTTemplateInfoResponseData response = (WSTTemplateInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 577);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.Templates.Count > 0);
    }
    [TestMethod]
    public void GetAllActiveWSTThemes()
    {
      int templateCount1 = 0;
      IDictionary<int, IList<WSTTemplateInfo>> allTemplates1 = new Dictionary<int, IList<WSTTemplateInfo>>();
      WSTCategoryInfoRequestData request = new WSTCategoryInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      WSTCategoryInfoResponseData response = (WSTCategoryInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 578);
      foreach (int categoryId in response.Categories.Keys)
      {
        WSTTemplateInfoRequestData request1 = new WSTTemplateInfoRequestData(categoryId, string.Empty, string.Empty, string.Empty, string.Empty, 0);
        WSTTemplateInfoResponseData response1 = (WSTTemplateInfoResponseData)DataCache.DataCache.GetProcessRequest(request1, 577);
        if (response1.Templates != null)
        {
          templateCount1 += response1.Templates.Count;
          allTemplates1.Add(categoryId, response1.Templates);
        }
      }
      Assert.AreEqual(response.Categories.Keys.Count, allTemplates1.Keys.Count);

      int templateCount2 = 0;
      WSTTemplateInfoRequestData request2 = new WSTTemplateInfoRequestData(0, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      WSTTemplateInfoResponseData response2 = (WSTTemplateInfoResponseData)DataCache.DataCache.GetProcessRequest(request2, 577);
      if (response2.Templates != null)
      {
        templateCount2 = response2.Templates.Count;
      }
      Assert.AreEqual(templateCount1, templateCount2);

      int templateCount3 = 0;
      WSTTemplateInfoRequestData request3 = new WSTTemplateInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      WSTTemplateInfoResponseData response3 = (WSTTemplateInfoResponseData)DataCache.DataCache.GetProcessRequest(request3, 577);
      if (response3.Templates != null)
      {
        templateCount3 = response3.Templates.Count;
      }
      Assert.AreEqual(templateCount1, templateCount3);
    }
  }
}
