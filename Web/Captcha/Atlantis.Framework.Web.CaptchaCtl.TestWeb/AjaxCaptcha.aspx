<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AjaxCaptcha.aspx.cs" Inherits="AjaxCaptcha" %>

<%@ Register TagPrefix="shared" Namespace="Atlantis.Framework.Web.CaptchaCtl" %>
<%@ Register Src="~/UserControl/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://img3.wsimg-com.ide/shared/js/1.8.3/global.20130605.min.js" type="text/javascript"></script>
    <script src="_ImageServer/script/bot_detect_20130724.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="http://img2.wsimg-com.ide/shared/css/1/styles_20120926.min.css" />
</head>
<body>
    <div style="padding-left: 100px;">
        <div style="height: 30px; padding-top: 10px;">
            <h1>Standard Ajax Validation with 10 second auto reload</h1>
        </div>
        <atlantis:CaptchaTemplateControl ID="captchaTemplate1" runat="server" CaptchaImagePlaceHolderID="plcCaptchaLocation" StashRenderLocation="javascriptStash" CaptchaValidateSelector=".captchaValidate" CaptchaValueID="captchaTextBox" ImageReloadPlaceHolderID="plcImageReload" PlaySoundPlaceHolderID="plcPlaySound" CaptchaID="standardAjaxValidation" AjaxAuthentication="true" AjaxAuthenticationCallback="AjaxAuthenticationResults" AutoClearInput="true" AutoFocusInput="false" LoadingText="Loading..." AutoReloadImage="true">
            <Template>
                <div style="border: solid; border-color: black; border-width: 1px; width: 400px; padding: 20px;">
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
                </div>
            </Template>
        </atlantis:CaptchaTemplateControl>
        <div style="height: 30px; padding-top: 10px;">
            <h1>Standard Ajax Validation 2nd Instance of captcha on same page</h1>
        </div>
        <atlantis:CaptchaTemplateControl ID="CaptchaTemplateControl1" runat="server" CaptchaImagePlaceHolderID="plcCaptchaLocation2" StashRenderLocation="javascriptStash" CaptchaValidateSelector=".captchaValidate2" CaptchaValueID="captchaTextBox2" ImageReloadPlaceHolderID="plcImageReload2" PlaySoundPlaceHolderID="plcPlaySound2" CaptchaID="standardAjaxValidation2" AjaxAuthentication="true" AjaxAuthenticationCallback="AjaxAuthenticationResults2" AutoClearInput="true" AutoFocusInput="true" LoadingImagePlaceHolderID="plcLoadingUI" LoadingImageContainerPlaceHolderID="plcLoadingUIContainer">
            <Template>
                <div style="border: solid; border-color: black; border-width: 1px; width: 400px; padding: 20px;">
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
                    <asp:PlaceHolder ID="plcLoadingUIContainer" runat="server">
                        <span id="plcLoadingUI" style="display: none;">Hello there I'm loading......
                        </span>
                    </asp:PlaceHolder>
                </div>
            </Template>
        </atlantis:CaptchaTemplateControl>
        <div style="height: 30px; padding-top: 10px;">
            <h1>Custom Ajax Validation 3nd Instance of captcha on same page - implemented in a user control</h1>
        </div>
        <div>
            <div style="border: solid; border-color: black; border-width: 1px; width: 500px; padding: 20px;">
                <uc1:Captcha ID="captcha1" runat="server" InstanceID="customAjaxValidation" SaveValidationDataInSession="true" />
            </div>
        </div>
        <div style="height: 30px; padding-top: 10px;">
            <h1>Custom Ajax Validation 4th Instance of captcha on same page - implemented in template control</h1>
        </div>
        <atlantis:CaptchaTemplateControl ID="CaptchaTemplateControl2" runat="server" CaptchaID="instance4" SaveValidationDataInSession="true" CaptchaImagePlaceHolderID="plcCaptchaLocation4" ReloadLinkID="change_code_instance4" PlaySoundLinkID="speak_code_instance4" StashRenderLocation="javascriptStash" CaptchaValueID="cart_captcha_value_instance4" AutoClearInput="true" AutoFocusInput="true">
            <Template>
                <div style="border: solid; border-color: black; border-width: 1px; width: 500px; padding: 20px;">
                    <div style="width: 500px;">
                        <div style="float: left; padding-bottom: 10px;">
                            <asp:PlaceHolder ID="plcCaptchaLocation4" runat="server"></asp:PlaceHolder>
                        </div>
                        <div style="padding-left: 10px; float: left;">
                            <div class="captcha_button" style="padding-bottom: 5px;" id="speak_code_instance4">
                                <div class="g-btn-lg g-btn-prg" style="width: 100px;">Speak Code</div>
                            </div>
                            <div class="captcha_button" id="change_code_instance4">
                                <div class="g-btn-lg g-btn-prg" style="width: 100px;">Change Code</div>
                            </div>
                        </div>
                        <div style="clear: both;">
                            <asp:PlaceHolder ID="plcCaptchaLable" runat="server">
                                <div style="padding-top: 15px;" class="cboth">
                                    <strong>Add Captcha here</strong>
                                </div>
                            </asp:PlaceHolder>
                            <input type="text" name="cart_captcha_value_instance4" id="cart_captcha_value_instance4" maxlength="90" style="width: 180px;" />
                        </div>
                    </div>
                </div>
            </Template>
        </atlantis:CaptchaTemplateControl>
        <div class="g-buttonpane">
            <span id="validateCaptcha4" class="g-btn-lg g-btn-prg">Validate Captcha</span>
        </div>

        <div style="height: 30px; padding-top: 10px;">
            <div>
                <h1>Default template - no code/html required</h1>
            </div>
            <div style="border: solid; border-color: black; border-width: 1px; width: 500px; padding: 20px;">
                <atlantis:CaptchaTemplateControl ID="CaptchaTemplateControl3" runat="server" CaptchaID="instance5" StashRenderLocation="javascriptStash">
                </atlantis:CaptchaTemplateControl>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function CaptchaDefaultAthentication(result) {
            $('#defaultcaptchaError').hide();
            if (!result) {
                $('#defaultcaptchaError').show();
            }
            else {
                alert(result);
            }
        }
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
            $('#g-validate-captcha').bind('click.captcha', function () {
                var captchaValue = $('#cart_captcha_value_customAjaxValidation').val();
                var extraData = $('#extraData').val();
                $.ajax({
                    dataType: "json",
                    url: '<%=ValidateCaptchaURL%>',
                    data: {
                        extraInfo: extraData,
                        captchaValue: captchaValue,
                        instanceID: 'customAjaxValidation'
                    },
                    success: isValid
                });
            });
        });
        $(document).ready(function () {
            $('#validateCaptcha4').bind('click.captcha', function () {
                var captchaValue = $('#cart_captcha_value_instance4').val();
                $.ajax({
                    dataType: "json",
                    url: '<%=ValidateCaptchaURL%>',
                    data: {
                        captchaValue: captchaValue,
                        instanceID: 'instance4'
                    },
                    success: isValid2
                });
            });
        });
        function isValid2(result) {
            alert(result.Valid);
            if (!result.Valid) {
                $('#cart_captcha_value_instance4').trigger('captchaReload');
            }
        }
        function isValid(result) {
            alert(result.Valid);
            if (!result.Valid) {
                $('#cart_captcha_value_customAjaxValidation').trigger('captchaReload');
            }
            else {
                alert('data validated:' + result.ExtraData);
            }
        }
    </script>

    <atlantis:StashRenderLocation ID="StashRenderLocation1" Location="javascriptStash" runat="server">
    </atlantis:StashRenderLocation>

</body>
</html>
