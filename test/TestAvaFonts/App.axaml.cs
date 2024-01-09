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
            var fmi = new AvaloniaFontsManagerImpl(new AvaloniaFontsSettings() { DefaultFontName = "Harmony" });//�������ļ���Ϊ�Զ������ƣ��븳ֵ�� DefaultFontDirectory��ֵΪ����·��
            AvaloniaLocator.CurrentMutable.Bind<IFontManagerImpl>().ToConstant(fmi);
            var list = fmi.InstalledFont;//�ɲ鿴�Ѿ�ʶ�𵽵�����
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