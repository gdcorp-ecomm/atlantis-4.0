using System;
using System.Text;

namespace Atlantis.Framework.RegRegistryPartnersData.Interface
{
  public class TldRegistryData
  {
    public string TldId { get; private set; }
    public string TldName { get; private set; }
    public int Rank { get; private set; }
    public string TldBidSnapshotId { get; private set; }

    public TldRegistryData(string tldId, string tldName, int rank, string tldBidSnapshotId)
    {
      TldId = tldId;
      TldName = tldName;
      Rank = rank;
      TldBidSnapshotId = tldBidSnapshotId;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder(256);
      sb.AppendFormat("{0},{1},{2},{3}", TldId, TldName, Rank.ToString(), TldBidSnapshotId);
      return sb.ToString();
    }

    public static TldRegistryData FromString(string str)
    {
      string[] attrs = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
      if (attrs.Length == 4)
      {
        int rank = 0;
        if (!string.IsNullOrEmpty(attrs[0]) && !string.IsNullOrEmpty(attrs[1]) &&
            int.TryParse(attrs[2], out rank) && !string.IsNullOrEmpty(attrs[3]))
        {
          return (new TldRegistryData(attrs[0], attrs[1], rank, attrs[3]));
        }
      }

      return null;
    }
  }
}
