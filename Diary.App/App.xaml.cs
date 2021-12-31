using Diary.App.Views;
using Diary.Core.Base;
using Prism.Ioc;
using Prism.Modularity;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Diary.App.Database;

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
    }
}
