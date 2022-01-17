// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregExternalSearchProviderUtil.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the brreg external search provider utility class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CluedIn.Core;
using CluedIn.Processing.EntityResolution;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CluedIn.ExternalSearch.Providers.Bregg
{
    public static class BrregExternalSearchProviderUtil
    {
        public static bool IsJson(string jsonString)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonString))
                {
                    var obj = JToken.Parse(jsonString);
                    return true;
                }
            }
            catch (JsonReaderException)
            {
                return false;
            }

            return false;
        }

        internal static bool CanDeserializeTo<T>(string jsonString)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonString))
                {
                    var obj = JsonUtility.Deserialize<T>(jsonString);
                    return true;
                }
            }
            catch (JsonSerializationException)
            {
                return false;
            }

            return false;
        }

        internal static Func<string, bool> NamePostFixFilter(HashSet<string> countryCode, Func<string, bool> countryFilter, HashSet<string> contains, HashSet<string> name, HashSet<string> postFixes)
        {
            bool PostFixFilter(string value)
            {
                if (countryCode != null && countryCode.Any() && !countryCode.Any(countryFilter)) return true;

                value = value.ToLowerInvariant();

                if (contains.Any(c => value.Contains(c))) return false;

                var nameFix = OrganizationName.Parse(value);
                {
                    if (name == null || string.IsNullOrEmpty(nameFix.Postfix)) return true;

                    if (postFixes.Contains(nameFix.Postfix.ToLowerInvariant())) return false;
                }

                if (postFixes.Any(v => value.EndsWith(v))) return false;

                return true;
            }

            return PostFixFilter;
        }
    }
}