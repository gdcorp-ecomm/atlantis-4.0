using System;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  public interface ITldPhase
  {
    string Name { get; set; }

    DateTime StartDate { get; set; }

    DateTime StopDate { get; set; }
  }
}
