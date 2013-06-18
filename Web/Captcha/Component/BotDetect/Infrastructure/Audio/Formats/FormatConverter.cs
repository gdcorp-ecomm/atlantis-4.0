using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
   internal abstract class FormatConverter : IFormatConverter
   {
       public IAudio Convert(IAudio inputSound, SoundFormat outputFormat)
       {
           byte[] outputHeaderlessData = inputSound.HeaderlessData.Clone() as byte[];
           IFormatConverter formatConverter;

           if (SoundFormat.WavPcm16bit8kHzMono != inputSound.Format)
           {
               // first convert the input to the default format
               formatConverter = FormatConverterFactory.CreateConverter(inputSound.Format);
               outputHeaderlessData = formatConverter.ConvertToWavPcm16bit8kHzMono(inputSound.HeaderlessData);
           }

           if (SoundFormat.WavPcm16bit8kHzMono != outputFormat)
           {
               // all other format converters take sound in the default format as input
               formatConverter = FormatConverterFactory.CreateConverter(outputFormat);
               outputHeaderlessData = formatConverter.ConvertFromWavPcm16bit8kHzMono(outputHeaderlessData);
           }

           IAudio outputSound = Audio.GetAudio(outputHeaderlessData, outputFormat);

           return outputSound;
       }

       public abstract byte[] ConvertToWavPcm16bit8kHzMono(byte[] inputHeaderlessSound);

       public abstract byte[] ConvertFromWavPcm16bit8kHzMono(byte[] inputHeaderlessSound);
   }
}
