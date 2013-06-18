using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace BotDetect
{
    internal sealed class ResourceHelper
    {
        private ResourceHelper() {}

        public static Stream GetResourceStream(string identifier)
        {
            // the name of the resource files
            string filename = BotDetect.AssemblyInfo.AssemblyName + identifier;

            // read raw resource data
            Stream dataStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);

            return dataStream;
        }

        public static byte[] GetResource(string identifier)
        {
            // read raw resource data
            Stream dataStream = GetResourceStream(identifier);
            byte[] dataBuffer = new byte[dataStream.Length];
            dataStream.Read(dataBuffer, 0, (int)dataStream.Length);

            return dataBuffer;
        }

        public static byte[] GetResource(string identifier, out DateTime modified)
        {
            // read raw resource data
            Stream dataStream = GetResourceStream(identifier);
            byte[] dataBuffer = new byte[dataStream.Length];
            dataStream.Read(dataBuffer, 0, (int)dataStream.Length);

            // resource modification time is approximated with the assembly modification time
            try
            {
                modified = File.GetLastWriteTimeUtc(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);
            }
            catch (Exception ex)
            {
                // fall-back option if we fail to get the modification time
                System.Diagnostics.Debug.Assert(false, ex.Message);
                modified = DateTime.UtcNow;
            }

            return dataBuffer;
        }
    }
}
