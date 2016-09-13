﻿using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Cinotam.AbpModuleZero.Web.Models.Layout;
using Cinotam.Cms.App.Pages;
using Cinotam.ModuleZero.AppModule.Sessions;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class LayoutController : AbpModuleZeroControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IPagesService _pagesService;
        public LayoutController(
            IUserNavigationManager userNavigationManager,
            ILocalizationManager localizationManager,
            ISessionAppService sessionAppService,
            IMultiTenancyConfig multiTenancyConfig, IPagesService pagesService)
        {
            _userNavigationManager = userNavigationManager;
            _localizationManager = localizationManager;
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _pagesService = pagesService;
        }

        [ChildActionOnly]
        public PartialViewResult TopMenu(string activeMenu = "")
        {
            var model = new TopMenuViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
                ActiveMenuItemName = activeMenu
            };

            return PartialView("_TopMenu", model);
        }

        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _localizationManager.CurrentLanguage,
                Languages = _localizationManager.GetAllLanguages()
            };

            return PartialView("_LanguageSelection", model);
        }


        [ChildActionOnly]
        public PartialViewResult UserMenuOrLoginLink()
        {
            UserMenuOrLoginLinkViewModel model;

            if (AbpSession.UserId.HasValue)
            {
                model = new UserMenuOrLoginLinkViewModel
                {
                    LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations()),
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                };
            }
            else
            {
                model = new UserMenuOrLoginLinkViewModel
                {
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
                };
            }

            return PartialView("_UserMenuOrLoginLink", model);
        }

        public ActionResult PagesMenu()
        {
            var menu = _pagesService.GetPagesMenu();
            return View(menu);
        }
    }
}