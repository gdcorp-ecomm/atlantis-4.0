<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <div>
    [@P[userControl:<Data location="~/controls/banner.ascx"><Parameters><Parameter key="title" value="Attention!" /><Parameter key="text" value="Scheduled maintenance underway." /></Parameters></Data>]@P]
  </div>
  <div>
    [@P[userControl:<Data location="~/controls/content.ascx" />]@P]
  </div>
   <div>
    [@P[cdsDocument:<Data app="sales" location="document/location/test" />]@P]
  </div>
</body>
</html>
