using Diary.App.Database;
using Diary.App.Modules.RedMine;
using Diary.App.Views;
using Diary.App.Windows;
using Diary.Core.Base;
using Diary.Core.Constant;
using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Diary.App.Events;
using Diary.App.Models;
using MahApps.Metro.IconPacks;
using Microsoft.EntityFrameworkCore;
using Prism.Events;

namespace Diary.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplication>(() => new DiaryApp());
            // containerRegistry.RegisterSingleton<DiaryDbContext>(() => new DiaryDbContext());
            containerRegistry.Register<IAppDbContext>(() => new DiaryDbContext());
            containerRegistry.RegisterForNavigation<EditorView>("Editor");
            containerRegistry.RegisterForNavigation<StatisticsView>("Statistics");
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialogWindow<MessageDialogWindow>("msg");
            Dialogs.Dialogs.ConfigAppDialogs(containerRegistry);
        }

        protected override Window CreateShell()
        {
            // Init Database
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Directory.CreateDirectory($@"{appDataDir}/{appName}");
            new DiaryDbContext().Database.Migrate();

            return Container.Resolve<ShellWindow>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directories = new[] { "Modules", "Services" };

            var components = new List<IModuleCatalogItem>();

            foreach (var dir in directories)
            {
                var moduleDir = $"{path}/{dir}";
                if (!Directory.Exists(moduleDir))
                    continue;
                var dirCatalog = new DirectoryModuleCatalog() { ModulePath = moduleDir };
                dirCatalog.Initialize();

                components.AddRange(dirCatalog.Items);
            }

            var catalog = new ModuleCatalog();

            foreach (var com in components)
            {
                catalog.Items.Add(com);
            }

            return catalog;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // addition functions
            {
                var app = Container.Resolve<IApplication>();
                var regionMng = Container.Resolve<IRegionManager>();
                if (app != null)
                {
                    AppMenuItem editor = new AppMenuItem()
                    {
                        Title = "编辑器",
                        Command = new DelegateCommand(() =>
                            regionMng.RequestNavigate(RegionNames.AppContentRegion, "Editor")),
                        Icon = AppMenuItem.CreateCanvasFromPackIcon(new PackIconMaterial(){Kind = PackIconMaterialKind.CommentEdit})
                    };
                    AppMenuItem statistics = new AppMenuItem()
                    {
                        Title = "统计工具",
                        Command = new DelegateCommand(() =>
                            regionMng.RequestNavigate(RegionNames.AppContentRegion, "Statistics")),
                        Icon = AppMenuItem.CreateCanvasFromPackIcon(new PackIconFontisto(){Kind = PackIconFontistoKind.AreaChart})
                    };

                    app.RegisterMenu(new[] { editor, statistics });
                }
            }



            // Static Modules
            Type moduleType = typeof(RedMineModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName!
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppSettings.Store(AppSettings.GetConfig());
            base.OnExit(e);
        }
        
        protected override void OnInitialized()
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = AppDomain.CurrentDomain.FriendlyName;

            Directory.CreateDirectory($@"{appDataDir}/{appName}");

            base.OnInitialized();
            Container.Resolve<IEventAggregator>().GetEvent<AppStartedEvent>().Publish();
        }
    }
}
