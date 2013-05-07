<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.Default" %>
<%@ Import Namespace="Atlantis.Framework.Providers.PlaceHolder.Interface" %>
<%@ Import Namespace="Atlantis.Framework.Providers.PlaceHolder.WebTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <div>
    <%= ProviderContainerHelper.Instance.Resolve<IPlaceHolderProvider>().ReplacePlaceHolders(@"[@P[userControl:<Data location=""~/controls/banner.ascx""><Parameters><Parameter key=""title"" value=""Attention!"" /><Parameter key=""text"" value=""Scheduled maintenance underway."" /></Parameters></Data>]@P]") %>
  </div>
</body>
</html>
