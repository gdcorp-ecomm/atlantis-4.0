using Atlantis.Framework.Interface;
using Atlantis.Framework.WhoIsGetRegData.Interface;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace Atlantis.Framework.WhoIsGetRegData.Impl
{
  public class WhoIsGetRegDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      string str = string.Empty;
      try
      {
        return (IResponseData)new WhoIsGetRegDataResponseData(this.ProcessXML(this.GetWhoIsRegData((WhoIsGetRegDataRequestData)oRequestData, oConfig)));
      }
      catch (AtlantisException ex)
      {
        return (IResponseData)new WhoIsGetRegDataResponseData(ex);
      }
      catch (Exception ex)
      {
        return (IResponseData)new WhoIsGetRegDataResponseData(oRequestData, ex);
      }
    }

    private string GetWhoIsRegData(WhoIsGetRegDataRequestData whoisRegDataRequestData, ConfigElement oConfig)
    {
      string str = string.Empty;
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(((WsConfigElement)oConfig).WSURL + "?domain=" + whoisRegDataRequestData.DomainToLookup + "&querytype=4");
      httpWebRequest.Timeout = (int)whoisRegDataRequestData.RequestTimeout.TotalMilliseconds;
      httpWebRequest.UserAgent = "Atlantis Framework Fetch Request";
      HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      using (httpWebResponse)
      {
        Encoding encoding = Encoding.GetEncoding("UTF-8");
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encoding);
        using (streamReader)
          str = streamReader.ReadToEnd();
        streamReader.Close();
      }
      httpWebResponse.Close();
      return str;
    }

    private string ProcessXML(string whoisRegDataXml)
    {
      string str = whoisRegDataXml.Replace("<pre>", string.Empty).Replace("</pre>", string.Empty);
      int startIndex = str.IndexOf("<br>");
      if (startIndex != -1)
        str = str.Remove(startIndex, 4);
      return this.TransformXML(WhoIsGetRegDataRequest.RemoveInvalidChars(str.Replace("<br>", "%%%br%%%"))).Tables[0].Rows[0]["REGISTRYRAWWHOIS"].ToString().Replace("%%%br%%%", "<br>");
    }

    private DataSet TransformXML(string XML)
    {
      DataSet dataSet = new DataSet();
      MemoryStream memoryStream = (MemoryStream)null;
      try
      {
        memoryStream = new MemoryStream(WhoIsGetRegDataRequest.StringToByteArray(XML));
        memoryStream.Position = 0L;
        int num = (int)dataSet.ReadXml((Stream)memoryStream);
      }
      finally
      {
        if (memoryStream != null)
          memoryStream.Close();
      }
      return dataSet;
    }

    private static byte[] StringToByteArray(string str)
    {
      return new UTF8Encoding().GetBytes(str);
    }

    public static string RemoveInvalidChars(string str)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < str.Length; ++index)
      {
        char ch = str[index];
        if ((int)ch < 253 && (int)ch > 31 || ((int)ch == 9 || (int)ch == 10) || (int)ch == 13)
          stringBuilder.Append(ch);
      }
      return ((object)stringBuilder).ToString();
    }
  }
}
