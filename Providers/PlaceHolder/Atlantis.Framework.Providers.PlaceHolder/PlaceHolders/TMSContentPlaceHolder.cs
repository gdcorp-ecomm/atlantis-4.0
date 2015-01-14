using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSContentPlaceHolder : IPlaceHolder
  {
    private const string PLACE_HOLDER_MARKUP_FORMAT = "[@P[" + PlaceHolderTypes.TMSContent + ":{0}]@P]";

    private readonly TMSContentPlaceHolderData _placeHolderData;

    public TMSContentPlaceHolder(string app, string location, string product,
      string interaction, string channel, string template, int? rank = null)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(8);

      // Optional Attribute: app
      if (!string.IsNullOrEmpty(app))
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Application, app));
      }

      // Optional Attribute: location
      if (!string.IsNullOrEmpty(location))
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.Location, location));
      }

      // Required Attribute: product
      if (string.IsNullOrEmpty(product))
      {
        throw new ArgumentException(string.Format("Missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Product)));
      }
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Product, product));

      // Required Attribute: interaction
      if (string.IsNullOrEmpty(interaction))
      {
        throw new ArgumentException(string.Format("Missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Interaction)));
      }
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Interaction, interaction));

      // Optional Attribute: channel
      if (!string.IsNullOrEmpty(channel))
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Channel, channel));
      }

      // Required Attribute: template
      if (!string.IsNullOrEmpty(template))
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Template, template));
      }

      // Optional Attribute: rank
      if (rank.HasValue)
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Rank, rank.Value.ToString(CultureInfo.InvariantCulture)));
      }


      _placeHolderData = new TMSContentPlaceHolderData(attributes);
    }

    #region IPlaceHolder Members

    public string ToMarkup()
    {
      return String.Format(PLACE_HOLDER_MARKUP_FORMAT, _placeHolderData);
    }

    #endregion
  }
}
