using System;
using System.Collections.Generic;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine.Model
{
  //public class ValidationResults : IDictionary<string, List<IValidationResult>>
  //{
  //  private readonly Dictionary<string, InputModel> _internalDict = new Dictionary<string, InputModel>();
    
  //  public void Add(string modelId, InputModel value)
  //  {
  //    _internalDict.Add(modelId, value);
  //  }

  //  public bool ContainsKey(string key)
  //  {
  //    return _internalDict.ContainsKey(key);
  //  }

  //  public ICollection<string> Keys
  //  {
  //    get { return _internalDict.Keys; }
  //  }

  //  public bool Remove(string key)
  //  {
  //    return _internalDict.Remove(key);
  //  }

  //  public bool TryGetValue(string key, out InputModel value)
  //  {
  //    return _internalDict.TryGetValue(key, out value);
  //  }

  //  public ICollection<InputModel> Values
  //  {
  //    get { return _internalDict.Values; }
  //  }

  //  public InputModel this[string key]
  //  {
  //    get
  //    {
  //      return _internalDict[key];
  //    }
  //    set
  //    {
  //      _internalDict[key] = value;
  //    }
  //  }


  //  #region ICollection<KeyValuePair<string,ResultModel>> Members

  //  public void Add(KeyValuePair<string, InputModel> item)
  //  {
  //    _internalDict.Add(item.Key, item.Value);
  //  }

  //  public void Clear()
  //  {
  //    _internalDict.Clear();
  //  }

  //  public bool Contains(KeyValuePair<string, InputModel> item)
  //  {
  //    return (_internalDict.ContainsKey(item.Key) && _internalDict.ContainsValue(item.Value));
  //  }
    
  //  public int Count
  //  {
  //    get { return _internalDict.Count; }
  //  }

  //  public bool IsReadOnly
  //  {
  //    get { return false; }
  //  }

  //  public bool Remove(KeyValuePair<string, InputModel> item)
  //  {
  //    return _internalDict.Remove(item.Key);
  //  }

  //  #endregion

  //  #region IEnumerable<KeyValuePair<string,ResultModel>> Members

  //  public IEnumerator<KeyValuePair<string, InputModel>> GetEnumerator()
  //  {
  //    return _internalDict.GetEnumerator();
  //  }

  //  #endregion

  //  #region IEnumerable Members

  //  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
  //  {
  //    return _internalDict.GetEnumerator();
  //  }

  //  #endregion


  //  public void CopyTo(KeyValuePair<string, InputModel>[] array, int arrayIndex)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public void Add(string key, List<IValidationResult> value)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public bool TryGetValue(string key, out List<IValidationResult> value)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  ICollection<List<IValidationResult>> IDictionary<string, List<IValidationResult>>.Values
  //  {
  //    get { throw new NotImplementedException(); }
  //  }

  //  List<IValidationResult> IDictionary<string, List<IValidationResult>>.this[string key]
  //  {
  //    get
  //    {
  //      throw new NotImplementedException();
  //    }
  //    set
  //    {
  //      throw new NotImplementedException();
  //    }
  //  }

  //  public void Add(KeyValuePair<string, List<IValidationResult>> item)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public bool Contains(KeyValuePair<string, List<IValidationResult>> item)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public void CopyTo(KeyValuePair<string, List<IValidationResult>>[] array, int arrayIndex)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public bool Remove(KeyValuePair<string, List<IValidationResult>> item)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  IEnumerator<KeyValuePair<string, List<IValidationResult>>> IEnumerable<KeyValuePair<string, List<IValidationResult>>>.GetEnumerator()
  //  {
  //    throw new NotImplementedException();
  //  }
  //}
}
