using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace BotDetect.Web
{
    public enum CaptchaHttpCommand
    {
        Unknown = 0,
        GetImage,
        GetSound,
        GetValidationResult,
        GetSoundIcon,
        GetSmallSoundIcon,
        GetDisabledSoundIcon,
        GetSmallDisabledSoundIcon,
        GetReloadIcon,
        GetSmallReloadIcon,
        GetDisabledReloadIcon,
        GetSmallDisabledReloadIcon,
        GetLayoutStyleSheet,
        GetClientScriptInclude,
        GetSessionTroubleshootingReport
    }

    public sealed class CaptchaCommandHelper
    {
        private CaptchaCommandHelper()
        {
        }

        internal static string GetQuerystring(CaptchaHttpCommand command)
        {
            string querystringCommand = "";

            switch (command)
            {
                case CaptchaHttpCommand.GetImage:
                    querystringCommand = "?get=image";
                    break;

                case CaptchaHttpCommand.GetSound:
                    querystringCommand = "?get=sound";
                    break;

                case CaptchaHttpCommand.GetValidationResult:
                    querystringCommand = "?get=validationResult";
                    break;

                case CaptchaHttpCommand.GetSoundIcon:
                    querystringCommand = "?get=SoundIcon";
                    break;

                case CaptchaHttpCommand.GetSmallSoundIcon:
                    querystringCommand = "?get=SmallSoundIcon";
                    break;

                case CaptchaHttpCommand.GetDisabledSoundIcon:
                    querystringCommand = "?get=DisabledSoundIcon";
                    break;

                case CaptchaHttpCommand.GetSmallDisabledSoundIcon:
                    querystringCommand = "?get=SmallDisabledSoundIcon";
                    break;

                case CaptchaHttpCommand.GetReloadIcon:
                    querystringCommand = "?get=ReloadIcon";
                    break;

                case CaptchaHttpCommand.GetSmallReloadIcon:
                    querystringCommand = "?get=SmallReloadIcon";
                    break;

                case CaptchaHttpCommand.GetDisabledReloadIcon:
                    querystringCommand = "?get=DisabledReloadIcon";
                    break;

                case CaptchaHttpCommand.GetSmallDisabledReloadIcon:
                    querystringCommand = "?get=SmallDisabledReloadIcon";
                    break;

                case CaptchaHttpCommand.GetLayoutStyleSheet:
                    querystringCommand = "?get=layoutStyleSheet";
                    break;

                case CaptchaHttpCommand.GetClientScriptInclude:
                    querystringCommand = "?get=clientScriptInclude";
                    break;

                case CaptchaHttpCommand.GetSessionTroubleshootingReport:
                    querystringCommand = "?get=sessionTroubleshootingReport";
                    break;

                default:
                    throw new CaptchaHttpException("Unknown CaptchaHttpCommand value", command);
            }

            return querystringCommand;
        }

        internal static CaptchaHttpCommand GetCaptchaCommand(HttpContext context)
        {
            CaptchaHttpCommand command = CaptchaHttpCommand.Unknown;

            string commandString = context.Request.QueryString["get"] as string;
            if (StringHelper.HasValue(commandString))
            {
                switch (commandString.ToLowerInvariant())
                {
                    case "image":
                        command = CaptchaHttpCommand.GetImage;
                        break;
                    case "sound":
                        command = CaptchaHttpCommand.GetSound;
                        break;
                    case "validationresult":
                        command = CaptchaHttpCommand.GetValidationResult;
                        break;
                    case "soundicon":
                        command = CaptchaHttpCommand.GetSoundIcon;
                        break;
                    case "smallsoundicon":
                        command = CaptchaHttpCommand.GetSmallSoundIcon;
                        break;
                    case "disabledsoundicon":
                        command = CaptchaHttpCommand.GetDisabledSoundIcon;
                        break;
                    case "smalldisabledsoundicon":
                        command = CaptchaHttpCommand.GetSmallDisabledSoundIcon;
                        break;
                    case "reloadicon":
                        command = CaptchaHttpCommand.GetReloadIcon;
                        break;
                    case "smallreloadicon":
                        command = CaptchaHttpCommand.GetSmallReloadIcon;
                        break;
                    case "disabledreloadicon":
                        command = CaptchaHttpCommand.GetDisabledReloadIcon;
                        break;
                    case "smalldisabledreloadicon":
                        command = CaptchaHttpCommand.GetSmallDisabledReloadIcon;
                        break;
                    case "layoutstylesheet":
                        command = CaptchaHttpCommand.GetLayoutStyleSheet;
                        break;
                    case "clientscriptinclude":
                        command = CaptchaHttpCommand.GetClientScriptInclude;
                        break;
                    case "sessiontroubleshootingreport":
                        command = CaptchaHttpCommand.GetSessionTroubleshootingReport;
                        break;
                    default:
                        break;
                }
            }

            return command;
        }
    }
}
