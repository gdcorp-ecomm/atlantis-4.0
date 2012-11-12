using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendRequest_Success()
		{
			var request = new ReferAFriendRequestData("847503", "", "", "", 0);
			var response = (ReferAFriendResponseData)Engine.Engine.ProcessRequest(request, 5);
			Assert.IsTrue(response.IsInStoreCreditAwardable);
			Assert.AreEqual("wowdlc11", response.ItemSourceCode);
			Assert.IsTrue(response.IsSuccess);
		}

		
	}
}
