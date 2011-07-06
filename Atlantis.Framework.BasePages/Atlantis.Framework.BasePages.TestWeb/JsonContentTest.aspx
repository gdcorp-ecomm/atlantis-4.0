<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsonContentTest.aspx.cs" Inherits="Atlantis.Framework.BasePages.TestWeb.JsonContentTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
	<atlantis:JsonContentPlaceholder ID="jsonContent" runat="server">
		<div style="background-color:Green;">
			<b>Hello World!</b>
		</div>
	</atlantis:JsonContentPlaceholder>
</body>
</html>
