using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSContentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.TMSContent + ":{0}]@P]";

    private static readonly IList<KeyValuePair<string, string>> _emptyParameters = new List<KeyValuePair<string, string>>(0);

    private readonly TMSContentPlaceHolderData _placeHolderData;

    public TMSContentPlaceHolder(string appProduct, string interactionName, string defaultAppID, string defaultLocation)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>();
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.AppProduct, appProduct));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.InteractionPoint, interactionName));

      TMSContentPlaceHolderData.DefaultData contentData = new TMSContentPlaceHolderData.DefaultData();
      contentData.Application = defaultAppID;
      contentData.Location = defaultLocation;

      _placeHolderData = new TMSContentPlaceHolderData(attributes, contentData);
    }

    #region IPlaceHolder Members

    public string ToMarkup()
    {
      return String.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }

    #endregion
  }
}
