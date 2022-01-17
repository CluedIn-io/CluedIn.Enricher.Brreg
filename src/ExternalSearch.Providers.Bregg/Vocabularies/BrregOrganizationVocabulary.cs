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
            VocabularyName = "Brreg Organization";
            KeyPrefix      = "brreg.organization";
            KeySeparator   = ".";
            Grouping       = EntityType.Organization;

            AddGroup("Metadata", group =>
            {
                Name                               = group.Add(new VocabularyKey("name"));
                BrregNumber                        = group.Add(new VocabularyKey("brregNumber",                        VocabularyKeyDataType.Number));
                FoundedDate                        = group.Add(new VocabularyKey("foundedDate",                        VocabularyKeyDataType.DateTime));
                RegistrationDate                   = group.Add(new VocabularyKey("registrationDate",                   VocabularyKeyDataType.DateTime));
                OrganizationType                   = group.Add(new VocabularyKey("organizationType"));
                OrganizationTypeFull               = group.Add(new VocabularyKey("organizationTypeFull",               VocabularyKeyDataType.Json,         VocabularyKeyVisibility.Hidden));
                VoluntaryRegistered                = group.Add(new VocabularyKey("voluntaryRegistered",                VocabularyKeyDataType.Boolean));
                RegisteredImGoodsRegister          = group.Add(new VocabularyKey("registeredImGoodsRegister",          VocabularyKeyDataType.Boolean));
                RegisteredBusinessRegister         = group.Add(new VocabularyKey("registeredBusinessRegister",         VocabularyKeyDataType.Boolean));
                RegisteredFoundingRegister         = group.Add(new VocabularyKey("registeredFoundingRegister",         VocabularyKeyDataType.Boolean));
                NumberEmployees                    = group.Add(new VocabularyKey("numberEmployees",                    VocabularyKeyDataType.Integer));
                BankruptBool                       = group.Add(new VocabularyKey("bankrupt",                           VocabularyKeyDataType.Boolean));
                UnderLiquidation                   = group.Add(new VocabularyKey("underLiquidation",                   VocabularyKeyDataType.Boolean));
                UnderLiquidationOrDissolution      = group.Add(new VocabularyKey("underLiquidationOrDissolution",      VocabularyKeyDataType.Boolean));
                LatestFiledAnnualAccounts          = group.Add(new VocabularyKey("latestFiledAnnualAccounts",          VocabularyKeyDataType.Boolean));
                LanguageVariant                    = group.Add(new VocabularyKey("languageVariant",                    VocabularyKeyDataType.Boolean));
                BrregUrl                           = group.Add(new VocabularyKey("brregUrl",                           VocabularyKeyDataType.Uri));
                Website                            = group.Add(new VocabularyKey("website",                            VocabularyKeyDataType.Uri));
            });

            AddGroup("Location", group =>
            {
                Address                        = group.Add(new BrregAddressVocabulary().AsCompositeKey("postAddress"));
                BusinessAddress                = group.Add(new BrregAddressVocabulary().AsCompositeKey("businessPostAddress"));

            });

            AddGroup("Details", group =>
            {
                IndustryCode                       = group.Add(new VocabularyKey("industryCode",                       VocabularyKeyDataType.Number));
                IndustryDescription                = group.Add(new VocabularyKey("industryDescription"));
                InstitutionSectorCode              = group.Add(new VocabularyKey("institutionSectorCode",              VocabularyKeyDataType.Number));
                InstitutionSectorDescription       = group.Add(new VocabularyKey("institutionSectorDescription"));
            });

            AddMapping(BrregNumber,                   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg);
            AddMapping(NumberEmployees,               CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.EmployeeCount);
            AddMapping(FoundedDate,                   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.FoundingDate);
            AddMapping(Website,                       CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);

            AddMapping(BusinessAddress.Address,       CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address);
            AddMapping(BusinessAddress.CountryCode,   CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode);
            AddMapping(BusinessAddress.PostalCode,    CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            AddMapping(BusinessAddress.PostalArea,    CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressPostalArea);

            AddMapping(IndustryDescription,           CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Industry);
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