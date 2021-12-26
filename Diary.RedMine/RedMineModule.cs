using System;
using Diary.Core;
using Prism.Commands;

namespace Diary.RedMine;

public class RedMineModule: IDiaryModule
{
    public override MenuNode? GetMenu()
    {
        MenuNode root = new MenuNode()
        {
            Title = "_RedMine",
            Command = null
        };

        for (int i = 0; i < 5; ++i)
            root.Children.Add(new MenuNode()
            {
                Title = $"Child #{i}",
                Command = new DelegateCommand(()=>{ Console.WriteLine("123"); })
            });

        return root;
    }
}