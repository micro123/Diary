using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Modularity;

namespace Diary.Core
{
    public abstract class IDiaryModule: IModule
    {
        // 返回模块提供的菜单
        public virtual MenuNode? GetMenu()
        {
            return null;
        }

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            IApplication app = containerProvider.Resolve<IApplication>();
            if (app != null && GetMenu() != null)
            {
                app.RegisterMenu(GetMenu());
            }
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
