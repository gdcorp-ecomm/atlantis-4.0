using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaReloadingConfiguration : ICaptchaReloadingConfiguration
    {
        // singleton
        private CaptchaReloadingConfiguration()
        {
        }

        private static readonly CaptchaReloadingConfiguration _instance = new CaptchaReloadingConfiguration();

        public static CaptchaReloadingConfiguration Instance
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
                bool enabled = CaptchaDefaults.ReloadingEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.Enabled;
                    enabled = configuredFlag;
                } 

                return enabled;
            }
        }

        public ICaptchaReloadIconConfiguration ReloadIcon
        {
            get
            {
                return ReloadIconConfiguration.Instance as ICaptchaReloadIconConfiguration;
            }
        }

        public IAutoReloadExpiredCaptchasConfiguration AutoReloadExpiredCaptchas
        {
            get
            {
                return AutoReloadExpiredCaptchasConfiguration.Instance as IAutoReloadExpiredCaptchasConfiguration;
            }
        }

    }

    public class ReloadIconConfiguration : ICaptchaReloadIconConfiguration
    {
        // singleton
        private ReloadIconConfiguration()
        {
        }

        private static readonly ReloadIconConfiguration _instance = new ReloadIconConfiguration();

        public static ReloadIconConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }
       
        public String FilePath
        {
            get
            {
                string filePath = CaptchaDefaults.ReloadIconPath;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon)
                {
                    string configuredPath = CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon.FilePath;
                    if (StringHelper.HasValue(configuredPath))
                    {
                        filePath = configuredPath;
                    }
                }

                return filePath;
            }
        }

        public Int32 IconWidth
        {
            get
            {
                Int32 iconWidth = CaptchaDefaults.ReloadIconWidth;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon)
                {
                    Int32 configuredWidth = CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon.IconWidth;
                    iconWidth = configuredWidth;
                }

                return iconWidth;
            }
        }

        public ILocalizedStringConfiguration Tooltip
        {
            get
            {
                return ReloadIconTooltipConfiguration.Instance as ILocalizedStringConfiguration;
            }
        }
    }

    public class ReloadIconTooltipConfiguration : ILocalizedStringConfiguration
    {
        // singleton
        private ReloadIconTooltipConfiguration()
        {
        }

        private static readonly ReloadIconTooltipConfiguration _instance = new ReloadIconTooltipConfiguration();

        public static ReloadIconTooltipConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public string this[String locale]
        {
            get
            {
                string tooltip = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon.Tooltip)
                {
                    string configuredTooltip = CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.ReloadIcon.Tooltip[locale];
                    if (StringHelper.HasValue(configuredTooltip))
                    {
                        tooltip = configuredTooltip;
                    }
                }

                return tooltip;
            }
        }
    }

    public class AutoReloadExpiredCaptchasConfiguration : IAutoReloadExpiredCaptchasConfiguration
    {
        // singleton
        private AutoReloadExpiredCaptchasConfiguration()
        {
        }

        private static readonly AutoReloadExpiredCaptchasConfiguration _instance = new AutoReloadExpiredCaptchasConfiguration();

        public static AutoReloadExpiredCaptchasConfiguration Instance
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
                bool enabled = CaptchaDefaults.AutoReloadExpiredCaptchas;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                   null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading &&
                   null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.AutoReloadExpiredCaptchas)
                {
                    bool configuredFlag = 
                        CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.AutoReloadExpiredCaptchas.Enabled;

                    enabled = configuredFlag;
                }

                // disabling reloading in general disables auto-reloading as well
                return CaptchaReloadingConfiguration.Instance.Enabled && enabled;
            }
        }

        public int Timeout
        {
            get
            {
                int timeout = CaptchaDefaults.AutoReloadTimeout;

                // use the configured timeout value
                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.AutoReloadExpiredCaptchas)
                {
                    int configuredTimeout = 
                        CaptchaConfiguration.ConfigurationProvider.CaptchaReloading.AutoReloadExpiredCaptchas.Timeout;

                    timeout = configuredTimeout;
                }

                return timeout;
            }
        }
    }
}
