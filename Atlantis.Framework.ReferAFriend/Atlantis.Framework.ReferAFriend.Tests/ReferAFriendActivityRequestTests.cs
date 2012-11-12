using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendActivitySummaryRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendActivitySummaryRequest_Success()
		{
			var request = new ReferAFriendActivityRequestData("dlc1", DateTime.Parse("10/21/2012"), DateTime.Parse("11/5/2012"), "847503", "", "", "", 0);
			var response = (ReferAFriendActivityResponseData)Engine.Engine.ProcessRequest(request, 6);
			Assert.IsTrue(response.Items.Count == 0);
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
