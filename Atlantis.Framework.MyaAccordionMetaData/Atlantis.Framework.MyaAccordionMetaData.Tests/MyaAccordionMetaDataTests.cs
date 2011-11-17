using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaAccordionMetaData.Tests
{
	[TestClass]
	public class GetMyaAccordionMetaDataTests
	{
		private const int _requestType = 428;
	
	
		public GetMyaAccordionMetaDataTests()
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
		public void MyaAccordionMetaDataTest()
		{
			MyaAccordionMetaDataRequestData request = new MyaAccordionMetaDataRequestData("856907"
				, string.Empty
				, string.Empty
				, string.Empty
				, 0 );

			MyaAccordionMetaDataResponseData response = (MyaAccordionMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);


			Assert.IsTrue(response.IsSuccess);
			if (response.IsSuccess)
			{

				foreach (AccordionMetaData accordion in response.AccordionMetaDataItems)
				{
					Debug.WriteLine("");
					Debug.WriteLine(string.Format("********************** GET ACCORDION BY ID ({0}) **********************", accordion.AccordionTitle));
					PropertyInfo[] properties = accordion.GetType().GetProperties();
					foreach (PropertyInfo p in properties)
					{
						switch (p.Name)
						{
							case "Namespaces":
								string list = string.Empty;
								HashSet<string> nss = p.GetValue(accordion, null) as HashSet<string>;
								foreach (string ns in nss)
								{
									list = list + string.Format("{0},", ns);
								}
								list.TrimEnd(',');
								Debug.WriteLine(string.Format("{0}: {1}", p.Name, list));
								break;
							case "IconCssCoordinates":
								AccordionMetaData.CssSpriteCoordinate coords = p.GetValue(accordion, null) as AccordionMetaData.CssSpriteCoordinate;
								Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("X:{0},Y:{1},Width:{2},Height:{3}", coords.X, coords.Y, coords.Width, coords.Height)));
								break;
							case "Content":
								AccordionMetaData.ContentData inner = p.GetValue(accordion, null) as AccordionMetaData.ContentData;
								Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("AccountList: {0} | JsonPage: {1} | CiOptions: {2}", inner.AccountList, inner.JsonPage, inner.CiOptions)));
								if (inner.ShowBuyLink)
								{
									StringBuilder sb = new StringBuilder();
									Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2}", inner.LinkUrl.Link, inner.LinkUrl.Page, inner.LinkUrl.Type)));
									foreach (string key in inner.LinkUrl.QsKeys)
									{
										sb.Append(string.Format("{0}: {1}", key, inner.LinkUrl.QsKeys[key]));
									}
									if (sb.Length > 0)
									{
										Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
									}
								}
								break;
							case "WorkspaceLogin":
								AccordionMetaData.WorkspaceLoginData wsl = p.GetValue(accordion, null) as AccordionMetaData.WorkspaceLoginData;
								if (wsl.HasLink)
								{
									StringBuilder sb = new StringBuilder();
									Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2}", wsl.LinkUrl.Link, wsl.LinkUrl.Page, wsl.LinkUrl.Type)));
									foreach (string key in wsl.LinkUrl.QsKeys)
									{
										sb.Append(string.Format("{0}: {1}", key, wsl.LinkUrl.QsKeys[key]));
									}
									if (sb.Length > 0)
									{
										Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
									}
								}
								else
								{
									Debug.WriteLine(string.Format("{0}: N/A", p.Name));
								}
								break;
							case "ControlPanels":
								AccordionMetaData.ControlPanelData cp = p.GetValue(accordion, null) as AccordionMetaData.ControlPanelData;
								foreach(AccordionMetaData.LinkUrlData link in cp.LinkUrls)
								{
									StringBuilder sb = new StringBuilder();
									Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2} | IdentificationRule: {3} | IdentificationValue: {4}", link.Link, link.Page, link.Type, link.IdentificationRule, link.IdentificationValue)));
									foreach (string key in link.QsKeys)
									{
										sb.Append(string.Format("{0}: {1} | ", key, link.QsKeys[key]));
									}
									if (sb.Length > 0)
									{
										Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString().TrimEnd(' ').TrimEnd('|')));
									}
								}
								if (cp.LinkUrls.Exists(new System.Predicate<AccordionMetaData.LinkUrlData>(url => url.IdentificationRule != string.Empty)))
								{
									foreach (AccordionMetaData.LinkUrlData link in cp.LinkUrls)
									{
										Debug.WriteLine(string.Format("{0} - IdentificationRule: {1} = {2} - HasManagerLink: {3}", p.Name, link.IdentificationRule, link.IdentificationValue, cp.HasManagerLink(link.IdentificationValue)));
									}
								}
								else 
								{
									Debug.WriteLine(string.Format("{0} - HasManagerLink: {1}", p.Name, cp.HasManagerLink(string.Empty)));									
								}
								break;
							case "AccordionTitle":
								Debug.WriteLine(string.Format("{0}: {1}", p.Name, HttpUtility.HtmlDecode(p.GetValue(accordion, null).ToString())));
								break;
							default:
								Debug.WriteLine(string.Format("{0}: {1}", p.Name, p.GetValue(accordion, null)));
								break;
						}
					}
					Debug.WriteLine(string.Format("HasProductList: {0}", accordion.HasProductList()));
					Debug.WriteLine(string.Format("ShowWorkspaceLogin: {0}", accordion.ShowWorkspaceLogin()));
					Debug.WriteLine(string.Format("IsAllInnerXmlValid: {0}", accordion.IsAllInnerXmlValid.HasValue ? "False" : "True"));
				}
				Debug.WriteLine("");
				Debug.WriteLine("Response.IsSuccess: {0}", response.IsSuccess);
				Debug.WriteLine("Response.NoXmlParsingErrors: {0}", response.NoXmlParsingErrors);
				Debug.WriteLine("");
								
				Debug.WriteLine("********************** ToXML() **********************");
				Debug.WriteLine(HttpUtility.HtmlDecode(response.ToXML()));
			}
		}
	}
}
