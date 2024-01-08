# AvaFonts
一个应用AvaloniaUI的自定义字体类库

# 使用说明
### 使用场景
  - 使用AvaloniaUI生成的桌面应用程序(其他平台尚未测试是否可用，请自行下载测试)
### 版本说明
  - .net版本：.net standard 2.0 (.NET 和.NET Core	2.0、2.1、2.2、3.0、3.1、5.0、6.0、7.0、8.0)
  - avalonia版本：支持11.0以上版本(截至发布前，AvaloniaUI已发布了11.0.6版本，实测可用，若新版本发布后这个库不再可用，请告知我)

# 准备工作
### 1.准备字体文件
  - 首先，在项目根目录中添加文件夹"AvaFonts"（自定义文件夹名称也可以）
  - 文件目录结构：
    ```csharp
    ──Avalonia\        //根目录
    ├── *.ttf        // 单样式字体，若在xaml中样式设置为粗体和斜体，会通过字体模拟器渲染
    ├── *.ttf
    ├── *.ttf
    ├── *.ttf\        //多样式字体，以文件夹方式表示，文件夹名最后要加.ttf，
        ├──*-Bold.ttf      //多样式字体文件的子字体，支持各种样式和斜体
        ├──*-Thin.ttf
        ├──*-italic.ttf
    ```
    ![image](https://github.com/KeyonChing/AvaFonts/assets/139779749/9eea1c82-dc12-43e1-a716-542fab2f564f)

  - 文件名与字体对应关系：
      - 单样式字体
    ```csharp
      文件名：Ava.ttf
      xaml：Ava        //xaml FontFamily="Ava"
                       //可加入样式，仅支持Bold和Italic
     ```
    
      - 多样式字体
    ```C#
     文件夹： Ava.ttf\
             ├── Ava.ttf            //xaml FontFamily="Ava"   必须要有Normal字体
             ├── Ava-Bold.ttf      //xaml FontFamily="Ava" FontWeight="Bold"
             ├── Ava-Light.ttf      //xaml FontFamily="Ava" FontWeight="Light"
             ├── Ava-italic.ttf      //xaml FontFamily="Ava" FontStyle="Italic"
     ```
### 2.修改项目文件
      - 加入以下代码：
      
```csharp
    <AvaloniaAccessUnstablePrivateApis>true</AvaloniaAccessUnstablePrivateApis
    <Avalonia_I_Want_To_Use_Private_Apis_In_Nuget_Package_And_Promise_To_Pin_The_Exact_Avalonia_Version_In_Package_Dependency>true</Avalonia_I_Want_To_Use_Private_Apis_In_Nuget_Package_And_Promise_To_Pin_The_Exact_Avalonia_Version_In_Package_Dependency>
    <IsPackable>true</IsPackable>
    
    <ItemGroup>
    <None Update="AvaFonts\*\*.ttf">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update="AvaFonts\*.ttf">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    </ItemGroup>
```
    
### 2.using & 注册字体管理器
  - 打开App.axaml.cs
```csharp
    
    using Avalonia.Benchmarks;

      public override void RegisterServices()
      {
      var fmi = new AvaloniaFontsManagerImpl(new AvaloniaFontsSettings() { DefaultFontName = "Harmony" });//若字体文件夹为自定义名称，请赋值给 DefaultFontDirectory，值为绝对路径
      AvaloniaLocator.CurrentMutable.Bind<IFontManagerImpl>().ToConstant(fmi);
      var list = fmi.InstalledFont;//可查看已经识别到的字体
      base.RegisterServices();
      }
```


    
