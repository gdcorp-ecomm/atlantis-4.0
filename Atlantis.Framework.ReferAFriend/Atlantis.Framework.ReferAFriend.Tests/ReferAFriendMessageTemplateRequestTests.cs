using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendMessageTemplateRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendMessageTemplateRequest_Success()
		{
			var request = new ReferAFriendMessageTemplateRequestData("", "", "", "", 0);
			var response = (ReferAFriendMessageTemplateResponseData)Engine.Engine.ProcessRequest(request, 2);
			Assert.AreEqual(response.GetTemplateText("UnitTest"), "UnitTest_DoNotTouch");
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
