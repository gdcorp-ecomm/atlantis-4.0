<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  [@P[cdsDocument:<Data app="atlantis" location="_global/csslink" />]@P]
</head>
<body>
  <div>
    [@P[userControl:<Data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
    [@P[userControl:<Data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention 1!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
    [@P[userControl:<Data location="~/controls/banner.ascx"><Parameters><Parameter key="Title" value="Attention 2!" /><Parameter key="Text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
    <hr />
    [@P[userControl:<Data location="~/controls/banner2.ascx" />]@P]
    <hr />
  </div>
  <div>
    [@P[userControl:<Data location="~/controls/content.ascx" />]@P]
    <hr />
  </div>
   <div>
    [@P[userControl:<Data location="~/controls/parent.ascx" />]@P]
    <hr />
  </div>
  <div>
    [@P[cdsDocument:<Data app="atlantis" location="_global/banner" />]@P]
    <hr />
  </div>
  [@P[webControl:<Data assembly="App_Code" type="WebControls.WebControlOne"><Parameters><Parameter key="Text" value="Web control one" /></Parameters></Data>]@P]
  <hr />
  [@P[webControl:<Data assembly="App_Code" type="WebControls.WebControlTwo"><Parameters><Parameter key="Text" value="Web control two" /></Parameters></Data>]@P]
  <hr />
  [@P[cdsDocument:<Data app="atlantis" location="_global/circularreference" />]@P]
  <hr />
</body>
</html>
