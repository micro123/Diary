using Prism.Ioc;
using Prism.Modularity;

namespace Diary.Core.Base
{
    public abstract class DiaryModuleBase : IModule
    {
        // 返回模块提供的菜单
        public virtual MenuNode? GetMenu()
        {
            return null;
        }

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IApplication app = containerProvider.Resolve<IApplication>();
            if (app != null && GetMenu() != null)
            {
                app.RegisterMenu(GetMenu());
            }
        }
    }
}
