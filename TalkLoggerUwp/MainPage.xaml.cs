using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tono.Gui.Uwp;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TalkLoggerUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;

            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            LOG.AddMes(LLV.INF, "Start-Welcome", $"{ver.Build}.{ver.Major}.{ver.Minor}.{ver.Revision}", DateTime.Now.Year);
            LOG.AddMes(LLV.INF, new LogAccessor.Image { Key = "Lump16" }, "Start-Quickhelp");
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            GuiView.ZoomX = ChatPanel.ZoomX = 22.269168853759766;
            GuiView.ScrollX = ChatPanel.ScrollX = ChatPanel.Rect.L + ChatPanel.Rect.Width * 0.5 / ChatPanel.ZoomX;
        }
    }
}
