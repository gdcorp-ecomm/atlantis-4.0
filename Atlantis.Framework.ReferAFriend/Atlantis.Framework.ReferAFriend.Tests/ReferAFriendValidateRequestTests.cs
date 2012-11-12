using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendValidateRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendValidateRequest_Success()
		{
			var request = new ReferAFriendValidateRequestData("test", "", "", "", "", 0);
			var response = (ReferAFriendValidateResponseData)Engine.Engine.ProcessRequest(request, 1);
			Assert.IsTrue(response.IsSuccess);
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendValidateRequest_NotValid()
		{
			var request = new ReferAFriendValidateRequestData("dlc11", "", "", "", "", 0);
			var response = (ReferAFriendValidateResponseData)Engine.Engine.ProcessRequest(request, 1);
			Assert.IsFalse(response.IsValid);
			Assert.IsTrue(response.IsSuccess);
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendValidateRequest_Valid()
		{
			var request = new ReferAFriendValidateRequestData("dlc102412", "", "", "", "", 0);
			var response = (ReferAFriendValidateResponseData)Engine.Engine.ProcessRequest(request, 1);
			Assert.IsTrue(response.IsValid);
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
