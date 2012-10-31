﻿
using System;
using System.Reflection;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class NullRenderingEngine : IRenderingEngine
  {
    public string Render<T>(string rawContent, T model)
    {
      ErrorLogHelper.LogError(new Exception("NullRenderingEngine selected. Please verify you have a valid template source format in your placeholder."), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      return string.Empty;
    }
  }
}
