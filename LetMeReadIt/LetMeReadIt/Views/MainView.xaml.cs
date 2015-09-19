using LetMeReadIt.Models;
using LetMeReadIt.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LetMeReadIt.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
            set { DataContext = value; }
        }

        public MainView()
        {
            ViewModel = new MainViewModel();

            this.InitializeComponent();
        }

        private async void MainWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            try
            {
                if (args.Uri != null)
                {
                    args.Cancel = true;
                    
                    await ViewModel.LoadCurrentPageFromUrlAsync(args.Uri.ToString());
                    MainWebView.NavigateToString(ViewModel.CurrentPage.Content);
                }
            }
            catch (Exception ex)
            {
                // TODO: handle it properly
            }
        }

        private async void UrlTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                try
                {
                    await ViewModel.LoadCurrentPageFromUrlAsync();
                    MainWebView.NavigateToString(ViewModel.CurrentPage.Content);
                }
                catch (Exception ex)
                {
                    // TODO: handle it properly
                }
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Url = "http://g1.globo.com/mundo/noticia/2015/09/papa-parte-rumo-havana-primeira-etapa-de-sua-viagem-cuba-e-eua.html";
        }

        private void NextPageMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.LoadNextPage();
            }
            catch (Exception ex)
            {
                // TODO: handle it properly
            }
        }

        private void PreviousPageMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.LoadPreviousPage();
            }
            catch (Exception ex)
            {
                // TODO: handle it properly
            }
        }
    }
}
