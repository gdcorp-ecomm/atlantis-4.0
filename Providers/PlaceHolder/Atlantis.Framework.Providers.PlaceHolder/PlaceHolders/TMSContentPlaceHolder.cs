using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSContentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.TMSContent + ":{0}]@P]";

    private readonly TMSContentPlaceHolderData _placeHolderData;

    public TMSContentPlaceHolder(string appProduct, string interactionName, string defaultApp, string defaultLocation,
      string contentApp = null, string contentLocation = null, bool overrideDocumentName = false)
    {
      IList<KeyValuePair<string, string>> attributes = new KeyValuePair<string, string>[2];
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.AppProduct, appProduct));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.InteractionPoint, interactionName));

      TMSContentPlaceHolderData.DefaultElementData defaultElement = new TMSContentPlaceHolderData.DefaultElementData(defaultApp, defaultLocation);
      TMSContentPlaceHolderData.ContentElementData contentElement = new TMSContentPlaceHolderData.ContentElementData(contentApp, contentLocation, overrideDocumentName);
      _placeHolderData = new TMSContentPlaceHolderData(attributes, defaultElement, contentElement);
    }

    #region IPlaceHolder Members

    public string ToMarkup()
    {
      return String.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }

    #endregion
  }
}
