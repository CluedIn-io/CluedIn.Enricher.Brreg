// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregOrganization.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the brreg organization class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    public class BrregOrganization
    {
        [JsonProperty("organisasjonsnummer")]
        public int BrregNumber { get; set; }

        [JsonProperty("links")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string Links { get; set; }

        [JsonProperty("_links")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        private string UnderscoreLinks { set => Links = value; }

        [JsonProperty("navn")]
        public string Name { get; set; }

        [JsonProperty("stiftelsesdato")]
        public string FoundedDate { get; set; }

        [JsonProperty("registreringsdatoEnhetsregisteret")]
        public string RegistrationDate { get; set; }

        [JsonProperty("organisasjonsform")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public virtual string OrganisationType { get; set; } // TODO: Does not deserialize correctly for get by id

        [JsonProperty("registrertIFrivillighetsregisteret")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string VoluntaryRegistered { get; set; }

        public bool? VoluntaryRegisteredBool { get { return GetBool(VoluntaryRegistered); } }

        [JsonProperty("registrertIMvaregisteret")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string RegisteredImGoodsRegister { get; set; }

        public bool? RegistredImGoodsRegisterBool { get { return GetBool(RegisteredImGoodsRegister); } }

        [JsonProperty("registrertIForetaksregisteret")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string RegisteredBusinessRegister { get; set; }

        public bool? RegistredBusinessRegisterBool { get { return GetBool(RegisteredBusinessRegister); } }

        [JsonProperty("registrertIStiftelsesregisteret")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string RegisteredFoundingRegister { get; set; }

        public bool? RegisteredFoundingRegisterBool { get { return GetBool(RegisteredFoundingRegister); } }

        [JsonProperty("antallAnsatte")]
        public int? NumberEmployees { get; set; }

        [JsonProperty("institusjonellSektorkode")]
        public InstitutionSectorCode InstitutionSectorCode { get; set; }

        [JsonProperty("naeringskode1")]
        public IndustryCode1 IndustryCode1 { get; set; }

        [JsonProperty("forretningsadresse")]
        public PostAddress BusinessAddress { get; set; }

        [JsonProperty("postadresse")]
        public virtual PostAddress PostAddress { get; set; }

        [JsonProperty("konkurs")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string Bankrupt { get; set; }

        public bool? BankruptBool { get { return GetBool(Bankrupt); } }

        [JsonProperty("underAvvikling")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string UnderLiquidation { get; set; }

        public bool? UnderLiquidationBool { get { return GetBool(UnderLiquidation); } }

        [JsonProperty("underTvangsavviklingEllerTvangsopplosning")]
        [JsonConverter(typeof(RawJsonStringConverter))]
        public string UnderLiquidationOrDissolution { get; set; }

        public bool? UnderLiquidationOrDissolutionBool { get { return GetBool(UnderLiquidationOrDissolution); } }

        [JsonProperty("sisteInnsendteAarsregnskap")]
        public string LatestFiledAnnualAccounts { get; set; }

        [JsonProperty("maalform")]
        public string LanguageVariant { get; set; }

        [JsonProperty("orgform")]
        public Orgform Orgform { get; set; }

        [JsonProperty("hjemmeside")]
        public string Website { get; set; }

        [JsonProperty("harRegistrertAntallAnsatte")]
        public bool? HasRegisteredNumberOfEmployees { get; set; }

        [JsonProperty("registreringsdatoMerverdiavgiftsregisteret")]
        public string VatRegistrationDate { get; set; }

        [JsonProperty("registreringsdatoMerverdiavgiftsregisteretEnhetsregisteret")]
        public string VatRegistrationDateEntityRegister { get; set; }

        [JsonProperty("aktivitet")]
        public List<string> Activities { get; set; }

        [JsonProperty("registrertIPartiregisteret")]
        public string RegisteredInPartyRegister { get; set; }

        public bool? RegisteredInPartyRegisterBool { get { return GetBool(RegisteredInPartyRegister); } }

        [JsonProperty("paategninger")]
        public List<Endorsement> Endorsements { get; set; }

        [JsonProperty("erIKonsern")]
        public bool? IsPartOfCorporateGroup { get; set; }

        [JsonProperty("respons_klasse")]
        public string ResponseClass { get; set; }

        private bool? GetBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (value.Equals("J", StringComparison.OrdinalIgnoreCase))
                return true;

            if (value.Equals("N", StringComparison.OrdinalIgnoreCase))
                return false;

            if (bool.TryParse(value, out var flag))
                return flag;

            throw new ArgumentException("Could not convert value: " + value, nameof(value));
        }
    }
}
