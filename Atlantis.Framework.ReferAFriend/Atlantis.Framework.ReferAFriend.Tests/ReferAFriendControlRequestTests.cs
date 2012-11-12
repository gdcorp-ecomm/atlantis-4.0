using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendControlRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendControlRequest_Success()
		{
			var request = new ReferAFriendControlRequestData("", "", "", "", 0);
			var response = (ReferAFriendControlResponseData)Engine.Engine.ProcessRequest(request, 3);
			Assert.AreEqual(response.Get<string>("ItemSourceCodePrefix", null), "wow");
			Assert.IsTrue(response.IsSuccess);
		}

		//[TestMethod]
		//[DeploymentItem("atlantis.config")]
		//[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		//public void ReferAFriendControlRequest_Success()
		//{
		//	var request = new ReferAFriendControlRequestData("", "", "", "", 0);
		//	var response = (ReferAFriendControlResponseData)DataCache.DataCache.GetProcessRequest(request, 3);

		//	var response2 = (ReferAFriendControlResponseData)DataCache.DataCache.GetProcessRequest(request, 3);
		//	Assert.IsTrue(response.);
		//}
	}
}
