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

    private List<SurveyItem> _tvSurveyTypes;
    public List<SurveyItem> TvSurveyTypes
    {
      get
      {
        return _tvSurveyTypes;
      }
    }

    private List<SurveyItem> _otherSurveyTypes;
    public List<SurveyItem> OtherSurveyTypes
    {
      get
      {
        return _otherSurveyTypes;
      }
    }

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
      _tvSurveyTypes = new List<SurveyItem>();
      _otherSurveyTypes = new List<SurveyItem>();
      List<SurveyItem> tvTypesRacing = new List<SurveyItem>();
      List<SurveyItem> allSurveyTypes = new List<SurveyItem>();

      Random rnd = new Random();
      int max = 100;
      int position = 1;

      foreach (SurveyItem item in AllSurveyTypes)
      {
        SurveyItem clonedItem = item.Clone(rnd.Next(max)) as SurveyItem;

        if (addPositionValue)
        {
          clonedItem.Value += "," + position.ToString();
          position++;
        }

        if (item.IsTVItem) // is item part of TV group
        {
          if (!item.IsRacingItem)  //if it's not a racing item in the TV group
          {
            _tvSurveyTypes.Add(clonedItem);
          }
          else
          {
            tvTypesRacing.Add(clonedItem);
          }
        }
        else
        {
          _otherSurveyTypes.Add(clonedItem);
        }
      }

      _tvSurveyTypes.Sort(); //sorts by random value
      _otherSurveyTypes.Sort(); //sorts by random value
      
      int insertIndex = new Random().Next(_tvSurveyTypes.Count - 1);
      _tvSurveyTypes.InsertRange(insertIndex, tvTypesRacing);
      allSurveyTypes.AddRange(_tvSurveyTypes);
      allSurveyTypes.AddRange(_otherSurveyTypes);
      
      return allSurveyTypes;
    }

    #endregion

    #region Interface Methods

    public string ToXML()
    {
      string xml = string.Empty;
      try
      {
        XmlSerializer serializer = new XmlSerializer(typeof(IEnumerable<SurveyItem>));
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
