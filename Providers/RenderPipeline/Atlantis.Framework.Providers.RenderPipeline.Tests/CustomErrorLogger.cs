using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests
{
  public class CustomErrorLogger : IErrorLogger
  {
    HashSet<string> _sourceFunctions = new HashSet<string>();

    void IErrorLogger.LogAtlantisException(AtlantisException atlantisException)
    {
      _sourceFunctions.Add(atlantisException.ToString());
    }

    public int Count
    {
      get
      {
        return _sourceFunctions.Count;
      }
    }

    public string FirstException
    {
      get 
      {
        return _sourceFunctions.FirstOrDefault();
      }
    }
  }
}
