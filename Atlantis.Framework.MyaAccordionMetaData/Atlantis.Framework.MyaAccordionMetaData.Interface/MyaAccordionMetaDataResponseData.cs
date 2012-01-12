using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class MyaAccordionMetaDataResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;

    private readonly Dictionary<int, AccordionMetaData> _accordionMetaDataDictionary;
    private readonly List<AccordionMetaData> _accordionMetaDataList;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public IEnumerable<AccordionMetaData> AccordionMetaDataItems
    {
      get { return _accordionMetaDataList; }
    }

    #endregion

    public MyaAccordionMetaDataResponseData(List<AccordionMetaData> accordionMetaDataList)
    {
      _accordionMetaDataList = accordionMetaDataList;
      _accordionMetaDataDictionary = new Dictionary<int, AccordionMetaData>(_accordionMetaDataList.Capacity);
      _accordionMetaDataList.ForEach(md => _accordionMetaDataDictionary.Add(md.AccordionId, md));
    }

    public MyaAccordionMetaDataResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MyaAccordionMetaDataResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "MyaAccordionMetaDataResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    public AccordionMetaData GetAccordionById(int accordionId)
    {
      AccordionMetaData result;
      return _accordionMetaDataDictionary.TryGetValue(accordionId, out result) ? result : null;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
