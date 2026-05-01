// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregOrganization.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the brreg organization class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class BrregOrganization
    {
        [JsonPropertyName("organisasjonsnummer")]
        public int BrregNumber { get; set; }

        [JsonPropertyName("links")]
        public string Links { get; set; }

        [JsonPropertyName("navn")]
        public string Name { get; set; }

        [JsonPropertyName("stiftelsesdato")]
        public string FoundedDate { get; set; }

        [JsonPropertyName("registreringsdatoEnhetsregisteret")]
        public string RegistrationDate { get; set; }

        [JsonPropertyName("organisasjonsform")]
        public virtual string OrganisationType { get; set; } // TODO: Does not deserialize correctly for get by id

        [JsonPropertyName("registrertIFrivillighetsregisteret")]
        public string VoluntaryRegistered { get; set; }

        public bool? VoluntaryRegisteredBool { get { return GetBool(VoluntaryRegistered); } }

        [JsonPropertyName("registrertIMvaregisteret")]
        public string RegisteredImGoodsRegister { get; set; }

        public bool? RegistredImGoodsRegisterBool { get { return GetBool(RegisteredImGoodsRegister); } }

        [JsonPropertyName("registrertIForetaksregisteret")]
        public string RegisteredBusinessRegister { get; set; }

        public bool? RegistredBusinessRegisterBool { get { return GetBool(RegisteredBusinessRegister); } }

        [JsonPropertyName("registrertIStiftelsesregisteret")]
        public string RegisteredFoundingRegister { get; set; }

        public bool? RegisteredFoundingRegisterBool { get { return GetBool(RegisteredFoundingRegister); } }

        [JsonPropertyName("antallAnsatte")]
        public int? NumberEmployees { get; set; }

        [JsonPropertyName("institusjonellSektorkode")]
        public InstitutionSectorCode InstitutionSectorCode { get; set; }

        [JsonPropertyName("naeringskode1")]
        public IndustryCode1 IndustryCode1 { get; set; }

        [JsonPropertyName("forretningsadresse")]
        public PostAddress BusinessAddress { get; set; }

        [JsonPropertyName("postadresse")]
        public virtual PostAddress PostAddress { get; set; }

        [JsonPropertyName("konkurs")]
        public string Bankrupt { get; set; }

        public bool? BankruptBool { get { return GetBool(Bankrupt); } }

        [JsonPropertyName("underAvvikling")]
        public string UnderLiquidation { get; set; }

        public bool? UnderLiquidationBool { get { return GetBool(UnderLiquidation); } }

        [JsonPropertyName("underTvangsavviklingEllerTvangsopplosning")]
        public string UnderLiquidationOrDissolution { get; set; }

        public bool? UnderLiquidationOrDissolutionBool { get { return GetBool(UnderLiquidationOrDissolution); } }

        [JsonPropertyName("sisteInnsendteAarsregnskap")]
        public string LatestFiledAnnualAccounts { get; set; }

        [JsonPropertyName("maalform")]
        public string LanguageVariant { get; set; }

        [JsonPropertyName("orgform")]
        public Orgform Orgform { get; set; }

        [JsonPropertyName("hjemmeside")]
        public string Website { get; set; }

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