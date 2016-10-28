﻿using Abp.Application.Services;
using Abp.Localization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Languages
{
    public interface ILanguageAppService : IApplicationService
    {

        Task AddLanguage(LanguageInput input);
        ReturnModel<LanguageDto> GetLanguagesForTable(RequestModel<object> input);
        ReturnModel<LanguageTextTableElement> GetLocalizationTexts(RequestModel<LanguageTextsForEditRequest> input);
        LanguageTextsForEditView GetLanguageTextsForEditView(string selectedTargetLanguage,
            string selectedSourceLanguage);

        Task DeleteLanguage(string code);
        Task AddEditLocalizationText(LocalizationTextInput input);


        Task UpdateLanguageFromXml(string languageName, string source, bool updateExistingValues = false);

        IReadOnlyList<LanguageInfo> GetLanguages();
    }
}
