// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregAddressVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the BrregAddressVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Brreg.Vocabularies
{
    /// <summary>The Brreg address vocabulary</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class BrregAddressVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrregAddressVocabulary"/> class.
        /// </summary>
        public BrregAddressVocabulary()
        {
            VocabularyName = "Brreg Address";
            KeyPrefix      = "brreg.address";
            KeySeparator   = ".";
            Grouping       = EntityType.Geography;

            PostalCode         = Add(new VocabularyKey("postalCode"));
            PostalArea         = Add(new VocabularyKey("postalArea"));
            MunicipalityNumber = Add(new VocabularyKey("municipalityNumber"));
            CountryCode        = Add(new VocabularyKey("countryCode"));
            Municipality       = Add(new VocabularyKey("municipality", VocabularyKeyDataType.GeographyLocation));
            Country            = Add(new VocabularyKey("country", VocabularyKeyDataType.GeographyCountry));
            Address            = Add(new VocabularyKey("address"));
            Formatted          = Add(new VocabularyKey("formatted"));
        }

        public VocabularyKey Formatted { get; set; }

        public VocabularyKey Address { get; set; }

        public VocabularyKey Country { get; set; }

        public VocabularyKey Municipality { get; set; }

        public VocabularyKey CountryCode { get; set; }

        public VocabularyKey MunicipalityNumber { get; set; }

        public VocabularyKey PostalArea { get; set; }

        public VocabularyKey PostalCode { get; set; }
    }
}