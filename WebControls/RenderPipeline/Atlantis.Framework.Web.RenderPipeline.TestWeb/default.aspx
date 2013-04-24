<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Atlantis.Framework.Web.RenderPipeline.TestWeb._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
      <atlantis:RenderPipelineControl ID="pipeline1" runat="server">
        <asp:Literal ID="litA" runat="server"></asp:Literal><br />
        This is some sample content here {{green}}<br />
        <br />
        This some sample code behind here <%=Green %><br />
      </atlantis:RenderPipelineControl>
    </div>
</body>
</html>
