using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IWebControlPlaceHolderData : IPlaceHolderData
  {
    string AssemblyName { get; }
  }
}
