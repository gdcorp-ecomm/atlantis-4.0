using System;
using System.Collections.Generic;
using System.IO;
using Atlantis.Framework.Engine.Configuration;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Testing.MockEngine
{
  public static class EngineRequestMocking
  {
    private class MockedEngineRequest : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        return _overrideImpls[config.RequestType](requestData, config);
      }
    }

    private class MockEngineRequestSetting : IEngineRequestSetting
    {
      public int RequestType { get; set; }
      public string TypeName { get; set; }
      public string AssemblyName { get; set; }
      public string WebServiceUrl { get; set; }
      public IDictionary<string, string> ConfigValues { get; set; }
    }

    private static bool _isOverrideInitialized;
    private static EngineRequestSettings _originalSettings;
    private static List<IEngineRequestSetting> _overrideSettings;
    private static Dictionary<int, Func<RequestData, ConfigElement, IResponseData>> _overrideImpls = new Dictionary<int, Func<RequestData, ConfigElement, IResponseData>>();

    public static void RegisterOverride(int requestType, IResponseData response)
    {
      Func<RequestData, ConfigElement, IResponseData> returnCannedResponseImplementation = (data, element) => response;
      RegisterOverride(requestType, returnCannedResponseImplementation);
    }

    public static void RegisterOverride(int requestType, Func<RequestData, ConfigElement, IResponseData> implementation)
    {
      if (!_isOverrideInitialized)
      {
        InitializeOverrides();
      }

      RegisterOverrideImplementation(requestType, implementation);
      RegisterOverideConfiguration(requestType);
      ApplyOverrideConfigurationToEngine();
    }

    public static void RegisterAsyncOverride(int requestType, IResponseData response)
    {
      throw new NotImplementedException();
    }

    private static void InitializeOverrides()
    {
      _originalSettings = Engine.Engine.EngineRequestSettings;
      _overrideSettings = new List<IEngineRequestSetting>();
      _overrideImpls.Clear();
      _isOverrideInitialized = true;
    }

    private static void RegisterOverrideImplementation(int requestType, Func<RequestData, ConfigElement, IResponseData> implementation)
    {
      _overrideImpls[requestType] = implementation;
    }

    private static void RegisterOverideConfiguration(int requestType)
    {
      MockEngineRequestSetting setting = new MockEngineRequestSetting();
      setting.AssemblyName = Path.GetFileName(typeof(MockedEngineRequest).Assembly.CodeBase);
      setting.TypeName = typeof(MockedEngineRequest).FullName;
      setting.RequestType = requestType;
      _overrideSettings.Add(setting);
    }

    private static void ApplyOverrideConfigurationToEngine()
    {
      Engine.Engine.EngineRequestSettings = AtlantisConfiguration.FromCustomConfiguration(_overrideSettings);
    }

    public static void ClearOverrides()
    {
      Engine.Engine.EngineRequestSettings = _originalSettings;
      _isOverrideInitialized = false;
    }
  }
}
