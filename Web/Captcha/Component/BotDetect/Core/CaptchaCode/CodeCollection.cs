using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

using BotDetect.CaptchaCode;
using BotDetect.Serialization;

namespace BotDetect
{
  /// <summary>
  /// Represents a collection of Captcha codes generated for a particular 
  /// Captcha instance (identified by captchaId) in the current applications.
  /// </summary>
  [Serializable]
  public class CodeCollection : IEnumerable<Code>, ISerializable
  {
    /// <summary>
    /// internal storage
    /// </summary>
    private Dictionary<string, Code> _collection;

    /// <summary>
    /// The default CodeCollection constructor takes no arguments
    /// </summary>
    public CodeCollection()
    {
      _collection = new Dictionary<string, Code>();
    }

    // this field is assigned every time before generation
    // so it doesn't need to be preserved
    [NonSerializedAttribute]
    private CharacterSet _charset;
    internal CharacterSet CharacterSet
    {
      get
      {
        return _charset;
      }
      set
      {
        _charset = value;
      }
    }

    /// <summary>
    /// The currently configured timeout used to invalidate Captcha codes 
    /// after a certain period of time
    /// </summary>
    public static int CodeTimeout
    {
      get
      {
        return CaptchaConfiguration.CaptchaCodes.Timeout;
      }
    }

    /// <summary>
    /// Given the Captcha instance identifier and Captcha code purpose, 
    /// returns an existing Captcha code if a valid one exists, 
    /// or creates a new one automatically
    /// </summary>
    /// <param name="instanceId">the unique instance identifier</param>
    /// <param name="purpose">basic caller identification</param>
    /// <returns>the code value</returns>
    public string GetCode(string instanceId, CodeGenerationPurpose purpose, CodeStyle codeStyle, int codeLength)
    {
      string code = null;

      Code captchaCode = null;

      if (IsCodeAlreadyGenerated(instanceId))
      {
        captchaCode = LoadCode(instanceId);

        bool shouldRegenerate = ShouldCodeBeRegenerated(captchaCode, purpose);
        if (shouldRegenerate)
        {
          captchaCode = _charset.GenerateCode(codeStyle, codeLength);
        }
      }
      else
      {
        captchaCode = _charset.GenerateCode(codeStyle, codeLength);
      }

      RecordCodeUsage(ref captchaCode, purpose);
      SaveCode(instanceId, captchaCode, CodeTimeout);
      code = captchaCode.CaptchaCode;

      return code;
    }

    private bool IsCodeAlreadyGenerated(string instanceId)
    {
      return _collection.ContainsKey(instanceId);
    }

    private Code LoadCode(string instanceId)
    {
      return _collection[instanceId] as Code;
    }

    /// <summary>
    /// Stores a Code object in the collection
    /// </summary>
    /// <param name="instanceId"></param>
    /// <param name="code"></param>
    /// <param name="timeLimit"></param>
    public void SaveCode(string instanceId, Code code, int timeLimit)
    {
      // save the new code
      _collection[instanceId] = code;

      // delete all expired values
      if (0 != timeLimit)
      {
        List<string> keysToDelete = new List<string>(_collection.Count);
        foreach (string key in _collection.Keys)
        {
          Code savedCode = _collection[key] as Code;
          if (null != savedCode)
          {
            if (HasExpired(savedCode, timeLimit))
            {
              keysToDelete.Add(key);
            }
          }
        }
        foreach (string key in keysToDelete)
        {
          _collection.Remove(key);
        }
      }
    }

    private static bool ShouldCodeBeRegenerated(Code captchaCode, CodeGenerationPurpose purpose)
    {
      bool shouldRegenerate = true;

      switch (purpose)
      {
        case CodeGenerationPurpose.ImageGeneration:
          shouldRegenerate = captchaCode.IsUsedForImageGeneration;
          break;
        case CodeGenerationPurpose.SoundGeneration:
          // clicking the sound icon multiple times doesn't change the audio code
          shouldRegenerate = false;
          break;
        case CodeGenerationPurpose.Other:
          shouldRegenerate = false;
          break;
        default:
          Debug.Assert(false, "Unknown CodeGenerationPurpose value!");
          break;
      }

      return shouldRegenerate;
    }

