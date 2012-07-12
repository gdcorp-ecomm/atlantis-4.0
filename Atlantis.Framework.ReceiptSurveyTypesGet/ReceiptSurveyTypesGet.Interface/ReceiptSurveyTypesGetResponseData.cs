using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.IO;
using System.Xml.Serialization;

namespace Atlantis.Framework.ReceiptSurveyTypesGet.Interface
{
  public class ReceiptSurveyTypesGetResponseData : IResponseData
  {
    private AtlantisException _aex;

    public IEnumerable<SurveyItem> AllSurveyTypes { get; private set; }
    public bool IsSuccess { get; private set; }

    public ReceiptSurveyTypesGetResponseData(IEnumerable<SurveyItem> surveyTypes)
    {
      AllSurveyTypes = surveyTypes;
      IsSuccess = surveyTypes != null;
    }

    public ReceiptSurveyTypesGetResponseData(RequestData requestData, Exception ex)
    {
      _aex = new AtlantisException(requestData, "ReceiptSurveyTypesGetResponseData", ex.Message, ex.StackTrace);
      IsSuccess = false;
    }

    public ReceiptSurveyTypesGetResponseData(AtlantisException aex)
    {
      _aex = aex;
      IsSuccess = false;
    }

    #region Filter And Randomize
    public List<SurveyItem> RandomizeSurveyItems(bool addPositionValue = false)
    {
      List<SurveyItem> tvTypes = new List<SurveyItem>();
      List<SurveyItem> tvTypesRacing = new List<SurveyItem>();
      List<SurveyItem> otherTypes = new List<SurveyItem>();
      Random rnd = new Random();
      int max = 100;

      foreach (SurveyItem item in AllSurveyTypes)
      {
        SurveyItem clonedItem = item.Clone(rnd.Next(max)) as SurveyItem;
        if (item.IsTVItem) // does item contain TV in the text
        {
          if (!item.IsRacingItem)  //this item doesn't have a racing context
          {
            tvTypes.Add(clonedItem);
          }
          else
          {
            tvTypesRacing.Add(clonedItem);
          }
        }
        else
        {
          otherTypes.Add(clonedItem);
        }
      }

      tvTypes.Sort(); //sorts by random value
      tvTypesRacing.Sort(); //sorts by random value
      otherTypes.Sort(); //sorts by random value

      int insertIndex = new Random().Next(tvTypes.Count - 1);
      tvTypes.InsertRange(insertIndex, tvTypesRacing);
      tvTypes.AddRange(otherTypes);

      if (addPositionValue)
      {
        int position = 1;
        tvTypes.ForEach(si => AddPositionToSurveyItem(ref si, ref position));
      }

      return tvTypes;
    }

    private void AddPositionToSurveyItem(ref SurveyItem item, ref int position)
    {
      item.Value += "," + position.ToString();
      position++;
    }
    #endregion

    #region Interface Methods

    public string ToXML()
    {
      string xml = string.Empty;
      try
      {
        XmlSerializer serializer = new XmlSerializer(typeof(SurveyItem));
        StringWriter writer = new StringWriter();

        serializer.Serialize(writer, AllSurveyTypes);
        xml = writer.ToString();
      }
      catch
      {
      }

      return xml;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }

    #endregion

  }
}
