<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPixels.aspx.cs" Inherits="Atlantis.Framework.PixelsGet.Test.Web.GetPixels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <style type="text/css">
    body { font-size: 12px;color:navy;}

  </style>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <asp:Repeater OnItemDataBound="rptPixels_ItemDataBound" runat="server" ID="rptPixels">
        <ItemTemplate>
          <div style="padding: 5px; margin: 5px; border: solid 1px black">
            Name: <%# Eval("Name") %><br />
            Type: <%# Eval("PixelType") %><br/>
          AppSetting: <%# Eval("AppSettingName") %>
            <fieldset title="Value"><legend>Pixel Value</legend>
              <%# HttpUtility.HtmlEncode(Eval("Value")) %>
            </fieldset>
            <fieldset title="Ci Codes"><legend>Ci Codes</legend>
              <asp:Repeater runat="server" ID="rptCiCodes">
                <ItemTemplate>
                  <%# ((System.Data.DataRowView)Container.DataItem)[0] %>
                </ItemTemplate>
              </asp:Repeater>
            </fieldset>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </form>
</body>
</html>
