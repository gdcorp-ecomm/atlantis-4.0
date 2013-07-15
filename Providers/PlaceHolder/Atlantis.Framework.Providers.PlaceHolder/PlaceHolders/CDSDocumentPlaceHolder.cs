using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class CDSDocumentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.CDSDocument + ":{0}]@P]";

    private static readonly IList<KeyValuePair<string, string>> _emptyParameters = new List<KeyValuePair<string, string>>(0); 

    private readonly PlaceHolderData _placeHolderData;

    public CDSDocumentPlaceHolder(string application, string location)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(2);
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Application, application));
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Location, location));

      _placeHolderData = new PlaceHolderData(attributes, _emptyParameters);
    }

    public string ToMarkup()
    {
      return string.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }
  }
}
