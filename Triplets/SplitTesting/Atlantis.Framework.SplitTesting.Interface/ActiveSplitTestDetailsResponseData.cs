using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTestDetailsResponseData : IResponseData
  {
    public static ActiveSplitTestDetailsResponseData Empty { get; private set; }

    static ActiveSplitTestDetailsResponseData()
    {
      Empty = new ActiveSplitTestDetailsResponseData();
    }

    private readonly AtlantisException _exception;
    private readonly IEnumerable<ActiveSplitTestSide> _activeSplitTestDetails;

    public static ActiveSplitTestDetailsResponseData FromException(AtlantisException exception)
    {
      return new ActiveSplitTestDetailsResponseData(exception);
    }

    private ActiveSplitTestDetailsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public static ActiveSplitTestDetailsResponseData FromCacheXml(string cacheXml)
    {
      var activeSplitDetails = new List<ActiveSplitTestSide>();

      if (!string.IsNullOrEmpty(cacheXml))
      {
        try
        {
          XElement splitTestList = XElement.Parse(cacheXml);
          foreach (var itemElement in splitTestList.Elements("item"))
          {
            var sideidAtt = itemElement.Attribute("SplitTestSideID");
            var sidenameAtt = itemElement.Attribute("SideName");
            var percentallocationAtt = itemElement.Attribute("InitialPercentAllocation");

            if (string.IsNullOrEmpty(sideidAtt.Value) || string.IsNullOrEmpty(sidenameAtt.Value) ||
                string.IsNullOrEmpty(percentallocationAtt.Value))
            {
              const string message = "Xml with invalid SplitTestSideID, SideName or InitialPercentAllocation";
              var aex = new AtlantisException("ActiveSplitTestDetailsResponseData.ctor", "0", message,
                                              itemElement.ToString(), null, null);
              Engine.Engine.LogAtlantisException(aex);

              continue;
            }

            var sideid = sideidAtt.Value;
            var sidename = sidenameAtt.Value;
            var percentallocation = percentallocationAtt.Value;

            var ast = new ActiveSplitTestSide
            {
              SideId = Convert.ToInt32(sideid),
              Name = sidename,
              Allocation = Convert.ToDouble(percentallocation)
            };

            activeSplitDetails.Add(ast);
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("ActiveSplitTestDetailsResponseData.FromCacheXml", "0", ex.Message + ex.StackTrace, cacheXml, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      if (activeSplitDetails.Count == 0)
      {
        return Empty;
      }

      return new ActiveSplitTestDetailsResponseData(activeSplitDetails);
    }

    private ActiveSplitTestDetailsResponseData()
    {
      _activeSplitTestDetails = new List<ActiveSplitTestSide>();
    }

    private ActiveSplitTestDetailsResponseData(IEnumerable<ActiveSplitTestSide> splitTests)
    {
      _activeSplitTestDetails = splitTests;
    }

    public IEnumerable<ActiveSplitTestSide> SplitTestDetails
    {
      get { return _activeSplitTestDetails; }
    }

    public string ToXML()
    {
      var rootElement = new XElement("ActiveSplitTestDetailsData");

      if (_activeSplitTestDetails != null)
      {
        foreach (var splitTestDetail in _activeSplitTestDetails)
        {
          var testElement = new XElement("SplitTestDetail");
          testElement.Add(new XAttribute("SideId", splitTestDetail.SideId));
          testElement.Add(new XAttribute("SideName", splitTestDetail.Name));
          testElement.Add(new XAttribute("InitialPercentAllocation", splitTestDetail.Allocation));

          rootElement.Add(testElement);
        }
      }
      return rootElement.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
