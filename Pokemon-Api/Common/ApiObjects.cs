using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Api.Common
{
    public abstract class ApiObject
    {
        /// <summary>
        /// The identifier for this <see cref="ApiObject" />.
        /// </summary>
        public int ID
        {
            get;
            internal set;
        }
    }
    public abstract class NamedApiObject : ApiObject
    {
        /// <summary>
        /// The name for this <see cref="NamedApiObject" />.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// The localized names for this <see cref="NamedApiObject" />.
        /// </summary>
        public ResourceName[] Names
        {
            get;
            internal set;
        }
    }
}
