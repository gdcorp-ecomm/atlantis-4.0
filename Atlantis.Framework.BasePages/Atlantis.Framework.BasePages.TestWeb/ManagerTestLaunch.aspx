<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagerTestLaunch.aspx.cs" Inherits="Atlantis.Framework.BasePages.TestWeb.ManagerTestLaunch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Launch ManagerTest for Shopper Id:<br />
        <asp:TextBox ID="tbShopperId" runat="server" Text="832652"></asp:TextBox><br />
        <asp:Button ID="cmdLaunch" runat="server" Text="Launch!" /><br />
        <asp:Button ID="cmdLaunchMstk" runat="server" Text = "Launch MSTK!" />
    </div>
    </form>
</body>
</html>
