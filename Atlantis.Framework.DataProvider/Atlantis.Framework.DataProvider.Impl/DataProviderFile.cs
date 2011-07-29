using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataProvider.Impl
{
  internal static class DataProviderFile
  {
    private static Dictionary<string, ProviderRequestSetting> _requestSettings;
    static DataProviderFile()
    {
      _requestSettings = new Dictionary<string, ProviderRequestSetting>();
      string dataProviderConfigFilePath = string.Empty;

      try
      {
        Uri pathUri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
        dataProviderConfigFilePath = Path.Combine(pathUri.LocalPath, "DataProvider.xml");


        FileInfo dataProviderFile = new FileInfo(dataProviderConfigFilePath);
        if (dataProviderFile.Exists)
        {
          ProcessFile(dataProviderFile);
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(
          "DataProviderFile.StaticConstructor", "0", ex.Message, dataProviderConfigFilePath, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    private static void ReadSetttingNode(XmlReader reader, out ProviderRequestSetting rs)
    {
      rs = new ProviderRequestSetting();
      rs.RequestName = reader.GetAttribute("name");
      rs.HostName = reader.GetAttribute("host_name");
      if (string.IsNullOrEmpty(rs.DSN))
      {
        rs.DSN = reader.GetAttribute("dsn");
      }

      if (string.IsNullOrEmpty(rs.AppName))
      {
        rs.AppName = reader.GetAttribute("app_name");
      }

      if (string.IsNullOrEmpty(rs.CertName))
      {
        rs.CertName = reader.GetAttribute("cert_name");
      }

      rs.TargetName = reader.GetAttribute("target_name");
      rs.ParamList = new List<ProviderParameter>();
    }

    private static void ReadNodeParams(XmlReader reader, ProviderRequestSetting rs)
    {
      if (reader.ReadToDescendant("param"))
      {
        do
        {
          ProviderParameter param = new ProviderParameter();
          param.Name = reader.GetAttribute("name");
          param.Type = reader.GetAttribute("type");
          param.Size = 0;
          string sz = reader.GetAttribute("size");
          if (!string.IsNullOrEmpty(sz))
          {
            param.Size = Convert.ToInt32(sz);
          }
          string direction = reader.GetAttribute("direction");
          if (!string.IsNullOrEmpty(direction))
          {
            param.Direction = direction;
          }


          rs.ParamList.Add(param);
        }
        while (reader.ReadToNextSibling("param"));
      }
    }

    private static  void ProcessFile(System.IO.FileInfo fileInfo)
    {
      Dictionary<string, ProviderRequestSetting> requestSettings = new Dictionary<string, ProviderRequestSetting>();

      using (XmlReader reader = XmlReader.Create(fileInfo.FullName))
      {
        try
        {
          while (reader.Read())
          {
            if (reader.NodeType == XmlNodeType.Element)
            {
              if (reader.Name == "dbsetting")
              {
                ProviderRequestSetting dbs;
                ReadSetttingNode(reader, out dbs);
                ReadNodeParams(reader, dbs);
                dbs.RequestSettingType = ProviderRequestSettingType.StoredProcedure;
                requestSettings.Add(dbs.RequestName, dbs);
              }
              else if (reader.Name == "wssetting")
              {
                ProviderRequestSetting wss;
                ReadSetttingNode(reader, out wss);
                ReadNodeParams(reader, wss);
                wss.RequestSettingType = ProviderRequestSettingType.WebService;
                requestSettings.Add(wss.RequestName, wss);
              }
              else if (reader.Name == "rssetting")
              {
                ProviderRequestSetting rss;
                ReadSetttingNode(reader, out rss);
                ReadNodeParams(reader, rss);
                rss.RequestSettingType = ProviderRequestSettingType.RestService;
                requestSettings.Add(rss.RequestName, rss);
              }
            }
          }
          _requestSettings = requestSettings;
        }
        finally
        {
          reader.Close();
        }
      }
    }

    public static ProviderRequestSetting GetRequestSetting(DataProviderRequestData request)
    {
      ProviderRequestSetting result = null;

      if (!_requestSettings.TryGetValue(request.RequestName, out result))
      {
        result = null;
      }

      return result;
    }
  }

}
