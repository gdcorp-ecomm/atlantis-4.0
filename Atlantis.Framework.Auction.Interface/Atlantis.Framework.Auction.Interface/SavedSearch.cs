using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface {
  [DataContract]
  [Serializable]
  public class SavedSearch 
  {
    public SavedSearch(string searchName )
    {
      SearchName = searchName;
    }
    [DataMember]
    public string SearchName { get; private set; }

    public override string ToString()
    {
      return SearchName;
    }
  }
}