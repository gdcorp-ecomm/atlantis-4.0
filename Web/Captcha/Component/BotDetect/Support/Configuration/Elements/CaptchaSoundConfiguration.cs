using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace BotDetect.Configuration
{
    public class CaptchaSoundConfiguration : ICaptchaSoundConfiguration
    {
        // singleton
        private CaptchaSoundConfiguration()
        {
        }

        private static readonly CaptchaSoundConfiguration _instance = new CaptchaSoundConfiguration();

        public static CaptchaSoundConfiguration Instance
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
                bool enabled = CaptchaDefaults.SoundEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.Enabled;
                    enabled = configuredFlag;
                } 

                return enabled;
            }
        }

        public int StartDelay
        {
            get
            {
                int startDelay = CaptchaDefaults.SoundStartDelay;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound)
                {
                    int configuredValue = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.StartDelay;
                    startDelay = configuredValue;
                }

                return startDelay;
            }
        }

        public ICaptchaSoundIconConfiguration SoundIcon
        {
            get
            {
                return CaptchaSoundIconConfiguration.Instance;
            }
        }

        public IDisabledSoundStylesConfiguration DisabledSoundStyles
        {
            get
            {
                return DisabledSoundStylesConfiguration.Instance as IDisabledSoundStylesConfiguration;
            }
        }

        public ISoundPackagesConfiguration SoundPackages
        {
            get
            {
                return SoundPackagesConfiguration.Instance;
            }
        }
    }

    public class CaptchaSoundIconConfiguration : ICaptchaSoundIconConfiguration
    {
        // singleton
        private CaptchaSoundIconConfiguration()
        {
        }

        private static readonly CaptchaSoundIconConfiguration _instance = new CaptchaSoundIconConfiguration();

        public static CaptchaSoundIconConfiguration Instance
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
                string filePath = CaptchaDefaults.SoundIconPath;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon)
                {
                    string configuredPath = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon.FilePath;
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
                Int32 iconWidth = CaptchaDefaults.SoundIconWidth;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon)
                {
                    Int32 configuredWidth = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon.IconWidth;
                    iconWidth = configuredWidth;
                }

                return iconWidth;
            }
        }

        public ILocalizedStringConfiguration Tooltip
        {
            get
            {
                return SoundIconTooltipConfiguration.Instance as ILocalizedStringConfiguration;
            }
        }

        public ISoundPackagesConfiguration SoundPackages
        {
            get
            {
                return SoundPackagesConfiguration.Instance as ISoundPackagesConfiguration;
            }
        }
    }

    public class SoundIconTooltipConfiguration : ILocalizedStringConfiguration
    {
        // singleton
        private SoundIconTooltipConfiguration()
        {
        }

        private static readonly SoundIconTooltipConfiguration _instance = new SoundIconTooltipConfiguration();

        public static SoundIconTooltipConfiguration Instance
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
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon.Tooltip)
                {
                    string configuredTooltip = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundIcon.Tooltip[locale];
                    if (StringHelper.HasValue(configuredTooltip))
                    {
                        tooltip = configuredTooltip;
                    }
                }

                return tooltip;
            }
        }
    }

    public class SoundPackagesConfiguration : ISoundPackagesConfiguration
    {
        // singleton
        private SoundPackagesConfiguration()
        {
        }

        private static readonly SoundPackagesConfiguration _instance = new SoundPackagesConfiguration();

        public static SoundPackagesConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public String FolderPath
        {
            get
            {
                string folderPath = CaptchaDefaults.SoundPackagesFolder;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundPackages)
                {
                    string configuredPath = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundPackages.FolderPath;

                    if (StringHelper.HasValue(configuredPath))
                    {
                        folderPath = configuredPath;
                    }
                }

                return folderPath;
            }
        }

        public bool WarnAboutMissingSoundPackages
        {
            get
            {
                bool warn = CaptchaDefaults.WarnAboutMissingSoundPackages;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.SoundPackages.WarnAboutMissingSoundPackages;
                    warn = configuredFlag;
                }

                return warn;
            }
        }
    }


    public class DisabledSoundStylesConfiguration : IDisabledSoundStylesConfiguration
    {
        // singleton
        private DisabledSoundStylesConfiguration()
        {
        }

        private static readonly DisabledSoundStylesConfiguration _instance = new DisabledSoundStylesConfiguration();

        public static DisabledSoundStylesConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public Set<SoundStyle> Styles
        {
            get
            {
                Set<SoundStyle> styles = new Set<SoundStyle>();

                StringCollection names = this.Names;
                if (null != names)
                {
                    foreach (string name in names)
                    {
                        try
                        {
                            object SoundStyle = Enum.Parse(typeof(BotDetect.SoundStyle), name, true);
                            if (null != SoundStyle)
                            {
                                styles.Add((BotDetect.SoundStyle)SoundStyle);
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
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSound.DisabledSoundStyles)
                {
                    StringCollection configuredCollection = CaptchaConfiguration.ConfigurationProvider.CaptchaSound.DisabledSoundStyles.Names;
                    collection = configuredCollection;
                }

                return collection;
            }
        }
    }
}
