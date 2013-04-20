using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Reseller.Interface.CustomDomains
{
    public struct ApplicationState
    {
        internal ApplicationState(int id, int ordinalPosition, string name)
        {
            Id = id;
            OrdinalPosition = ordinalPosition;
            Name = name;
        }

        /// <summary>
        /// The database ID associated with this particular application setup state.
        /// </summary>
        public int Id;
        
        /// <summary>
        /// Identifies at what position this application state falls in the list of all application positions. Primarily used
        /// for the provisioning process.
        /// </summary>
        public int OrdinalPosition;

        /// <summary>
        /// The friendly-name associated with this application setup state.
        /// </summary>
        public string Name;
    }
}
