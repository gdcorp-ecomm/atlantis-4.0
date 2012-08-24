using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelDeletePL.Interface
{
    public class PromoOrderLevelDeletePLRequestData : RequestData
    {
      #region Constructors

      public PromoOrderLevelDeletePLRequestData(string sourceUrl, string pathway, int pageCount)
        : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
      {
      }

      public PromoOrderLevelDeletePLRequestData(string sourceUrl, string pathway, int pageCount, string promoId)
        : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
      {
        this._promoId = promoId;
      }

      public PromoOrderLevelDeletePLRequestData(string sourceUrl, string pathway, int pageCount, string promoId, int privateLabelId)
        : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
      {
        this._promoId = promoId;
        this._privateLabelIds.Add(privateLabelId);
      }

      public PromoOrderLevelDeletePLRequestData(string sourceUrl, string pathway, int pageCount, string promoId, List<int> privateLabelId)
        : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
      {
        this._promoId = promoId;
        this._privateLabelIds = privateLabelId;
      }

      #endregion

      #region Members

      /// <summary>
      /// The ID of the promo that the removal request will be executed against.
      /// </summary>
      private string _promoId;
      public string PromoId
      {
        get { return this._promoId; }
        set { this._promoId = value; }
      }

      /// <summary>
      /// The list of private label IDs that are to be removed from the promo.
      /// </summary>
      private List<int> _privateLabelIds = new List<int>();
      public List<int> PrivateLabelIdList
      {
        get { return this._privateLabelIds; }
        set { this._privateLabelIds = value; }
      }

      #endregion

      public override string ToXML()
      {
        StringBuilder result = new StringBuilder();

        try
        {
          using (XmlTextWriter writer = new XmlTextWriter( new StringWriter(result)))
          {
            writer.WriteStartElement("OrderPromoPrivateLabel");
            writer.WriteAttributeString("id", this._promoId);
            writer.WriteStartElement("PrivateLabels");

            foreach (int i in this._privateLabelIds)
            {
              writer.WriteStartElement("PrivateLabelRemove");
              writer.WriteAttributeString("privateLabelID", i.ToString());
              writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
          }
        }
        catch (Exception ex)
        {
          throw new AtlantisException(this, "PromoOrderLevelDeletePLRequestData::ToXML()", ex.Message, ex.StackTrace);
        }

        return result.ToString();
      }
     
      public override string GetCacheMD5()
      {
        throw new Exception("PromoOrderLevelDeletePLRequestData is not a cacheable item");
      }
    }
}
