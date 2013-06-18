using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal interface IAudio
    {
        // members
        SoundFormat Format { get; }
        byte[] HeaderlessData { get; }

        // filesystem io
        bool IsValidSoundFile(FileInfo info);
        void Open(FileInfo info);
        void Save(FileInfo info, SoundFormat format);

        // memory io
        byte[] Bytes { get; }
        MemoryStream GetStream(SoundFormat format);
    }
}
