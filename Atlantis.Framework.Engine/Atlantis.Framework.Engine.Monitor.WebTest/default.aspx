<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Atlantis.Framework.Engine.Monitor.WebTest._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <div>
    <a href="/_engine/monitor/Stats?responsetype=html">Engine Stats Html</a><br />
    <a href="/_engine/monitor/Stats?responsetype=xml">Engine Stats Xml</a><br />
    <a href="/_engine/monitor/FirewallTest?responsetype=html">Firewall Test Html</a><br />
    <a href="/_engine/monitor/FirewallTest?responsetype=xml">Firewall Test Xml</a><br />
  </div>
  <br />
  <div>
    Request Trace Data:<br />
    <%: RequestTraceStats %>
  </div>
</body>
</html>
