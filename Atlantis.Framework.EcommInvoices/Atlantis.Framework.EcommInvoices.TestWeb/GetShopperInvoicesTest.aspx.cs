using System;
using Atlantis.Framework.EcommInvoices.Interface;

namespace Atlantis.Framework.EcommInvoices.TestWeb
{
  public partial class GetShopperInvoicesTest : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      GetOrderedByAmountInvoicesForShopperTest();
    }

    private void GetOrderedByAmountInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes();
      retAttr.SortColumn = "expireDate";
      retAttr.PageSize = 3;
      retAttr.CurrentPage = 1;
      retAttr.SortDirection = "desc";
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("133729", retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      GridView1.DataSource = response.Invoices;// OrderByDescending(x => x.Amount).Skip(5).Take(5);
      GridView1.DataBind();
    }  
  }
}