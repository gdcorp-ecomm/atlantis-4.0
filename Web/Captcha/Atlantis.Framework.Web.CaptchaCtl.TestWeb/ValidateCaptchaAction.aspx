<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValidateCaptchaAction.aspx.cs" Inherits="ValidateCaptchaAction" %>
<%@ Register Src="~/UserControl/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>
 
<uc1:Captcha ID="captcha1" runat="server" Visible="false" InstanceID="customAjaxValidation" SaveValidationDataInSession="true" />