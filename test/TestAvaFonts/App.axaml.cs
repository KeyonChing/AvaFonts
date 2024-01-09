using Avalonia;
using Avalonia.Benchmarks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace TestAvaFonts
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void RegisterServices()
        {
            var fmi = new AvaloniaFontsManagerImpl(new AvaloniaFontsSettings() { DefaultFontName = "Harmony" });//若字体文件夹为自定义名称，请赋值给 DefaultFontDirectory，值为绝对路径
            AvaloniaLocator.CurrentMutable.Bind<IFontManagerImpl>().ToConstant(fmi);
            var list = fmi.InstalledFont;//可查看已经识别到的字体
            base.RegisterServices();
        }
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}