<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Captcha.ascx.cs" Inherits="Shared_Captcha_Captcha" %>
<atlantis:CaptchaTemplateControl ID="captchaTemplate1" runat="server" CaptchaImagePlaceHolderID="plcCaptchaLocation" StashRenderLocation="javascriptStash" AjaxAuthentication="false" CaptchaValidateSelector=".captchaValidate1" AutoClearInput="true" AutoFocusInput="true">
    <Template>
        <div style="width: 500px;">
            <div style="float: left; padding-bottom: 10px;">
                <asp:PlaceHolder ID="plcCaptchaLocation" runat="server"></asp:PlaceHolder>
            </div>
            <div style="padding-left: 10px; float: left;">
                <div class="captcha_button" style="padding-bottom:5px;" id="speak_code_<%=InstanceID %>">
                    <div class="g-btn-lg g-btn-prg" style="width: 100px;">Speak Code</div>
                </div>
                <div class="captcha_button" id="change_code_<%=InstanceID %>">
                    <div class="g-btn-lg g-btn-prg" style="width: 100px;">Change Code</div>
                </div>
            </div>
            <div style="clear: both;">
                <asp:PlaceHolder ID="plcCaptchaLable" runat="server">
                    <div style="padding-top: 15px;" class="cboth">
                        <strong><%=CaptchaLable %></strong>
                    </div>
                </asp:PlaceHolder>
                <input type="text" name="cart_captcha_value_<%=InstanceID %>" id="cart_captcha_value_<%=InstanceID %>" maxlength="90" style="width: 180px;" />
            </div>

        </div>
    </Template>
</atlantis:CaptchaTemplateControl>
