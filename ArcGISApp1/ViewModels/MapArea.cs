using ArcGISApp1.ViewModels;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Offline;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ArcGISApp1.ViewModels {

    public class MapArea : BindableBase {

        public string Id { get; set; }
        public string Title { get; set; }
        public ImageSource Thumbnail { get; set; }
        public string Snippet { get; set; }
        public Map Map { get; set; }

        public bool IsDownloaded {
            get { return _isDownloaded; }
            set { if (SetProperty(ref _isDownloaded, value)) OnPropertyChanged(nameof(this.IsDownloaded)); }
        }
        private bool _isDownloaded;

        public bool IsDownloading {
            get { return _isDownloading; }
            set { if (SetProperty(ref _isDownloading, value)) OnPropertyChanged(nameof(this.IsDownloading)); }
        }
        private bool _isDownloading;

        public PreplannedMapArea Payload { get; set; }
        public OfflineMapTask OfflineTask { get; set; }


    }
}
