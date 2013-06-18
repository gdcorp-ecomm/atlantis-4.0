using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    /// <summary>
    /// SoundPackages can have different format versions in the future
    /// </summary>
    internal enum SoundPackageFormat
    {
        // dots are not valid separators in identifier names
        V300000000 = 0 // 3 digits per number = 3.0.0.0
    }
}
