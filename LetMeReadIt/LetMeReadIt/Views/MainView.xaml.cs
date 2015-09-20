using LetMeReadIt.Models;
using LetMeReadIt.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
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
        private ShareOperation _shareOperation;
        //private bool _isLinkFromTextBox;
        private bool _isFromNavigationButton;

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

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPage.IsParsed)
            {
                await ViewModel.RefreshCurrentPageAsync();
                MainWebView.NavigateToString(ViewModel.CurrentPage.ParsedPage.Content);
            }
            else
            {
                MainWebView.NavigateToString(ViewModel.CurrentPage.NotParsedPage.ToString());
            }
        }

        private async void MainWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            try
            {
                // is coming from click on page
                if (_isFromNavigationButton == false && args.Uri != null)
                {
                    if (ViewModel.ParseLinksPages)
                    {
                        args.Cancel = true;

                        await ViewModel.LoadCurrentPageAsync(args.Uri.ToString());
                        NavigateToCurrentPage(false);
                    }
                    else
                    {
                        await ViewModel.LoadCurrentPageAsync(args.Uri.ToString());
                    }
                }

                _isFromNavigationButton = false;
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
                    await ViewModel.LoadCurrentPageAsync();
                    NavigateToCurrentPage(false);
                }
                catch (Exception ex)
                {
                    // TODO: handle it properly
                }
            }
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Url = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            }
            catch (Exception ex)
            {

            }
        }

        private void NextPageMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.LoadNextPage();
                NavigateToCurrentPage();
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
                NavigateToCurrentPage();
            }
            catch (Exception ex)
            {
                // TODO: handle it properly
            }
        }
        
        private void NavigateToCurrentPage(bool isFromNavigationButton = true)
        {
            _isFromNavigationButton = isFromNavigationButton;

            if (ViewModel.CurrentPage.IsParsed)
                MainWebView.NavigateToString(ViewModel.CurrentPage.ParsedPage.Content);
            else
                MainWebView.Navigate(ViewModel.CurrentPage.NotParsedPage);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _shareOperation = e.Parameter as ShareOperation;

            if (_shareOperation != null)
            {
                ViewModel.Url = (await _shareOperation.Data.GetWebLinkAsync()).ToString();
                await ViewModel.LoadCurrentPageAsync();

                if (ViewModel.CurrentPage.IsParsed)
                    MainWebView.NavigateToString(ViewModel.CurrentPage.ParsedPage.Content);
                else
                    MainWebView.Navigate(ViewModel.CurrentPage.NotParsedPage);
            }
        }
    }
}
