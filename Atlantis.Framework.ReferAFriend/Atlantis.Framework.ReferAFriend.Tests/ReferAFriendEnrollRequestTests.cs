using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendEnrollRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendEnrollRequest_Success()
		{
			string refCode = string.Format("{0}{1:hhmm}", DateTime.Now.DayOfYear, DateTime.Now);
			var request = new ReferAFriendEnrollRequestData(refCode, "847503", "", "", "", 0);
			var response = (ReferAFriendEnrollResponseData)Engine.Engine.ProcessRequest(request, 4);
			Assert.IsFalse(response.IsTaken);
			Assert.IsFalse(response.IsBlacklisted);
			Assert.IsTrue(response.IsSuccess);
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendEnrollRequest_Taken()
		{
			string refCode = string.Format("dlc1026", DateTime.Now);
			var request = new ReferAFriendEnrollRequestData(refCode, "847503", "", "", "", 0);
			var response = (ReferAFriendEnrollResponseData)Engine.Engine.ProcessRequest(request, 4);
			Assert.IsFalse(response.IsBlacklisted, "Item is not blacklisted");
			Assert.IsTrue(response.IsTaken, "Item is taken");
			Assert.IsTrue(response.IsSuccess);
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendEnrollRequest_Blacklisted()
		{
			string refCode = string.Format("UnitTst", DateTime.Now);
			var request = new ReferAFriendEnrollRequestData(refCode, "847503", "", "", "", 0);
			var response = (ReferAFriendEnrollResponseData)Engine.Engine.ProcessRequest(request, 4);
			Assert.IsTrue(response.IsBlacklisted, "Item is not blacklisted");
			Assert.IsTrue(response.IsTaken, "Item is taken");
			Assert.IsTrue(response.IsSuccess);
		}

		//[TestMethod]
		//[DeploymentItem("atlantis.config")]
		//[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		//public void ReferAFriendEnrollRequest_NotValid()
		//{
		//	var request = new ReferAFriendEnrollRequestData("test", "", "", "", "", 0);
		//	var response = (ReferAFriendEnrollResponseData)Engine.Engine.ProcessRequest(request, 1);
		//	Assert.IsFalse(response.IsValid);
		//}

		//[TestMethod]
		//[DeploymentItem("atlantis.config")]
		//[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		//public void ReferAFriendEnrollRequest_Valid()
		//{
		//	var request = new ReferAFriendEnrollRequestData("dlc102412", "", "", "", "", 0);
		//	var response = (ReferAFriendEnrollResponseData)Engine.Engine.ProcessRequest(request, 1);
		//	Assert.IsTrue(response.IsValid);
		//}
	}
}
