// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregOrganization.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the brreg organization class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class BrregOrganization
    {
        [DeserializeAs(Name = "organisasjonsnummer")]
        public int BrregNumber { get; set; }

        [DeserializeAs(Name = "links")]
        public string Links { get; set; }

        [DeserializeAs(Name = "navn")]
        public string Name { get; set; }

        [DeserializeAs(Name = "stiftelsesdato")]
        public string FoundedDate { get; set; }

        [DeserializeAs(Name = "registreringsdatoEnhetsregisteret")]
        public string RegistrationDate { get; set; }

        [DeserializeAs(Name = "organisasjonsform")]
        public virtual string OrganisationType { get; set; } // TODO: Does not deserialize correctly for get by id

        [DeserializeAs(Name = "registrertIFrivillighetsregisteret")]
        public string VoluntaryRegistered { get; set; }

        public bool? VoluntaryRegisteredBool { get { return this.GetBool(this.VoluntaryRegistered); } }

        [DeserializeAs(Name = "registrertIMvaregisteret")]
        public string RegisteredImGoodsRegister { get; set; }

        public bool? RegistredImGoodsRegisterBool { get { return this.GetBool(this.RegisteredImGoodsRegister); } }

        [DeserializeAs(Name = "registrertIForetaksregisteret")]
        public string RegisteredBuisnessRegister { get; set; }

        public bool? RegistredBuisnessRegisterBool { get { return this.GetBool(this.RegisteredBuisnessRegister); } }

        [DeserializeAs(Name = "registrertIStiftelsesregisteret")]
        public string RegisteredFoundingRegister { get; set; }

        public bool? RegisteredFoundingRegisterBool { get { return this.GetBool(this.RegisteredFoundingRegister); } }

        [DeserializeAs(Name = "antallAnsatte")]
        public int? NumberEmployees { get; set; }

        [DeserializeAs(Name = "institusjonellSektorkode")]
        public InstitutionSectorCode InstitutionSectorCode { get; set; }

        [DeserializeAs(Name = "naeringskode1")]
        public IndustryCode1 IndustryCode1 { get; set; }

        [DeserializeAs(Name = "forretningsadresse")]
        public PostAddress BusinessAddress { get; set; }

        [DeserializeAs(Name = "postadresse")]
        public virtual PostAddress PostAddress { get; set; }

        [DeserializeAs(Name = "konkurs")]
        public string Bankrupt { get; set; }

        public bool? BankruptBool { get { return this.GetBool(this.Bankrupt); } }

        [DeserializeAs(Name = "underAvvikling")]
        public string UnderLiquidation { get; set; }

        public bool? UnderLiquidationBool { get { return this.GetBool(this.UnderLiquidation); } }

        [DeserializeAs(Name = "underTvangsavviklingEllerTvangsopplosning")]
        public string UnderLiquidationOrDissolution { get; set; }

        public bool? UnderLiquidationOrDissolutionBool { get { return this.GetBool(this.UnderLiquidationOrDissolution); } }

        [DeserializeAs(Name = "sisteInnsendteAarsregnskap")]
        public int LatestFiledAnnualAccounts { get; set; }

        [DeserializeAs(Name = "maalform")]
        public string LanguageVariant { get; set; }

        [DeserializeAs(Name = "orgform")]
        public Orgform Orgform { get; set; }

        [DeserializeAs(Name = "hjemmeside")]
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