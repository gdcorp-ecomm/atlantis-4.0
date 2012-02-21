<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetShopperInvoicesTest.aspx.cs"
  Inherits="Atlantis.Framework.EcommInvoices.TestWeb.GetShopperInvoicesTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
</head>
<body>
  <form id="form1" runat="server">
  <div>
    <asp:GridView ID="GridView1" AutoGenerateColumns="true" runat="server">
      <Columns>
        <asp:TemplateField>
          <ItemTemplate>
            <%# Container.DataItemIndex + 1 %>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
  </div>
  </form>
</body>
</html>
