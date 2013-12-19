<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  [@P[cdsDocument:<data app="atlantis" location="_global/csslink" />]@P]
</head>
<body>
  <div>
    [@P[userControl:<data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></data>]@P]
    <hr />
    [@P[userControl:<data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention 1!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></data>]@P]
    <hr />
    [@P[userControl:<data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention 2!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></data>]@P]
    <hr />
    [@P[userControl:<data location="~/controls/banner2.ascx" />]@P]
    <hr />
  </div>
  <div>
    [@P[userControl:<data location="~/controls/content.ascx" />]@P]
    <hr />
  </div>
   <div>
    [@P[userControl:<data location="~/controls/parent.ascx" />]@P]
    <hr />
  </div>
  <div>
    [@P[cdsDocument:<data app="atlantis" location="_global/banner" />]@P]
    <hr />
  </div>
  [@P[webControl:<data assembly="App_Code" type="WebControls.WebControlOne"><Parameters><Parameter key="Text" value="Web control one" /></Parameters></data>]@P]
  <hr />
  [@P[webControl:<data assembly="App_Code" type="WebControls.WebControlTwo"><Parameters><Parameter key="Text" value="Web control two" /></Parameters></data>]@P]
  <hr />
  Circular reference cdsDocument placeholder SHOULD NOT render below:
  <%--[@P[cdsDocument:<data app="atlantis" location="_global/circularreference" />]@P]--%>
</body>
</html>