    private static void RecordCodeUsage(ref Code captchaCode, CodeGenerationPurpose purpose)
    {
      switch (purpose)
      {
        case CodeGenerationPurpose.ImageGeneration:
          captchaCode.IsUsedForImageGeneration = true;
          break;
        case CodeGenerationPurpose.SoundGeneration:
          captchaCode.IsUsedForSoundGeneration = true;
          break;
        case CodeGenerationPurpose.Other:
          // do nothing
          break;
        default:
          Debug.Assert(false, "Unknown CodeGenerationPurpose value!");
          break;
      }
    }

    private static bool HasExpired(Code captchaCode, int timeLimit)
    {
      bool hasCodeExpired = true;

      if (0 == timeLimit)
      {
        hasCodeExpired = false;
      }
      else if (captchaCode.ElapsedSeconds < timeLimit)
      {
        hasCodeExpired = false;
      }

      return hasCodeExpired;
    }

    /// <summary>
    /// Compares the user input to stored values matching the criteria
    /// </summary>
    /// <param name="userInput"></param>
    /// <param name="instanceId"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public bool Validate(string userInput, string instanceId, ValidationAttemptOrigin origin)
    {
      bool isCorrect = false;
      Code captchaCode = null;

      // if the instanceId is correct
      if ((null != instanceId) && (_collection.ContainsKey(instanceId)))
      {
        if ((null != userInput) &&
              (0 != userInput.Length) &&
              (0 != userInput.Trim().Length)
            )
        {
          captchaCode = _collection[instanceId] as Code;
          if (userInput.ToUpperInvariant().Equals(captchaCode.CaptchaCode.ToUpperInvariant()))
          {
            // codes are valid only for a period of time
            if (!HasExpired(captchaCode, CodeTimeout))
            {
              isCorrect = true;
            }
          }
        }

        if ((ValidationAttemptOrigin.Client != origin) || (!isCorrect))
        {
          /// each code can only be validated once, 
          /// but client-side validations returning 'true' should not delete the value
          /// so the code can also be validated as 'true' on the server
          _collection.Remove(instanceId);
        }
        else
        {
          // record when did client-side validation pass
          captchaCode.RecordClientSideValidation();
        }
      }

      return isCorrect;
    }

    public override string ToString()
    {
      StringBuilder str = new StringBuilder();
      str.Append("BotDetect.CodeCollection {");

      if (null == _collection || 0 == _collection.Count)
      {
        str.Append(" empty }");
      }
      else
      {
        str.AppendLine();
        foreach (string key in _collection.Keys)
        {
          str.Append(key);
          str.Append(": ");
          Code savedValue = _collection[key] as Code;
          str.AppendLine(StringHelper.ToString(savedValue));
        }
        str.Append("}");
      }

      return str.ToString();
    }

    /// <summary>
    /// How many Code instances are contained in the collection
    /// </summary>
    public int Count
    {
      get
      {
        return _collection.Count;
      }
    }

    #region IEnumerable<Code> Members

    /// <summary>
    /// Allows enumerating the contained Code instances
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Code> GetEnumerator()
    {
      return _collection.Values.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    /// <summary>
    /// Allows enumerating the contained Code instances
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return _collection.Keys.GetEnumerator();
    }

    #endregion

    #region ISerializable Members

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      SerializationWriter writer = new SerializationWriter();
      writer.Write<string, Code>(_collection);

      info.AddValue("data", writer.ToArray());
    }

    protected CodeCollection(SerializationInfo info, StreamingContext context)
    {
      SerializationReader reader = new SerializationReader((byte[])info.GetValue("data", typeof(byte[])));

      _collection = reader.ReadDictionary<string, Code>();
    }

    #endregion
  }
}
