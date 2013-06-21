using Atlantis.Framework.Web.CaptchaCtl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ValidateCaptchaAction : System.Web.UI.Page
{
  #region JsonData
  [DataContract]
  protected class JsonData
  {
    private bool _valid = false;
    [DataMember]
    public bool Valid
    {
      get { return _valid; }
      set { _valid = value; }
    }

    private string _newImage = string.Empty;
    [DataMember]
    public string NewImage
    {
      get { return _newImage; }
      set { _newImage = value; }
    }
  }

  readonly JsonData _data = new JsonData();

  protected JsonData currentDataObject
  {
    get
    {
      return _data;
    }
  }

  #endregion

  protected override void Render(HtmlTextWriter writer)
  {
    WriteSerializedJSON(writer, string.Empty);
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    string captchaValue = Request.QueryString["captchaValue"] as string;
    string instanceID = Request.QueryString["instanceID"] as string;
    bool isValid = CaptchaTemplateControl.IsCaptchaValid(captchaValue, instanceID);
    currentDataObject.Valid = isValid;
  }


  protected virtual void WriteSerializedJSON(HtmlTextWriter writer, string errorMessage)
  {
    Response.ContentType = "application/json";
    string json = string.Empty;

    if (string.IsNullOrEmpty(errorMessage))
    {
      json = SerializeToJson<JsonData>(currentDataObject);
    }
    else
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("{\"Error\":\"");
      sb.Append(Server.HtmlEncode(errorMessage.Replace('\n', ' ').Replace('\r', ' ')));
      sb.Append("\"}");
      json = sb.ToString();
    }

    if (writer != null)
    {
      writer.Write(json);
    }
    else
    {
      Response.Write(json);
    }
  }

  protected string SerializeToJson<T>(T dataContractObject)
  {
    string resultJson;
    DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(T));
    using (MemoryStream ms = new MemoryStream())
    {
      jsSerializer.WriteObject(ms, dataContractObject);
      resultJson = Encoding.Default.GetString(ms.ToArray());
      ms.Close();
    }
    return resultJson;
  }

}