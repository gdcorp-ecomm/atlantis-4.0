using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class WebControlPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.WebControl + ":{0}]@P]";

    private readonly PlaceHolderData _placeHolderData;

    public WebControlPlaceHolder(string assembly, string typeName, IList<KeyValuePair<string, string>> parameters)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(3);
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Assembly, assembly));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Type, typeName));

      _placeHolderData = new PlaceHolderData(attributes, parameters);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }
  }
}
