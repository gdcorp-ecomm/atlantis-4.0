<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AjaxCaptcha.aspx.cs" Inherits="AjaxCaptcha" %>

<%@ Register TagPrefix="shared" Namespace="Atlantis.Framework.Web.CaptchaCtl" %>
<%@ Register Src="~/UserControl/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://img3.wsimg-com.ide/shared/js/1.8.3/global.20130605.min.js" type="text/javascript"></script>
    <script src="http://img3.wsimg-com.ide/AtlantisScripts/Atlantis.Web.Controls/Captcha/bot_detect_20130613.min.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="http://img2.wsimg-com.ide/shared/css/1/styles_20120926.min.css" />
</head>
<body>
    <div style="padding-left: 100px;">
        <div>
            <h1>Standard Ajax Validation</h1>
        </div>
        <atlantis:CaptchaTemplateControl ID="captchaTemplate1" runat="server" CaptchaImagePlaceHolderID="plcCaptchaLocation" StashRenderLocation="javascriptStash" CaptchaValidateSelector=".captchaValidate" CaptchaValueID="captchaTextBox" ImageReloadPlaceHolderID="plcImageReload" PlaySoundPlaceHolderID="plcPlaySound" CaptchaID="standardAjaxValidation" AjaxAuthentication="true" AjaxAuthenticationCallback="AjaxAuthenticationResults" AutoClearInput="true" AutoFocusInput="true">
            <Template>
                <div style="width: 330px;">
                    <div style="float: left; padding-bottom: 10px;">
                        <span style="padding-right: 10px;">Access Code:</span>
                        <asp:PlaceHolder ID="plcCaptchaLocation" runat="server"></asp:PlaceHolder>
                    </div>
                    <div style="float: right; padding-right: 20px; padding-top: 20px;">
                        <asp:PlaceHolder ID="plcImageReload" runat="server"></asp:PlaceHolder>
                        <br />
                        <asp:PlaceHolder ID="plcPlaySound" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
                <div style="clear: both;">
                    <span style="padding-right: 10px;">Please Enter the Access Code:</span><br />
                    <input type="text" name="captchaTextBox" id="captchaTextBox" maxlength="90" style="width: 180px;" />
                    <br />
                    <div id="captchaError" style="display: none; width: 200px; text-align: center; padding-top: 5px;">
                        <span class="bodyText" style="color: #cc0000">
                            <b>Authentication failed.&nbsp;Please try again.</b>
                        </span>
                    </div>
                    <br />
                    <div class="g-btn-lg g-btn-prg captchaValidate" style="border: none; cursor: pointer">
                        Validate Me
                    </div>
                </div>
            </Template>
        </atlantis:CaptchaTemplateControl>
         <div>
            <h1>Standard Ajax Validation 2nd Instance of captcha on same page</h1>
        </div>
        <atlantis:CaptchaTemplateControl ID="CaptchaTemplateControl1" runat="server" CaptchaImagePlaceHolderID="plcCaptchaLocation2" StashRenderLocation="javascriptStash" CaptchaValidateSelector=".captchaValidate2" CaptchaValueID="captchaTextBox2" ImageReloadPlaceHolderID="plcImageReload2" PlaySoundPlaceHolderID="plcPlaySound2" CaptchaID="standardAjaxValidation2" AjaxAuthentication="true" AjaxAuthenticationCallback="AjaxAuthenticationResults2" AutoClearInput="true" AutoFocusInput="true">
            <Template>
                <div style="width: 330px;">
                    <div style="float: left; padding-bottom: 10px;">
                        <span style="padding-right: 10px;">Access Code:</span>
                        <asp:PlaceHolder ID="plcCaptchaLocation2" runat="server"></asp:PlaceHolder>
                    </div>
                    <div style="float: right; padding-right: 20px; padding-top: 20px;">
                        <asp:PlaceHolder ID="plcImageReload2" runat="server"></asp:PlaceHolder>
                        <br />
                        <asp:PlaceHolder ID="plcPlaySound2" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
                <div style="clear: both;">
                    <span style="padding-right: 10px;">Please Enter the Access Code:</span><br />
                    <input type="text" name="captchaTextBox2" id="captchaTextBox2" maxlength="90" style="width: 180px;" />
                    <br />
                    <div id="captchaError2" style="display: none; width: 200px; text-align: center; padding-top: 5px;">
                        <span class="bodyText" style="color: #cc0000">
                            <b>Authentication failed.&nbsp;Please try again.</b>
                        </span>
                    </div>
                    <br />
                    <div class="g-btn-lg g-btn-prg captchaValidate2" style="border: none; cursor: pointer">
                        Validate Me
                    </div>
                </div>
            </Template>
        </atlantis:CaptchaTemplateControl>
        <div>
            <hr />
            <div style="padding-bottom: 10px;">
                <h1>Custom Ajax Validation 3rd Instance of captcha on same page</h1>
            </div>
            <div>
                <span>Add this extra data</span>
                <input id="extraData" name="extraData" />
            </div>
            <br />
            <uc1:Captcha ID="captcha1" runat="server" InstanceID="customAjaxValidation" SaveValidationDataInSession="true" />
            <br />
            <div class="g-buttonpane">
                <span id="g-addGiftCard-Confirm" class="g-btn-lg g-btn-prg">Validate Captcha</span>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function AjaxAuthenticationResults(result) {
            $('#captchaError').hide();
            if (!result) {
                $('#captchaError').show();
            }
            else {
                alert(result);
            }
        }
        function AjaxAuthenticationResults2(result) {
            $('#captchaError2').hide();
            if (!result) {
                $('#captchaError2').show();
            }
            else {
                alert(result);
            }
        }
        $(document).ready(function () {
            $('#g-addGiftCard-Confirm').bind('click.captcha', function () {
                var captchaValue = $('#cart_captcha_value_customAjaxValidation').val();
                $.ajax({
                    dataType: "json",
                    url: '<%=ValidateCaptchaURL%>',
                    data: {
                        captchaValue: captchaValue
                    },
                    success: isValid
                });
            });
        });
        function isValid(result) {
            alert(result.Valid);
            if (!result.Valid) {
               $('#cart_captcha_value_customAjaxValidation').trigger('captchaReload');
            }
        }
    </script>

    <atlantis:StashRenderLocation ID="StashRenderLocation1" Location="javascriptStash" runat="server">
    </atlantis:StashRenderLocation>

</body>
</html>
