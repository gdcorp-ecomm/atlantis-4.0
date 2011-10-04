using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetEmailPlansForShopper.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;

namespace ServiceTester
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      string shopperId = txtShopper.Text;
      int requestType = 225;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCGetEmailPlansForShopperRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout);




      try
      {
        var getEmailPlansForShopperResponseData = (ECCGetEmailPlansForShopperResponseData)Engine.ProcessRequest(requestData, requestType);
        txtResults.Text = getEmailPlansForShopperResponseData.ToXML();
       
      }
      catch (Exception ex)
      {
        txtResults.Text = ex.Message;
      }
    }

    private void btnSerialize_Click(object sender, EventArgs e)
    {
      EccJsonResponse<EccEmailPlan> oResponse = new EccJsonResponse<EccEmailPlan>();
      oResponse.Item.Message = string.Empty;
      oResponse.Item.ResultCode = 0;
      oResponse.Item.State = null;
      oResponse.Item.Timer = (0.46584).ToString();
      oResponse.Item.Results.Add(new EccEmailPlan(Guid.Empty, EccAccountType.email, EccAccountStatus.setup, DateTime.Now, "US", "Test Plan 1"));
      oResponse.Item.Results.Add(new EccEmailPlan(Guid.Empty, EccAccountType.emailforwarding, EccAccountStatus.pendremove, DateTime.Now, "US", "Test Plan 2"));
      oResponse.Item.Results.Add(new EccEmailPlan(Guid.Empty, EccAccountType.email, EccAccountStatus.setup, DateTime.Now, "US", "Test Plan 3"));

      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(EccJsonResponse<EccEmailPlan>));
      MemoryStream ms = new MemoryStream();
      ser.WriteObject(ms, oResponse);

      string json = Encoding.Default.GetString(ms.ToArray());
      ms.Close();

      txtResults.Text = json;
    }

    

 
  }
}
