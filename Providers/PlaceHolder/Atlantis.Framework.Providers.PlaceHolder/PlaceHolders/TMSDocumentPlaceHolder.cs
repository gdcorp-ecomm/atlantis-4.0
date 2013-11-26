using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSDocumentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.TMSDocument + ":{0}]@P]";

    private static readonly IList<KeyValuePair<string, string>> _emptyParameters = new List<KeyValuePair<string, string>>(0);

    private readonly PlaceHolderData _placeHolderData;

    public TMSDocumentPlaceHolder(string application, string tmsAppId, string interactionPoint, string messageTag)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(2);
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Application, application));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMSAppId, tmsAppId));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.InteractionPoint, interactionPoint));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.MessageTag, messageTag));

      _placeHolderData = new PlaceHolderData(attributes, _emptyParameters);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }
  }
}
