using System;
using System.IO;
using System.Reflection;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.RazorEngine.Compilation;

namespace Atlantis.Framework.RazorEngine
{
  internal class AssemblyFileManager
  {
    private const string ASSEMBLY_FILE_NAME_FORMAT = "Atlantis.Framework.RazorEngine\\bin\\razortemplate{0}.dll";
    private const string TEMP_ASSEMBLY_FILE_FORMAT = "Atlantis.Framework.RazorEngine\\bin\\_temp\\temp{0}.dll";

    private const int BUFFER_SIZE = 16 * 1024; // 16 KB buffer

    private static readonly SlimLock _slimLock = new SlimLock();

    private byte[] ReadAssembly(FileStream assemblyFileStream)
    {
      byte[] assemblyBytes;

      byte[] buffer = new byte[BUFFER_SIZE];

      try
      {
        using (MemoryStream ms = new MemoryStream())
        {
          int read;
          while ((read = assemblyFileStream.Read(buffer, 0, buffer.Length)) > 0)
          {
            ms.Write(buffer, 0, read);
          }

          assemblyBytes = ms.ToArray();
        }
      }
      catch
      {
        assemblyBytes = new byte[0];
        // TODO: Log silent
      }

      return assemblyBytes;
    }

    internal string GetAssemblyFilePath(TypeContext context)
    {
      return Path.Combine(context.SaveLocation, string.Format(ASSEMBLY_FILE_NAME_FORMAT, context.TemplateKey));
    }

    internal string GetTempAssemblyCompilePath(TypeContext context)
    {
      return Path.Combine(context.SaveLocation, string.Format(TEMP_ASSEMBLY_FILE_FORMAT, Guid.NewGuid().ToString("N")));
    }

    internal void SetupCompileDirectory(TypeContext context)
    {
      bool compileDirectoryExists;
      string compileDirectory = Path.GetDirectoryName(GetTempAssemblyCompilePath(context));

      using(_slimLock.GetReadLock())
      {
        compileDirectoryExists = Directory.Exists(compileDirectory);
      }
      
      if(!compileDirectoryExists)
      {
        using(_slimLock.GetWriteLock())
        {
          if(!Directory.Exists(compileDirectory))
          {
            Directory.CreateDirectory(compileDirectory);
          }
        }
      }
    }

    internal void SaveAssemblyToDisk(string tempAssemblyPath, TypeContext context)
    {
      byte[] assemblyBytes;

      using (FileStream tempAssemblyFileStream = new FileStream(tempAssemblyPath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        assemblyBytes = ReadAssembly(tempAssemblyFileStream);
      }

      /*
      using (MemoryStream stream = new MemoryStream())
      {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, assembly);

        assemblyBytes = stream.ToArray();
      }
      */

      if(assemblyBytes.Length > 0)
      {
        using(FileStream fileStream = new FileStream(GetAssemblyFilePath(context), FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
          fileStream.Write(assemblyBytes, 0, assemblyBytes.Length);
        }
      }
    }

    internal AssemblyLoadResult LoadAssembly(TypeContext context, out Assembly assembly)
    {
      AssemblyLoadResult result = AssemblyLoadResult.Error;
      assembly = null;

      if (!string.IsNullOrEmpty(context.SaveLocation) && !string.IsNullOrEmpty(context.TemplateKey))
      {
        try
        {
          string assemblyFilePath = Path.Combine(context.SaveLocation, string.Format(ASSEMBLY_FILE_NAME_FORMAT, context.TemplateKey));
          string directory = Path.GetDirectoryName(assemblyFilePath);

          bool directoryExists;
          using(_slimLock.GetReadLock())
          {
            directoryExists = Directory.Exists(directory);
          }

          if (!directoryExists)
          {
            using(_slimLock.GetWriteLock())
            {
              if (!Directory.Exists(directory))
              {
                Directory.CreateDirectory(directory);
                result = AssemblyLoadResult.NotFound;
              }
            }
          }

          if(result != AssemblyLoadResult.NotFound)
          {
            using (_slimLock.GetReadLock())
            {
              if (File.Exists(assemblyFilePath))
              {
                //assembly = Assembly.LoadFile(assemblyFilePath);
                
                using (FileStream fileStream = new FileStream(assemblyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                  byte[] assemblyBytes = ReadAssembly(fileStream);
                  if (assemblyBytes.Length > 0)
                  {
                    assembly = Assembly.Load(assemblyBytes);
                  }
                }
                
                result = AssemblyLoadResult.Success;
              }
              else
              {
                result = AssemblyLoadResult.NotFound;
              }
            }
          }
        }
        catch
        {
          assembly = null;
        }
      }

      return result;
    }
  }
}
