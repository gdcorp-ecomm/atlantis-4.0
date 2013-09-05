using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.Services.Description;
using System.Xml.Serialization;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Microsoft.CSharp;

namespace Atlantis.Framework.DataProvider.Impl
{
  public class DataProviderRequest : IRequest, IAsyncRequest
  {
    private static Dictionary<string, string> m_dbConnections = new Dictionary<string, string>();
    private static Dictionary<string, Type> m_webServices = new Dictionary<string, Type>();
    private static ReaderWriterLock _wsdlLock = new ReaderWriterLock();
    private static CSharpCodeProvider m_CSharpProv = new CSharpCodeProvider();

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      object resp = null;
      IResponseData oResponseData = null;

      try
      {
        DataProviderRequestData oDataProviderRequestData = (DataProviderRequestData)oRequestData;

        ProviderRequestSetting requestSetting = DataProviderFile.GetRequestSetting(oDataProviderRequestData);
        if (requestSetting == null)
        {
          throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
            "RequestSetting " + oDataProviderRequestData.RequestName + " was not found in the DataProvider.xml file.",
            string.Empty);
        }

        Dictionary<string, object> outputParameters = null;

        switch (requestSetting.RequestSettingType)
        {
          case ProviderRequestSettingType.WebService:
            resp = DoWebServiceMethod(requestSetting, oDataProviderRequestData, out outputParameters);
            break;
          case ProviderRequestSettingType.StoredProcedure:
            resp = DoDatabaseCommand(requestSetting, oDataProviderRequestData, out outputParameters);
            break;
          case ProviderRequestSettingType.RestService:
            resp = DoRestServiceCall(requestSetting, oDataProviderRequestData);
            break;
          default:
            throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
              "RequestSetting " + oDataProviderRequestData.RequestName + " did not have a recognized type.",
              string.Empty);
        }

        oResponseData = new DataProviderResponseData(resp, outputParameters);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DataProviderResponseData(resp, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DataProviderResponseData(resp, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    #region IAsyncRequest Members

    private delegate DataSet AsyncDbMethodCaller(ProviderRequestSetting setting, DataProviderRequestData oRequestData, out Dictionary<string, object> outputParameters);
    private delegate object AsyncWsMethodCaller(ProviderRequestSetting setting, DataProviderRequestData oRequestData, out Dictionary<string, object> outputParameters);
    private delegate object AsyncRestMethodCaller(ProviderRequestSetting setting, DataProviderRequestData oRequestData);

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      DataProviderRequestData oDataProviderRequestData = (DataProviderRequestData)oRequestData;

      IAsyncResult oAsyncResult = null;

      ProviderRequestSetting requestSetting = DataProviderFile.GetRequestSetting(oDataProviderRequestData);
      if (requestSetting == null)
      {
        throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
          "RequestSetting " + oDataProviderRequestData.RequestName + " was not found in the DataProvider.xml file.",
          string.Empty);
      }

      switch (requestSetting.RequestSettingType)
      {
        case ProviderRequestSettingType.WebService:
          AsyncWsMethodCaller ws_caller = new AsyncWsMethodCaller(DoWebServiceMethod);
          AsyncState ws_oAsyncState = new AsyncState(oRequestData, oConfig, ws_caller, oState);
          Dictionary<string, object> outputParameters;
          oAsyncResult = ws_caller.BeginInvoke(requestSetting, oDataProviderRequestData, out outputParameters, oCallback, ws_oAsyncState);
          break;
        case ProviderRequestSettingType.StoredProcedure:
          AsyncDbMethodCaller db_caller = new AsyncDbMethodCaller(DoDatabaseCommand);
          AsyncState db_oAsyncState = new AsyncState(oRequestData, oConfig, db_caller, oState);
          Dictionary<string, object> outputDBParameters;
          oAsyncResult = db_caller.BeginInvoke(requestSetting, oDataProviderRequestData, out outputDBParameters, oCallback, db_oAsyncState);
          break;
        case ProviderRequestSettingType.RestService:
          AsyncRestMethodCaller rest_caller = new AsyncRestMethodCaller(DoRestServiceCall);
          AsyncState rest_oAsyncState = new AsyncState(oRequestData, oConfig, rest_caller, oState);
          oAsyncResult = rest_caller.BeginInvoke(requestSetting, oDataProviderRequestData, oCallback, rest_oAsyncState);
          break;
        default:
          throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
            "RequestSetting " + oDataProviderRequestData.RequestName + " did not have a recognized type.",
            string.Empty);
      }

      return oAsyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      object resp = null;
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;
      DataProviderRequestData oDataProviderRequestData = (DataProviderRequestData)oAsyncState.RequestData;
      IResponseData oResponseData = null;

