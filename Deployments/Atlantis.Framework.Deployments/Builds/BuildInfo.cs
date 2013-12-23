using System;
using System.Web;
using System.Xml;

namespace Atlantis.Framework.Deployments.Builds
{
  public class BuildInfo
  {
    private BuildInfo()
    {
      ProcessFile();
    }

    public string BuildTag
    {
      get;
      private set;
    }

    public int BuildNumber
    {
      get;
      private set;
    }

    private static BuildInfo _instance;
    private static readonly object _syncLock = new object();

    static public BuildInfo Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_syncLock)
          {
            if (_instance == null)
              _instance = new BuildInfo();
          }
        }
        return _instance;
      }
    }

    private void ProcessFile()
    {
      try
      {
        string filePath = HttpContext.Current.ApplicationInstance.Server.MapPath(@"~\buildinfo.xml");
        var doc = new XmlDocument();
        doc.Load(filePath);
        var rootNode = doc.SelectSingleNode("/BuildInfo");
        BuildTag = rootNode.Attributes["buildTag"].Value;
        BuildNumber = int.Parse(rootNode.Attributes["buildNumber"].Value);
      }
      catch (Exception)
      {
        BuildTag = "n/a";
        BuildNumber = -1;
      }
    }
  }
}