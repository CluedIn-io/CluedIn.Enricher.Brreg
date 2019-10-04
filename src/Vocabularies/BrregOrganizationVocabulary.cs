// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregOrganizationVocabulary.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the brreg organization vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Bregg.Vocabularies
{
    public class BrregOrganizationVocabulary : SimpleVocabulary
    {
        public BrregOrganizationVocabulary()
        {
            this.VocabularyName = "Brreg Organization";
            this.KeyPrefix      = "brreg.organization";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Organization;

            this.AddGroup("Metadata", group =>
            {
                this.Name                               = group.Add(new VocabularyKey("name"));
                this.BrregNumber                        = group.Add(new VocabularyKey("brregNumber",                        VocabularyKeyDataType.Number));
                this.FoundedDate                        = group.Add(new VocabularyKey("foundedDate",                        VocabularyKeyDataType.DateTime));
                this.RegistrationDate                   = group.Add(new VocabularyKey("registrationDate",                   VocabularyKeyDataType.DateTime));
                this.OrganizationType                   = group.Add(new VocabularyKey("organizationType"));
                this.OrganizationTypeFull               = group.Add(new VocabularyKey("organizationTypeFull",               VocabularyKeyDataType.Json,         VocabularyKeyVisibility.Hidden));
                this.VoluntaryRegistered                = group.Add(new VocabularyKey("voluntaryRegistered",                VocabularyKeyDataType.Boolean));
                this.RegisteredImGoodsRegister          = group.Add(new VocabularyKey("registeredImGoodsRegister",          VocabularyKeyDataType.Boolean));
                this.RegisteredBusinessRegister         = group.Add(new VocabularyKey("registeredBusinessRegister",         VocabularyKeyDataType.Boolean));
                this.RegisteredFoundingRegister         = group.Add(new VocabularyKey("registeredFoundingRegister",         VocabularyKeyDataType.Boolean));
                this.NumberEmployees                    = group.Add(new VocabularyKey("numberEmployees",                    VocabularyKeyDataType.Integer));
                this.BankruptBool                       = group.Add(new VocabularyKey("bankrupt",                           VocabularyKeyDataType.Boolean));
                this.UnderLiquidation                   = group.Add(new VocabularyKey("underLiquidation",                   VocabularyKeyDataType.Boolean));
                this.UnderLiquidationOrDissolution      = group.Add(new VocabularyKey("underLiquidationOrDissolution",      VocabularyKeyDataType.Boolean));
                this.LatestFiledAnnualAccounts          = group.Add(new VocabularyKey("latestFiledAnnualAccounts",          VocabularyKeyDataType.Boolean));
                this.LanguageVariant                    = group.Add(new VocabularyKey("languageVariant",                    VocabularyKeyDataType.Boolean));
                this.BrregUrl                           = group.Add(new VocabularyKey("brregUrl",                           VocabularyKeyDataType.Uri));
                this.Website                            = group.Add(new VocabularyKey("website",                            VocabularyKeyDataType.Uri));
            });

            this.AddGroup("Location", group =>
            {
                this.Address                        = group.Add(new BrregAddressVocabulary().AsCompositeKey("postAddress"));
                this.BusinessAddress                = group.Add(new BrregAddressVocabulary().AsCompositeKey("businessPostAddress"));

            });

            this.AddGroup("Details", group =>
            {
                this.IndustryCode                       = group.Add(new VocabularyKey("industryCode",                       VocabularyKeyDataType.Number));
                this.IndustryDescription                = group.Add(new VocabularyKey("industryDescription"));
                this.InstitutionSectorCode              = group.Add(new VocabularyKey("institutionSectorCode",              VocabularyKeyDataType.Number));
                this.InstitutionSectorDescription       = group.Add(new VocabularyKey("institutionSectorDescription"));
            });

            this.AddMapping(this.BrregNumber,                   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg);
            this.AddMapping(this.NumberEmployees,               CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.EmployeeCount);
            this.AddMapping(this.FoundedDate,                   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.FoundingDate);
            this.AddMapping(this.Website,                       CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);

            this.AddMapping(this.BusinessAddress.Address,       CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address);
            this.AddMapping(this.BusinessAddress.CountryCode,   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode);
            this.AddMapping(this.BusinessAddress.PostalCode,    CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            this.AddMapping(this.BusinessAddress.PostalArea,    CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressPostalArea);

            this.AddMapping(this.IndustryDescription,           CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Industry);
        }

        public VocabularyKey OrganizationTypeFull { get; set; }
        public VocabularyKey LanguageVariant { get; set; }
        public VocabularyKey LatestFiledAnnualAccounts { get; set; }
        public VocabularyKey BrregNumber { get; protected set; }
        public VocabularyKey Name { get; protected set; }
        public VocabularyKey FoundedDate { get; protected set; }
        public VocabularyKey RegistrationDate { get; protected set; }
        public VocabularyKey OrganizationType { get; protected set; }
        public VocabularyKey VoluntaryRegistered { get; protected set; }
        public VocabularyKey RegisteredImGoodsRegister { get; protected set; }
        public VocabularyKey RegisteredBusinessRegister { get; protected set; }
        public VocabularyKey RegisteredFoundingRegister { get; protected set; }
        public VocabularyKey NumberEmployees { get; protected set; }
        public VocabularyKey InstitutionSectorCode { get; protected set; }
        public VocabularyKey InstitutionSectorDescription { get; protected set; }
        public VocabularyKey IndustryCode { get; protected set; }
        public VocabularyKey IndustryDescription { get; protected set; }
        public VocabularyKey BankruptBool { get; protected set; }
        public VocabularyKey UnderLiquidation { get; protected set; }
        public VocabularyKey UnderLiquidationOrDissolution { get; protected set; }
        public VocabularyKey BrregUrl { get; protected set; }
        public VocabularyKey Website { get; protected set; }
        public BrregAddressVocabulary Address { get; protected set; }
        public BrregAddressVocabulary BusinessAddress { get; protected set; }
    }
}