      try
      {

        ProviderRequestSetting requestSetting = DataProviderFile.GetRequestSetting(oDataProviderRequestData);
        if (requestSetting == null)
        {
          throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
            "RequestSetting " + oDataProviderRequestData.RequestName + " was not found in the DataProvider.xml file.",
            string.Empty);
        }

        Dictionary<string, object> outputParameters = null;

        switch (requestSetting.RequestSettingType)
        {
          case ProviderRequestSettingType.WebService:
            AsyncWsMethodCaller ws_caller = (AsyncWsMethodCaller)oAsyncState.Request;
            resp = ws_caller.EndInvoke(out outputParameters, oAsyncResult);
            break;
          case ProviderRequestSettingType.StoredProcedure:
            AsyncDbMethodCaller db_caller = (AsyncDbMethodCaller)oAsyncState.Request;
            resp = db_caller.EndInvoke(out outputParameters, oAsyncResult);
            break;
          case ProviderRequestSettingType.RestService:
            AsyncRestMethodCaller rest_caller = (AsyncRestMethodCaller)oAsyncState.Request;
            resp = rest_caller.EndInvoke(oAsyncResult);
            break;
          default:
            throw new AtlantisException(oDataProviderRequestData, "DataProviderRequest.RequestHandler",
              "RequestSetting " + oDataProviderRequestData.RequestName + " did not have a recognized type.",
              string.Empty);
        }

