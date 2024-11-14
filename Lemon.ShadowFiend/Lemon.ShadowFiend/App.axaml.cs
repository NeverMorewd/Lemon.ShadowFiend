using Avalonia;
using Avalonia.Markup.Xaml;
using System;

namespace Lemon.ShadowFiend
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public IServiceProvider ServiceProvider { get; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}