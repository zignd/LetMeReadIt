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
        private const string ApiKey = "393079ba45bbe480bef924bb8df3b8a27b0f8491";

        #endregion

        private string _url;
        private bool _isLoading;
        private ParsedPage _currentPage;
        private ObservableCollection<ParsedPage> _previousPages;
        private ObservableCollection<ParsedPage> _nextPages;
        private bool _enableImages;
        private bool _parseLinksPages;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _previousPages = new ObservableCollection<ParsedPage>();
            _nextPages = new ObservableCollection<ParsedPage>();
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

        public ParsedPage CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (_currentPage != null)
                {
                    _previousPages.Add(_currentPage);
                    OnPropertyChanged("HasPreviousPages");
                }

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

        public async Task LoadCurrentPageFromUrlAsync(string url = null)
        {
            if (url != null)
                Url = url;

            var requestUri = new Uri(string.Format(Resource, ApiKey, Url));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var client = new HttpClient();
            var responseMessage = await client.SendRequestAsync(requestMessage);
            var json = await responseMessage.Content.ReadAsStringAsync();

            CurrentPage = JsonConvert.DeserializeObject<ParsedPage>(json);
        }

        public void LoadPreviousPage()
        {
            _nextPages.Add(CurrentPage);
            CurrentPage = _previousPages.Last();
            _previousPages.Remove(CurrentPage);

            OnPropertyChanged("HasPreviousPages");
            OnPropertyChanged("HasNextPages");
        }

        public void LoadNextPage()
        {
            _previousPages.Add(CurrentPage);
            CurrentPage = _nextPages.Last();
            _nextPages.Remove(CurrentPage);

            OnPropertyChanged("HasPreviousPages");
            OnPropertyChanged("HasNextPages");
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