        oResponseData = new DataProviderResponseData(resp, outputParameters);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DataProviderResponseData(resp, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DataProviderResponseData(resp, oDataProviderRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    public DataProviderRequest()
    {
    }

    #region Database Access

    private DataSet DoDatabaseCommand(ProviderRequestSetting dbs, DataProviderRequestData oDataProviderRequestData, out Dictionary<string, object> outputParameters)
    {
      DataSet ds = new DataSet(Guid.NewGuid().ToString());

      outputParameters = null;
      bool hasOutput = false;

      string connStr = LookupConnectionString(dbs, oDataProviderRequestData);
      using (SqlConnection sqlConn = new SqlConnection(connStr))
      {
        sqlConn.Open();

        using (SqlCommand sqlCmd = new SqlCommand(dbs.TargetName, sqlConn))
        {
          sqlCmd.CommandType = CommandType.StoredProcedure;
          sqlCmd.CommandTimeout = (int)oDataProviderRequestData.RequestTimeout.TotalSeconds;

          foreach (ProviderParameter p in dbs.ParamList)
          {
            SqlParameter pn = new SqlParameter();
            if (oDataProviderRequestData.Params.ContainsKey(p.Name))
            {
              pn.ParameterName = p.Name;
              pn.SqlDbType = GetSqlDbType(p.Type);
              pn.Direction = GetParameterDirection(p.Direction);
              if (pn.Direction != ParameterDirection.Input)
              {
                hasOutput = true;
              }

              switch (p.Type)
              {
                case "structured":
                  sqlCmd.Parameters.AddWithValue(p.Name, oDataProviderRequestData.Params[p.Name]);
                  break;
                default:
                  pn.SqlValue = oDataProviderRequestData.Params[p.Name];
                  sqlCmd.Parameters.Add(pn);
                  break;
              }
            }
          }

          using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
          {
            adp.Fill(ds);
          }

          if (hasOutput)
          {
            outputParameters = new Dictionary<string, object>();
            foreach (SqlParameter param in sqlCmd.Parameters)
            {
              if (param.Direction != ParameterDirection.Input)
              {
                outputParameters.Add(param.ParameterName, param.Value);
              }
            }
          }
        }
      }

      return ds;
    }

    private string LookupConnectionString(ProviderRequestSetting providerSetting, DataProviderRequestData request)
    {
      string dsnName = providerSetting.DSN;
      string appName = providerSetting.AppName;
      string certName = providerSetting.CertName;

      if (!string.IsNullOrEmpty(request.DataSourceName))
      {
        dsnName = request.DataSourceName;
      }

      if (!string.IsNullOrEmpty(request.ApplicationName))
      {
        appName = request.ApplicationName;
      }

      if (!string.IsNullOrEmpty(request.CertificateName))
      {
        certName = request.CertificateName;
      }

      string result = NetConnect.LookupConnectInfo(
        dsnName, certName, appName,
        "DataProviderRequest." + providerSetting.RequestName, ConnectLookupType.NetConnectionString);
      return result;
    }

    private SqlDbType GetSqlDbType(string type)
    {
      switch (type.ToLower())
      {
        case "int":
          return SqlDbType.Int;
        case "nvarchar":
          return SqlDbType.NVarChar;
        case "varchar":
        default:
          return SqlDbType.VarChar;
      }
    }

    private ParameterDirection GetParameterDirection(string direction)
    {
      string tempDirection = string.IsNullOrEmpty(direction) ? "input" : direction;
      switch (tempDirection.ToLower())
      {
        case "output":
          return ParameterDirection.Output;
        case "inputoutput":
          return ParameterDirection.InputOutput;
        case "return":
          return ParameterDirection.ReturnValue;
        default:
          return ParameterDirection.Input;
      }
    }

    #endregion

    #region Web Services

    private object DoWebServiceMethod(ProviderRequestSetting wss, DataProviderRequestData oDataProviderRequestData, out Dictionary<string, object> outputParameters)
    {
      Type serviceType = LookupWebServiceType(oDataProviderRequestData, wss);
      MethodInfo targetMethod = GetTargetMethod(oDataProviderRequestData, serviceType, wss.TargetName);
      PropertyInfo timeoutProperty = GetTimeoutProperty(oDataProviderRequestData, serviceType);

      outputParameters = null;
      Dictionary<int, object> tempOutputParameters = new Dictionary<int, object>();

      ParameterInfo[] parameters = targetMethod.GetParameters();
      object[] methodParms = parameters.Length > 0 ? new object[parameters.Length] : null;
      for (int i = 0; i < parameters.Length; ++i)
      {
        if (oDataProviderRequestData.Params.ContainsKey(parameters[i].Name))
        {
          methodParms[i] = oDataProviderRequestData.Params[parameters[i].Name];
          if (parameters[i].IsOut)
          {
            tempOutputParameters[i] = methodParms[i];
          }
        }
        else
        {
          throw new AtlantisException(oDataProviderRequestData, "DoWebServiceMethod", string.Format("Missing required parameter: {0} for Web Method: {1}",
                      parameters[i].Name, wss.TargetName), "");
        }
      }

      object result = null;
      using (IDisposable instance = Activator.CreateInstance(serviceType) as IDisposable)
      {
        if (!string.IsNullOrEmpty(wss.CertName))
        {
          var clientCertsProperty = GetClientCertificatesProperty(oDataProviderRequestData, serviceType);
          if (clientCertsProperty != null)
          {
            var cert = wss.GetClientCertificate(wss.CertName);
            cert.Verify();
            var certs = (X509CertificateCollection)clientCertsProperty.GetValue(instance, null);
            certs.Add(cert);
          }
        }

        if (timeoutProperty != null)
        {
          int millisecondTimeout = (int)oDataProviderRequestData.RequestTimeout.TotalMilliseconds;
          timeoutProperty.SetValue(instance, millisecondTimeout, null);
        }
        result = targetMethod.Invoke(instance, methodParms);

        if (tempOutputParameters.Count > 0)
        {
          outputParameters = new Dictionary<string, object>(tempOutputParameters.Count);
          foreach (int key in tempOutputParameters.Keys)
          {
            outputParameters[parameters[key].Name] = methodParms[key];
          }
        }
      }

      return result;
    }

    private Type LookupWebServiceType(DataProviderRequestData oDataProviderRequestData, ProviderRequestSetting wss)
    {
      var hostName = wss.HostName;
      try
      {
        _wsdlLock.AcquireReaderLock(Timeout.Infinite);
        Type serviceType;
        if (m_webServices.TryGetValue(oDataProviderRequestData.RequestName, out serviceType))
        {
          _wsdlLock.ReleaseLock();
        }
        else
        {
          _wsdlLock.UpgradeToWriterLock(Timeout.Infinite);
          if (m_webServices.TryGetValue(oDataProviderRequestData.RequestName, out serviceType))
          {
            _wsdlLock.ReleaseLock();
          }
          else
          {
            string suffixedHostName = EnsureWSDLQuery(hostName);

            var webReq = WebRequest.Create(suffixedHostName);
            if (!string.IsNullOrEmpty(wss.CertName))
            {
              ((HttpWebRequest)webReq).ClientCertificates.Add(wss.GetClientCertificate(wss.CertName));
            }

            Stream stream = webReq.GetResponse().GetResponseStream();
            ServiceDescription serviceDesc = ServiceDescription.Read(stream);
            string serviceName = serviceDesc.Services[0].Name;
            // Generate the Proxy Class
            ServiceDescriptionImporter servImport = new ServiceDescriptionImporter();
            servImport.AddServiceDescription(serviceDesc, string.Empty, string.Empty);
            servImport.ProtocolName = "Soap";
            servImport.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;
            CodeNamespace ns = new CodeNamespace();
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(ns);
            ServiceDescriptionImportWarnings warnings = servImport.Import(ns, ccu);
            StringWriter sw = new StringWriter();
            m_CSharpProv.GenerateCodeFromNamespace(ns, sw, null);
            CompilerParameters param = new CompilerParameters(new string[] { 
              "System.dll", "System.Xml.dll", "System.Web.Services.dll", "System.Data.dll" });
            param.GenerateExecutable = false;
            param.GenerateInMemory = true;
            param.TreatWarningsAsErrors = false;
            param.WarningLevel = 4;
            CompilerResults results = new CompilerResults(null);
            results = m_CSharpProv.CompileAssemblyFromSource(param, sw.ToString());
            Assembly proxyAssembly = results.CompiledAssembly;
            // Service Type
            serviceType = proxyAssembly.GetType(serviceName);
            if (null != serviceType)
            {
              m_webServices[oDataProviderRequestData.RequestName] = serviceType;
            }

            _wsdlLock.ReleaseLock();
          }
        }

        return serviceType;
      }
      catch (Exception ex)
      {
        throw new AtlantisException(oDataProviderRequestData, "LookupWebServiceType",
          string.Format("Unable to find Web Service, Request Name: {0}, Web Service: {1}", oDataProviderRequestData.RequestName, hostName), "", ex);
      }
      finally
      {
        _wsdlLock.ReleaseLock();
      }

      throw new AtlantisException(oDataProviderRequestData, "LookupWebServiceType",
        string.Format("Unable to find Web Service, Request Name: {0}, Web Service: {1}", oDataProviderRequestData.RequestName, hostName), "");
    }

    private string EnsureWSDLQuery(string hostName)
    {
      Uri uri = new Uri(hostName);

      if (string.IsNullOrEmpty(uri.Query))
      {
        if (uri.AbsolutePath.ToLower().EndsWith(".dll"))
        {
          string[] segments = uri.Segments;
          string dllName = segments[segments.Length - 1];
          dllName = dllName.Replace(".dll", "");
          return hostName + "?Handler=Gen" + dllName + "WSDL";
        }
        else if (uri.AbsolutePath.ToLower().EndsWith(".asmx"))
        {
          return hostName + "?WSDL";
        }
      }

      return hostName;
    }

    private MethodInfo GetTargetMethod(DataProviderRequestData oDataProviderRequestData, Type serviceType, string targetName)
    {
      MethodInfo[] methods = serviceType.GetMethods(BindingFlags.DeclaredOnly |
                                BindingFlags.IgnoreCase | BindingFlags.Instance |
                                BindingFlags.InvokeMethod | BindingFlags.Public);
      foreach (MethodInfo m in methods)
      {
        if (0 == m.Name.CompareTo(targetName))
          return m;
      }

      throw new AtlantisException(oDataProviderRequestData, "GetTargetMethod",
        string.Format("Unable to find Web Method: {0}, in Web Service: {1}", targetName, serviceType.Name), "");
    }

    private PropertyInfo GetTimeoutProperty(DataProviderRequestData oDataProviderRequestData, Type serviceType)
    {
      PropertyInfo result = null;
      PropertyInfo[] properties = serviceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (PropertyInfo property in properties)
      {
        if (property.Name == "Timeout")
        {
          result = property;
          break;
        }
      }
      return result;
    }

    private PropertyInfo GetClientCertificatesProperty(DataProviderRequestData oDataProviderRequestData, Type serviceType)
    {
      PropertyInfo result = null;
      PropertyInfo[] properties = serviceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (PropertyInfo property in properties)
      {
        if (property.Name == "ClientCertificates")
        {
          result = property;
          break;
        }
      }
      return result;
    }

    #endregion

    #region REST Services

    private object DoRestServiceCall(ProviderRequestSetting rss, DataProviderRequestData oDataProviderRequestData)
    {
      byte[] oRequestBytes = Encoding.ASCII.GetBytes(oDataProviderRequestData.ToXML());

      StringBuilder sb = new StringBuilder();
      foreach (ProviderParameter p in rss.ParamList)
      {
        if (sb.Length == 0)
          sb.Append("?");
        else
          sb.Append("&");
        sb.AppendFormat("{0}={1}", p.Name, oDataProviderRequestData.Params[p.Name]);
      }

      HttpWebRequest oRequest = (HttpWebRequest)HttpWebRequest.Create(rss.HostName + sb.ToString());

      oRequest.Timeout = (int)oDataProviderRequestData.RequestTimeout.TotalMilliseconds;
      oRequest.KeepAlive = false;
      oRequest.Method = "POST";
      oRequest.ContentLength = oRequestBytes.Length;
      string result = string.Empty;

      using (Stream requestStream = oRequest.GetRequestStream())
      {
        requestStream.Write(oRequestBytes, 0, oRequestBytes.Length);
        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        try
        {
          using (StreamReader srResponse = new StreamReader(oResponse.GetResponseStream()))
          {
            result = srResponse.ReadToEnd();
          }
        }
        finally
        {
          if (oResponse != null)
          {
            oResponse.Close();
          }
        }
      }

      return result;
    }

    #endregion
  }
}
