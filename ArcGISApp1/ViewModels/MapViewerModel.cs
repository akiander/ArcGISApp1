using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.UI;
using ArcGISApp1.Components;
using ArcGISApp1.Utils;
using ArcGISApp1.ViewModels;

namespace ArcGISApp1 {

    public class MapViewerModel : BindableBase {

        public MapViewerModel() {
            
        }

        public void Load(string mapId)
        {
            var mapUrl = $"{Constants.MapBaseAddress}?id={mapId}";
            this.Map = new NotifyTaskCompletion<Map>(MapManager.GetMapAsync(mapUrl));
            this.Map.PropertyChanged += Map_PropertyChanged;
        }

        private void Map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var result = sender as NotifyTaskCompletion<Map>;
            if (e.PropertyName == "IsSuccessfullyCompleted"
                && result.Status == TaskStatus.RanToCompletion)
            {
                this.MapRaw = this.Map.Result;
            }
        }

        public void Load(Map map)
        {
            this.MapRaw = map;
        }

        public NotifyTaskCompletion<Map> Map { get; private set; }

        public Map MapRaw {
            get { return _mapRaw; }
            set { if (SetProperty(ref _mapRaw, value)) OnPropertyChanged(nameof(this.MapRaw)); }
        }
        private Map _mapRaw;

    }
}
