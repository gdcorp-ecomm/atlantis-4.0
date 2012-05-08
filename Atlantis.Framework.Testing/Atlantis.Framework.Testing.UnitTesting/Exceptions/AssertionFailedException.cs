using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for AssertionFailedException
  /// </summary>
  [Serializable]
  public class AssertionFailedException : Exception
  {
    
    public AssertionFailedException() : base()
    {
    }

    public AssertionFailedException(string message) : base(message)
    {
    }

    public AssertionFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }

  }
}