using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Web.Stash
{
  internal static class StashContext
  {
    const string _STASHDATAKEY = "Atlantis.Framework.Web.Stash.Data";

    static Dictionary<string, StashData> GetStashData()
    {
      Dictionary<string, StashData> result = HttpContext.Current.Items[_STASHDATAKEY] as Dictionary<string, StashData>;
      if (result == null)
      {
        result = new Dictionary<string, StashData>();
        HttpContext.Current.Items[_STASHDATAKEY] = result;
      }
      return result;
    }

    static StashData GetStashDataForLocation(string location)
    {
      Dictionary<string, StashData> stashData = GetStashData();
      StashData result;
      if (!stashData.TryGetValue(location, out result))
      {
        result = new StashData();
        stashData[location] = result;
      }
      return result;
    }

    internal static void RenderAndStashContent(StashContent content)
    {
      if (!string.IsNullOrEmpty(content.Location))
      {
        Dictionary<string, StashData> stashDataLocations = GetStashData();
        StashData renderStash;
        if (!stashDataLocations.TryGetValue(content.Location, out renderStash))
        {
          renderStash = new StashData();
          stashDataLocations[content.Location] = renderStash;
        }

        renderStash.RenderIntoStash(content);
      }
    }

    internal static string GetRenderedStashContent(string location)
    {
      string result = null;

      if (!string.IsNullOrEmpty(location))
      {
        Dictionary<string, StashData> stashDataLocations = GetStashData();
        StashData renderStash;
        if (stashDataLocations.TryGetValue(location, out renderStash))
        {
          result = renderStash.GetHtml();
        }
      }

      return result;
    }
  }
}
