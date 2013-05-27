using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  internal class UrlData : IUrlData
  {
    #region IUrlData Members

    [JsonProperty("style")]
    public string Style { get; set; }

    #endregion
  }
}
