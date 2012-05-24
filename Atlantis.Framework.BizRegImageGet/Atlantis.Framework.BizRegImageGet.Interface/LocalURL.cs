using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegImageGet.Interface
{
  public class LocalURL
  {
    private string imageURLValue;

    public string ImageURL
    {
      get { return imageURLValue; }
      set { imageURLValue = value; }
    }
  }
}
