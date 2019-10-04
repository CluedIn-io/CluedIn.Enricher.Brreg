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

namespace CluedIn.ExternalSearch.Providers.Bregg.Vocabularies
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
            this.VocabularyName = "Brreg Address";
            this.KeyPrefix      = "brreg.address";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Geography;

            this.PostalCode         = this.Add(new VocabularyKey("postalCode"));
            this.PostalArea         = this.Add(new VocabularyKey("postalArea"));
            this.MunicipalityNumber = this.Add(new VocabularyKey("municipalityNumber"));
            this.CountryCode        = this.Add(new VocabularyKey("countryCode"));
            this.Municipality       = this.Add(new VocabularyKey("municipality", VocabularyKeyDataType.GeographyLocation));
            this.Country            = this.Add(new VocabularyKey("country", VocabularyKeyDataType.GeographyCountry));
            this.Address            = this.Add(new VocabularyKey("address"));
            this.Formatted          = this.Add(new VocabularyKey("formatted"));
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