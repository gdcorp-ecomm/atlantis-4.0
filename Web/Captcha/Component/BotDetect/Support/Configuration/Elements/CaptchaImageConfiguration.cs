using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace BotDetect.Configuration
{
    public class CaptchaImageConfiguration : ICaptchaImageConfiguration
    {
        // singleton
        private CaptchaImageConfiguration()
        {
        }

        private static readonly CaptchaImageConfiguration _instance = new CaptchaImageConfiguration();

        public static CaptchaImageConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public ILocalizedStringConfiguration Tooltip
        {
            get 
            {
                return ImageTooltipConfiguration.Instance as ILocalizedStringConfiguration;
            }
        }

        public IDisabledImageStylesConfiguration DisabledImageStyles
        {
            get
            {
                return DisabledImageStylesConfiguration.Instance as IDisabledImageStylesConfiguration;
            }
        }

        public IHelpLinkConfiguration HelpLink
        {
            get
            {
                return HelpLinkConfiguration.Instance as IHelpLinkConfiguration;
            }
        }
    }

    public class ImageTooltipConfiguration : ILocalizedStringConfiguration
    {
        // singleton
        private ImageTooltipConfiguration()
        {
        }

        private static readonly ImageTooltipConfiguration _instance = new ImageTooltipConfiguration();

        public static ImageTooltipConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public String this[String locale]
        {
            get
            {
                String tooltip = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.Tooltip)
                {
                    String configuredTooltip = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.Tooltip[locale];
                    if (StringHelper.HasValue(configuredTooltip))
                    {
                        tooltip = configuredTooltip;
                    }
                }

                return tooltip;
            }
        }
    }

    public class HelpLinkConfiguration : IHelpLinkConfiguration
    {
        // singleton
        private HelpLinkConfiguration()
        {
        }

        private static readonly HelpLinkConfiguration _instance = new HelpLinkConfiguration();

        public static HelpLinkConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool Enabled
        {
            get
            {
                bool enabled = CaptchaDefaults.HelpLinkEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.Enabled;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }

        public HelpLinkMode Mode
        {
            get
            {
                HelpLinkMode mode = HelpLinkMode.Text;



                mode = HelpLinkMode.Image;


                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink)
                {
                    HelpLinkMode configuredMode = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.Mode;
                    mode = configuredMode;
                }

                return mode;
            }
        }

        public ILocalizedStringConfiguration HelpPage
        {
            get
            {
                return HelpPageConfiguration.Instance as ILocalizedStringConfiguration;
            }
        }

        public ILocalizedStringConfiguration HelpText
        {
            get
            {
                return HelpTextConfiguration.Instance as ILocalizedStringConfiguration;
            }
        }
    }

    public class HelpPageConfiguration : ILocalizedStringConfiguration
    {
        // singleton
        private HelpPageConfiguration()
        {
        }

        private static readonly HelpPageConfiguration _instance = new HelpPageConfiguration();

        public static HelpPageConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public String this[String locale]
        {
            get
            {
                string page = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.HelpPage)
                {
                    string configuredPage = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.HelpPage[locale];
                    if (StringHelper.HasValue(configuredPage))
                    {
                        page = configuredPage;
                    }
                }

                return page;
            }
        }
    }


    public class HelpTextConfiguration : ILocalizedStringConfiguration
    {
        // singleton
        private HelpTextConfiguration()
        {
        }

        private static readonly HelpTextConfiguration _instance = new HelpTextConfiguration();

        public static HelpTextConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public String this[String locale]
        {
            get
            {
                string page = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.HelpText)
                {
                    string configuredPage = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.HelpLink.HelpText[locale];
                    if (StringHelper.HasValue(configuredPage))
                    {
                        page = configuredPage;
                    }
                }

                return page;
            }
        }
    }


    public class DisabledImageStylesConfiguration : IDisabledImageStylesConfiguration
    {
        // singleton
        private DisabledImageStylesConfiguration()
        {
        }

        private static readonly DisabledImageStylesConfiguration _instance = new DisabledImageStylesConfiguration();

        public static DisabledImageStylesConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public Set<ImageStyle> Styles
        {
            get
            {
                Set<ImageStyle> styles = new Set<ImageStyle>();

                StringCollection names = this.Names;
                if (null != names)
                {
                    foreach(string name in names)
                    {
                        try
                        {
                            object imageStyle = Enum.Parse(typeof(BotDetect.ImageStyle), name, true);
                            if (null != imageStyle)
                            {
                                styles.Add((BotDetect.ImageStyle)imageStyle);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // ignore unrecognized values
                            System.Diagnostics.Debug.Assert(false, ex.Message);
                        }
                    }
                }

                return styles;
            }
        }

        public StringCollection Names
        {
            get
            {
                StringCollection collection = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaImage.DisabledImageStyles)
                {
                    StringCollection configuredCollection = CaptchaConfiguration.ConfigurationProvider.CaptchaImage.DisabledImageStyles.Names;
                    collection = configuredCollection;
                }

                return collection;
            }
        }
    }

}
