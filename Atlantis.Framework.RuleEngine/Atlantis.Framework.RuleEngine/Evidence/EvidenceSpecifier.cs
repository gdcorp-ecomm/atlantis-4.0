/*
Simple Rule Engine
Copyright (C) 2005 by Sierra Digital Solutions Corp

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/


namespace Atlantis.Framework.RuleEngine.Evidence
{
  public struct EvidenceSpecifier
  {
    public EvidenceSpecifier(bool truthality, string evidenceId)
    {
      _truthality = truthality;
      _evidence = evidenceId;
    }

    private bool _truthality;
    public bool Truthality { get { return _truthality; }
      set { _truthality = value; }
    }

    private string _evidence;
    public string EvidenceId
    {
      get { return _evidence; }
      set { _evidence = value; }
    }
  }
}
