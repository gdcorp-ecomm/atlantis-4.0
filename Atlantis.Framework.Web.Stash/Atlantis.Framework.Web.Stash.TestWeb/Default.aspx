<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <atlantis:StashRenderLocation runat="server" ID="headRender" Location="headStash" />
</head>
<body>
    <div class="cool">
        <atlantis:StashContent ID="scriptTest" runat="server" Location="bottomStash">
            <script type="text/javascript">
                function DoSomething() {
                    alert('hello world!');
                }
            </script>
        </atlantis:StashContent>
        Hello World Test Text!<br />
    </div>
    <div>
        <atlantis:StashContent ID="cssTest" runat="server" Location="headStash" StashBeforeRender="true">
            <style type="text/css">
                .cool { background-color:Aqua; }
            </style>
        </atlantis:StashContent>
    </div>
    <atlantis:StashRenderLocation runat="server" ID="bottomRender" Location="bottomStash" />
        <atlantis:StashContent ID="StashContent1" runat="server" Location="headStash" StashBeforeRender="true" RenderKey="reusableButton">
            <style type="text/css">
                .cool2 { background-color:Aqua; }
            </style>
        </atlantis:StashContent>
        <atlantis:StashContent ID="StashContent2" runat="server" Location="headStash" StashBeforeRender="true" RenderKey="reusableButton">
            <style type="text/css">
                .cool2 { background-color:Aqua; }
            </style>
        </atlantis:StashContent>
</body>
</html>
