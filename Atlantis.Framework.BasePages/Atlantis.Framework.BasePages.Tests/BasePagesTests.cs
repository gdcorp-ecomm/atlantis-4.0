using System;
using System.Reflection;
using Atlantis.Framework.BasePages.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Atlantis.Framework.BasePages.Tests
{
  [TestClass]
  public class BasePagesTests
  {
    [TestMethod]
    public void JsonCallbackRegex()
    {
      JsonContent content = new JsonContent(string.Empty, string.Empty, null);
      Assembly basePages = Assembly.GetAssembly(content.GetType());
      Type jsonSecurityType = basePages.GetType("Atlantis.Framework.BasePages.Json.JsonSecurity");
      MethodInfo isCallbackValidMethod = jsonSecurityType.GetMethod("IsCallbackValid", BindingFlags.NonPublic | BindingFlags.Static);

      JsonSecurity.ValidateCallbackValue = true;

      bool result = IsCallbackValue(isCallbackValidMethod, "pcj_setData");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_set!D$at-a");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_set#Data");
      Assert.IsFalse(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s.etData");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s<etData");
      Assert.IsFalse(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s(etData");
      Assert.IsFalse(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s[etData");
      Assert.IsFalse(result);

      JsonSecurity.ValidateCallbackValue = false;

      result = IsCallbackValue(isCallbackValidMethod, "pcj_set#Data");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s<etData");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s(etData");
      Assert.IsTrue(result);

      result = IsCallbackValue(isCallbackValidMethod, "pcj_s[etData");
      Assert.IsTrue(result);

    }

    private bool IsCallbackValue(MethodInfo isCallbackValidMethod, string testValue)
    {
      object[] callbackArgs = new object[1];
      callbackArgs[0] = testValue;

      object result = isCallbackValidMethod.Invoke(null, callbackArgs);
      return (bool)result;
    }
    [TestClass]
    public class testpage: AtlantisContextJsonDataBasePage
    {
      string jsonToBeSerialized = "informações";
      protected override string GetSerializedJson()
      {
        return SerializeToJson(jsonToBeSerialized);
      }
      public string serialize()
      {
        return GetSerializedJson();
      }
    }
    [TestMethod]
    public void jsonDataBaseSerialization ()
    {
      testpage tp = new testpage();
      Assert.AreEqual("\"informações\"", tp.serialize());
    }

  }
}
