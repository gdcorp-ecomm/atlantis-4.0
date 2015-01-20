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

    public TMSContentPlaceHolder(string product, string interaction, string channel, string template, int? rank = null)
    {
      IList<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(8);

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

      // Required Attribute: channel
      if (string.IsNullOrEmpty(channel))
      {
        throw new ArgumentException(string.Format("Missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Channel)));
      }
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Channel, channel));

      // Required Attribute: template
      if (string.IsNullOrEmpty(template))
      {
        throw new ArgumentException(string.Format("Missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Template)));
      }
      attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Template, template));

      // Optional Attribute: rank
      if (rank.HasValue)
      {
        attributes.Add(new KeyValuePair<string, string>(PlaceHolderAttributes.TMS_Rank, 
          rank.Value.ToString(CultureInfo.InvariantCulture)));
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
