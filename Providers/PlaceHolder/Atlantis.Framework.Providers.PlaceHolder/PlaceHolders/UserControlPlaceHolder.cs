using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class UserControlPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.UserControl + ":{0}]@P]";

    private readonly PlaceHolderData _placeHolderData;

    public UserControlPlaceHolder(string location, IList<KeyValuePair<string, string>> parameters)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(2);
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Location, location));

      _placeHolderData = new PlaceHolderData(attributes, parameters);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }
  }
}
