﻿using Pokemon_Api.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pokemon_Api.Models
{
    public class Language : NamedApiObject
    {
        /// <summary>
        /// Whether or not hte games are published in this language.
        /// </summary>
        [JsonPropertyName("official")]
        public bool IsOfficial
        {
            get;
            internal set;
        }

        /// <summary>
        /// The two-letter code of the country where this language is spoken. Note that it is not unique.
        /// </summary>
        public string Iso639
        {
            get;
            internal set;
        }
        /// <summary>
        /// the two-letter code of the language. Note that this is not unique.
        /// </summary>
        public string Iso3166
        {
            get;
            internal set;
        }
    }
}
