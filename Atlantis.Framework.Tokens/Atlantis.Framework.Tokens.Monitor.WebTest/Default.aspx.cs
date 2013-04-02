using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Tokens.Monitor.WebTest
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string inputText1 = GetTextDataResource("inputdata1.txt");
      string inputText2 = GetTextDataResource("inputdata2.txt");

      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();

      string outputText1;
      TokenManager.ReplaceTokens(inputText1, container, null, out outputText1);

      ITokenEncoding tokenEncoding = new QuoteEncoding();
      string outputText2;
      TokenManager.ReplaceTokens(inputText2, container, tokenEncoding, out outputText2);
    }

    public static string GetTextDataResource(string dataName)
    {
      string result;
      string resourcePath = "Atlantis.Framework.Tokens.Monitor.WebTest.Data." + dataName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = textReader.ReadToEnd();
      }

      return result;
    }

    private class QuoteEncoding : ITokenEncoding
    {
      public string DecodeTokenData(string rawTokenData)
      {
        return rawTokenData.Replace("\\\"", "\"");
      }

      public string EncodeTokenResult(string tokenResult)
      {
        return tokenResult.Replace("\"", "\\\"");
      }
    }

    private class MockDebug : ProviderBase, IDebugContext
    {
      List<KeyValuePair<string, string>> _data;

      public MockDebug(IProviderContainer container)
        : base(container)
      {
        _data = new List<KeyValuePair<string, string>>();
      }

      public List<KeyValuePair<string, string>> GetDebugTrackingData()
      {
        return _data;
      }

      public void LogDebugTrackingData(string key, string data)
      {
        _data.Add(new KeyValuePair<string, string>(key, data));
      }

      public string GetQaSpoofQueryValue(string spoofParamName)
      {
        return string.Empty;
      }
    }
  }
}