using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class SubLocation
  {
    public string Name { get; set; }
    public List<SelectedOfferLite> SelectedOffers { get; set; }
  }
}
