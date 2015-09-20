using LetMeReadIt.Common;
using LetMeReadIt.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace LetMeReadIt.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        #region Constants

        private const string Resource = "https://readability.com/api/content/v1/parser?token={0}&url={1}";

        #endregion

        private string _url;
        private bool _isLoading;
        private LetMePage _currentPage;
        private ObservableCollection<LetMePage> _previousPages;
        private ObservableCollection<LetMePage> _nextPages;
        private bool _enableImages;
        private bool _parseLinksPages;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _previousPages = new ObservableCollection<LetMePage>();
            _nextPages = new ObservableCollection<LetMePage>();

            EnableImages = true;
            ParseLinksPages = true;
        }

        #endregion

        #region Properties

        public string Url
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public LetMePage CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("HasCurrentPage");
            }
        }
        
        public bool HasCurrentPage
        {
            get { return CurrentPage != null; }
        }

        public bool HasPreviousPages
        {
            get { return _previousPages.Count > 0; }
        }

        public bool HasNextPages
        {
            get { return _nextPages.Count > 0; }
        }

        #region Settings
        
        public bool EnableImages
        {
            get { return _enableImages; }
            set { _enableImages = value; OnPropertyChanged(); }
        }

        public bool ParseLinksPages
        {
            get { return _parseLinksPages; }
            set { _parseLinksPages = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        #region Public Methods

        public async Task LoadCurrentPageAsync(string url = null)
        {
            if (url != null)
                Url = url;

            if (CurrentPage != null)
            {
                _previousPages.Add(_currentPage);
                _nextPages.Clear();

                OnPropertyChanged("HasPreviousPages");
                OnPropertyChanged("HasNextPages");
            }

            if (ParseLinksPages)
            {
                var json = await ParsePageAsync(Url);
                CurrentPage = new LetMePage { IsParsed = true, ParsedPage = JsonConvert.DeserializeObject<ParsedPage>(json) };
                CurrentPage.ParsedPage.Content = "<h2>" + CurrentPage.ParsedPage.Title + "</h2><hr/>" + CurrentPage.ParsedPage.Content;
            }
            else
            {
                CurrentPage = new LetMePage { IsParsed = false, NotParsedPage = new Uri(Url) };
            }
        }

        public void LoadPreviousPage()
        {
            _nextPages.Add(CurrentPage);
            CurrentPage = _previousPages.Last();
            _previousPages.Remove(CurrentPage);

            Url = CurrentPage.IsParsed ? CurrentPage.ParsedPage.Url : CurrentPage.NotParsedPage.ToString();

            OnPropertyChanged("HasPreviousPages");
            OnPropertyChanged("HasNextPages");
        }

        public void LoadNextPage()
        {
            _previousPages.Add(CurrentPage);
            CurrentPage = _nextPages.Last();
            _nextPages.Remove(CurrentPage);

            Url = CurrentPage.IsParsed ? CurrentPage.ParsedPage.Url : CurrentPage.NotParsedPage.ToString();

            OnPropertyChanged("HasPreviousPages");
            OnPropertyChanged("HasNextPages");
        }

        public async Task RefreshCurrentPageAsync()
        {
            var json = await ParsePageAsync(CurrentPage.ParsedPage.Url);
            CurrentPage = new LetMePage { IsParsed = true, ParsedPage = JsonConvert.DeserializeObject<ParsedPage>(json) };
            CurrentPage.ParsedPage.Content = "<h2>" + CurrentPage.ParsedPage.Title + "</h2><hr/>" + CurrentPage.ParsedPage.Content;
        }

        #endregion

        #region Private Methods

        private async Task<string> ParsePageAsync(string url)
        {
            var requestUri = new Uri(string.Format(Resource, Settings.ApiKey, url));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var client = new HttpClient();
            var responseMessage = await client.SendRequestAsync(requestMessage);
            var json = await responseMessage.Content.ReadAsStringAsync();

            return json;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
