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
using Diary.App.Models;

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
            containerRegistry.RegisterSingleton<DiaryDbContext>(() => new DiaryDbContext());
            containerRegistry.RegisterForNavigation<EditorView>("Editor");
            containerRegistry.RegisterForNavigation<StatisticsView>("Statistics");
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            Dialogs.Dialogs.ConfigAppDialogs(containerRegistry);
        }

        protected override Window CreateShell()
        {
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
                            regionMng.RequestNavigate(RegionNames.AppContentRegion, "Editor"))
                    };
                    AppMenuItem statistics = new AppMenuItem()
                    {
                        Title = "统计工具",
                        Command = new DelegateCommand(() =>
                            regionMng.RequestNavigate(RegionNames.AppContentRegion, "Statistics"))
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
    }
}
