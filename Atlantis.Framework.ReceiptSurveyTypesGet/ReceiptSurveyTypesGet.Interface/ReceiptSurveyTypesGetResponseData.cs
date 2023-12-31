﻿using System;
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
    public string Culture { get; private set; }

    private List<SurveyItem> _tvSurveyTypes;
    public List<SurveyItem> TvSurveyTypes
    {
      get
      {
        return _tvSurveyTypes;
      }
    }

    private List<SurveyItem> _eventSurveyTypes;
    public List<SurveyItem> EventSurveyTypes
    {
      get
      {
        return _eventSurveyTypes;
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

    public ReceiptSurveyTypesGetResponseData(IEnumerable<SurveyItem> surveyTypes, string culture = "en")
    {
      AllSurveyTypes = surveyTypes;
      Culture = culture;
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

    #region Populate survey item list.
    [Obsolete("Use PopulateSurveyItems instead")]
    public List<SurveyItem> RandomizeSurveyItems(bool addPositionValue)
    {
      return PopulateSurveyItems(addPositionValue, true);
    }

    public List<SurveyItem> PopulateSurveyItems(bool addPositionValue = false, bool randomizeItems = true, bool removeTvText = false)
    {
      _tvSurveyTypes = new List<SurveyItem>();
      _eventSurveyTypes = new List<SurveyItem>();
      _otherSurveyTypes = new List<SurveyItem>();
      List<SurveyItem> tvTypesRacing = new List<SurveyItem>();
      List<SurveyItem> allSurveyTypes = new List<SurveyItem>();

      Random rnd = new Random();
      int max = 100;

      foreach (SurveyItem item in AllSurveyTypes)
      {
        SurveyItem clonedItem = item.Clone(rnd.Next(max)) as SurveyItem;

        if (item.IsTVItem) // is item part of TV group
        {
          if (removeTvText)
          {
            int dashIndex = clonedItem.Text.IndexOf("-", StringComparison.OrdinalIgnoreCase);
            clonedItem.Text = clonedItem.Text.Remove(0, dashIndex + 1).Trim();
          }

          if (!item.IsRacingItem)  //if it's not a racing item in the TV group
          {
            _tvSurveyTypes.Add(clonedItem);
          }
          else
          {
            tvTypesRacing.Add(clonedItem);
          }
        }
        else if (item.IsEventItem)
        {
          _eventSurveyTypes.Add(clonedItem);
        }
        else
        {
          _otherSurveyTypes.Add(clonedItem);
        }
      }

      if (randomizeItems)
      {
        _tvSurveyTypes.Sort(); //sorts by random value
        _eventSurveyTypes.Sort();
        _otherSurveyTypes.Sort(); //sorts by random value
      }

      int insertIndex = new Random().Next(_tvSurveyTypes.Count - 1);
      _tvSurveyTypes.InsertRange(insertIndex, tvTypesRacing);

      if (!randomizeItems) //sort alphabetically
      {
        _tvSurveyTypes = _tvSurveyTypes.OrderBy(si => si.Text).ToList();
        _eventSurveyTypes = _eventSurveyTypes.OrderBy(si => si.Text).ToList();
        _otherSurveyTypes = _otherSurveyTypes.OrderBy(si => si.Text).ToList();
      }

      FetchResource resourcFetcher = new FetchResource("Atlantis.Framework.ReceiptSurveyTypesGet.Interface.LanguageResources.ReceiptSurveyTypes", Culture);

      _tvSurveyTypes.Insert(0, new SurveyItem("1", resourcFetcher.GetString("surveySelectOne"), string.Empty));
      allSurveyTypes.AddRange(_tvSurveyTypes);
      allSurveyTypes.AddRange(_eventSurveyTypes);
      allSurveyTypes.AddRange(_otherSurveyTypes);

      if (addPositionValue)
      {
        int position = 1;
        allSurveyTypes.ForEach(si => AddPositionToSurveyItem(ref si, ref position));
      }

      
      return allSurveyTypes;
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
