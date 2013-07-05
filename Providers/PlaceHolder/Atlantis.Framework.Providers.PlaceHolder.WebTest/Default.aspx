<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
</head>
<body>
  <div>
    [@P[userControl:<Data id="banner" location="~/controls/banner.ascx"><Parameters><Parameter key="title" value="Attention!" /><Parameter key="text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
    [@P[userControl:<Data id="banner1" location="~/controls/banner.ascx"><Parameters><Parameter key="title" value="Attention 1 !" /><Parameter key="text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
    [@P[userControl:<Data id="banner2" location="~/controls/banner.ascx"><Parameters><Parameter key="title" value="Attention 2 !" /><Parameter key="text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
  </div>
  <div>
    [@P[userControl:<Data id="content" location="~/controls/content.ascx" />]@P]
    <hr />
  </div>
  <div>
    [@P[cdsDocument:<Data id="cdstest" app="sales" location="document/location/test" />]@P]
    <hr />
  </div>
  [@P[webControl:<Data id="one" assemblyname="App_Code" location="WebControls.WebControlOne"><Parameters><Parameter key="text" value="Web control one" /></Parameters></Data>]@P]
  <hr />
  [@P[webControl:<Data id="two" assemblyname="App_Code" location="WebControls.WebControlTwo"><Parameters><Parameter key="text" value="Web control two" /></Parameters></Data>]@P]
  <hr />
  [@P[webControl:<Data id="button" assemblyname="System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" location="System.Web.UI.WebControls.Button"></Data>]@P]
  <hr />
  <br />
  <sup><asp:label runat="server" ID="temp"></asp:label></sup>
</body>
</html>
