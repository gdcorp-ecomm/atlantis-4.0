﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Common.Interfaces
{
  public interface ICDSWidget<T>
  {
    T WidgetModel { get; set; }
  }
}
