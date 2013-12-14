using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSDocumentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.TMSDocument + ":{0}]@P]";

    private static readonly IList<KeyValuePair<string, string>> _emptyParameters = new List<KeyValuePair<string, string>>(0);

    private readonly TMSPlaceHolderData _placeHolderData;

    public TMSDocumentPlaceHolder(string interactionPoint, IList<string> messageTags)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(2);
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.InteractionPoint, interactionPoint));
      
      _placeHolderData = new TMSPlaceHolderData(attributes, messageTags);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }
  }
}
