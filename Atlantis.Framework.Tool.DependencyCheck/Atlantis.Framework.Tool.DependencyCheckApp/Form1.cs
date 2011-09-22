using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Atlantis.Framework.Interface;
using System.Net;

namespace Atlantis.Framework.Tool.DependencyCheckApp
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      GetEngineDependencies();
    }

    private void GetEngineDependencies()
    {
      IList<ConfigElement> configElements = Engine.Engine.GetConfigElements();
      List<DependencyItem> items = new List<DependencyItem>(configElements.Count);

      foreach (ConfigElement config in configElements)
      {
        WsConfigElement webserviceElement = config as WsConfigElement;
        if ((webserviceElement != null) && (!string.IsNullOrEmpty(webserviceElement.WSURL)))
        {
          DependencyItem item = new DependencyItem();
          item.ImplClass = webserviceElement.ProgID;
          item.Url = webserviceElement.WSURL;

          Uri url;
          if (Uri.TryCreate(item.Url, UriKind.Absolute, out url))
          {
            item.Host = url.Host;
            item.Port = 80;
            if (url.Scheme.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
              item.Port = 443;
            }

            item.IpAddress = "Unknown";

            try
            {
              IPAddress[] addresses = Dns.GetHostAddresses(item.Host);
              if ((addresses != null) && (addresses.Length > 0))
              {
                item.IpAddress = addresses[0].ToString();
              }
            }
            catch { }
          }

          items.Add(item);
        }

      }

      dgResults.DataSource = items;
      dgResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
    }
  }
}
