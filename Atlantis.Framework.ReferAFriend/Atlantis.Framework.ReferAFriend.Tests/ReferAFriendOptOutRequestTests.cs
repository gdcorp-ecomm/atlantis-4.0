using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendOptOutRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendOptOutRequest_Success()
		{
			var request = new ReferAFriendOptOutRequestData(new string[] {"dclayton@godaddy.com"}, "847503", "", "", "", 0);
			var response = (ReferAFriendOptOutResponseData)Engine.Engine.ProcessRequest(request, 8);
			Assert.IsTrue(response.OptOutList.Count == 0);
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
