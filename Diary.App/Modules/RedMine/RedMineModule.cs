using Diary.App.Modules.RedMine.Dialogs;
using Diary.App.Modules.RedMine.Views;
using Diary.Core.Base;
using Diary.Core.Constant;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;

namespace Diary.App.Modules.RedMine;

public class RedMineModule : DiaryModuleBase
{
    private readonly IContainerProvider _container;

    public RedMineModule(IContainerProvider container)
    {
        _container = container;
    }

    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<RedMineSettingView>("RedMineSettings");
        containerRegistry.RegisterDialog<RedMineIssueImportView, RedMineIssueImportViewModel>("RedMineIssueImport");
    }

    public override IEnumerable<AppMenuItem>? GetMenu()
    {
        AppMenuItem appMenuRoot = new AppMenuItem()
        {
            Title = "RedMine 工具",
            Command = new DelegateCommand(ShowRedMineActivitiesImportDlg),
            Icon = AppMenuItem.CreateCanvasFromPackIcon(new PackIconSimpleIcons() { Kind = PackIconSimpleIconsKind.Redmine })
        };
        return new[] { appMenuRoot };
    }

    private void ShowRedMineActivitiesImportDlg()
    {
        var regionManager = _container.Resolve<IRegionManager>();
        if (regionManager == null)
            throw new NullReferenceException("No Region Manager Found!!");

        regionManager
            .RequestNavigate(RegionNames.AppContentRegion, "RedMineSettings");
    }
}