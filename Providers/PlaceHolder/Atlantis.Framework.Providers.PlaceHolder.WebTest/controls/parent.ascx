<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="parent.ascx.cs" Inherits="Atlantis.Framework.Providers.PlaceHolder.WebTest.controls.parent" %>
<%@ Register src="~/controls/child1.ascx" tagPrefix ="child" tagName="child1" %>
<%@ Register src="~/controls/child2.ascx" tagPrefix ="child" tagName="child2" %>
<child:child1 runat="server" />
<child:child2 runat="server" />