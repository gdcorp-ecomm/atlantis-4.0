using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class StateResponseData : IResponseData
  {
    public static StateResponseData Empty { get; private set; }

    static StateResponseData()
    {
      Empty = new StateResponseData(new List<State>(0));
    }

    private AtlantisException _exception = null;
    private List<State> _stateList;
    private Dictionary<string, State> _statesByName;
    private Dictionary<string, State> _statesByCode;
    private Dictionary<int, State> _statesById;

    public static StateResponseData FromDataCacheXml(string statesXml)
    {
      XElement states = XElement.Parse(statesXml);

      List<State> stateList = new List<State>();
      var stateElements = states.Descendants("state");
      foreach (var stateElement in stateElements)
      {
        State state = State.FromCacheElement(stateElement);
        stateList.Add(state);
      }

      if (stateList.Count > 0)
      {
        return new StateResponseData(stateList);
      }
      else
      {
        return Empty;
      }

    }

    public static StateResponseData FromException(AtlantisException exception)
    {
      return new StateResponseData(exception);
    }

    private StateResponseData(List<State> states)
    {
      _stateList = states;

      _statesByName = new Dictionary<string, State>(_stateList.Count, StringComparer.OrdinalIgnoreCase);
      _statesByCode = new Dictionary<string, State>(_stateList.Count, StringComparer.OrdinalIgnoreCase);
      _statesById = new Dictionary<int, State>(_stateList.Count);

      foreach (State state in _stateList)
      {
        _statesByName[state.Name] = state;
        _statesByCode[state.Code] = state;
        _statesById[state.Id] = state;
      }
    }

    private StateResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public IEnumerable<State> States
    {
      get { return _stateList; }
    }

    public int Count
    {
      get 
      {
        if (_stateList != null)
        {
          return _stateList.Count;
        }
        else
        {
          return 0;
        }
      }
    }

    public State FindStateByName(string name)
    {
      State result = null;
      if (_statesByName.ContainsKey(name))
      {
        result = _statesByName[name];
      }
      return result;
    }

    public State FindStateByCode(string code)
    {
      State result = null;
      if (_statesByCode.ContainsKey(code))
      {
        result = _statesByCode[code];
      }
      return result;
    }

    public State FindStateById(int id)
    {
      State result = null;
      if (_statesById.ContainsKey(id))
      {
        result = _statesById[id];
      }
      return result;
    }

    public string ToXML()
    {
      XElement states = new XElement("states");

      if (_stateList != null)
      {
        foreach (State state in _stateList)
        {
          XElement stateElement = new XElement("state");
          stateElement.Add(
            new XAttribute("id", state.Id.ToString()),
            new XAttribute("code", state.Code),
            new XAttribute("name", state.Name));
          states.Add(stateElement);
        }
      }

      return states.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
