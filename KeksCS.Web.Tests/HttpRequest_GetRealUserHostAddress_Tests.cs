using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeksCS.Web.Tests
{
	[TestClass]
	public class HttpRequest_GetIPFromHeaderValue_Tests
	{
		[TestMethod]
		public void Normal_IP()
		{
			string testValue = "109.87.9.250";
			var result = HttpRequestExtensions.GetIPFromHeaderValue(testValue);
			Assert.AreEqual(testValue, result);
		}

		[TestMethod]
		public void IP_With_Port()
		{
			string testValue = "109.87.9.250:56895";
			var result = HttpRequestExtensions.GetIPFromHeaderValue(testValue);
			Assert.AreEqual("109.87.9.250", result);
		}
	}
}
