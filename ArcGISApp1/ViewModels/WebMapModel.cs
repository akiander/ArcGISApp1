using ArcGISApp1.Components;
using ArcGISApp1.Models;
using ArcGISApp1.Utils;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace ArcGISApp1.ViewModels {

    public class WebMapModel : BindableBase{

        public WebMapModel() {

            this.WebMap = new NotifyTaskCompletion<WebMap>(WebMapManager.GetMapAsync(Constants.MapId_ExploreMaine));
            this.WebMap.PropertyChanged += WebMap_PropertyChanged;

            this.MapAreas = new NotifyTaskCompletion<List<MapArea>>(WebMapManager.GetMapAreasAsync(Constants.MapId_ExploreMaine));
            this.MapAreas.PropertyChanged += MapAreas_PropertyChanged;
        }

        private void MapAreas_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var result = sender as NotifyTaskCompletion<List<MapArea>>;
            if (e.PropertyName == "IsSuccessfullyCompleted"
                && result.Status == TaskStatus.RanToCompletion)
            {
                this.MapAreasRaw = new ObservableCollection<MapArea>(this.MapAreas.Result);
            }
        }

        private void WebMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var result = sender as NotifyTaskCompletion<WebMap>;
            if (e.PropertyName == "IsSuccessfullyCompleted"
                && result.Status == TaskStatus.RanToCompletion)
            {
                this.Id = this.WebMap.Result.Id;
                this.Title = this.WebMap.Result.Title;
                this.Snippet = this.WebMap.Result.Snippet;
                this.Thumbnail = this.WebMap.Result.Thumbnail;
            }
        }

        public NotifyTaskCompletion<WebMap> WebMap { get; private set; }
        public NotifyTaskCompletion<List<MapArea>> MapAreas { get; private set; }

        public string Id {
            get { return _id; }
            set { if (SetProperty(ref _id, value)) OnPropertyChanged(nameof(this.Id)); }
        }
        private string _id;

        public string Title {
            get { return _title; }
            set { if (SetProperty(ref _title, value)) OnPropertyChanged(nameof(this.Title)); }
        }
        private string _title;

        public string Snippet {
            get { return _snippet; }
            set { if (SetProperty(ref _snippet, value)) OnPropertyChanged(nameof(this.Snippet)); }
        }
        private string _snippet;

        public ImageSource Thumbnail {
            get { return _thumbnail; }
            set { if (SetProperty(ref _thumbnail, value)) OnPropertyChanged(nameof(this.Thumbnail)); }
        }
        private ImageSource _thumbnail;

        public ObservableCollection<MapArea> MapAreasRaw {
            get { return _mapAreasRaw;  }
            set { if (SetProperty(ref _mapAreasRaw, value)) OnPropertyChanged(nameof(this.MapAreasRaw)); }
        }
        private ObservableCollection<MapArea> _mapAreasRaw;


    }
}
