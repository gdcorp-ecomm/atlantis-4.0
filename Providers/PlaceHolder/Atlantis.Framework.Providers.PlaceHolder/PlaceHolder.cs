using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class PlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[{0}:{1}]@P]";

    private readonly string _type;
    private readonly XmlPlaceHolderData _placeHolderData;

    internal PlaceHolder(string type, string location, IDictionary<string, string> parameters)
    {
      _type = type;
      _placeHolderData = new XmlPlaceHolderData(location, parameters);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _type, _placeHolderData.ToXml());
    }
  }
}
