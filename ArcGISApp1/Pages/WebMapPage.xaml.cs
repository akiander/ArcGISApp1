using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using ArcGISApp1.ViewModels;
using System.Threading.Tasks;
using Windows.Storage;
using Esri.ArcGISRuntime.Tasks.Offline;
using System.Diagnostics;
using ArcGISApp1.Components;

namespace ArcGISApp1.Pages
{
	/// <summary>
	/// Displays a WebMap and all related Area Maps
	/// </summary>
	public sealed partial class WebMapPage : Page
	{
		public WebMapPage()
		{
			this.InitializeComponent();
            this.ViewModel = new WebMapModel();
		}

        public WebMapModel ViewModel { get; private set; }

        private void WebMapOpen_Click(object sender, RoutedEventArgs e)
        {
            string mapId = ((Button)sender)?.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(mapId))
            {
                return; //TODO: Log this
            }

            var parameters = new MapParameters()
            {
                MapId = mapId
            };

            this.Frame.Navigate(typeof(MapViewer), parameters);

        }

        private async void MapAreaGetButton_Click(object sender, RoutedEventArgs e)
        {
            string areaId = ((Button)sender)?.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(areaId))
            {
                return; //TODO: Log this
            }

            var area = this.ViewModel.MapAreasRaw.FirstOrDefault(m => m.Id == areaId);    

            var offlineMap = await MapAreaManager.DownloadMapAsync(area);

            //Capture the state
            area.IsDownloaded = true;
        }

        private async void OpenMapArea_Click(object sender, RoutedEventArgs e)
        {
            string areaId = ((Button)sender)?.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(areaId))
            {
                return; //TODO: Log this
            }

            var area = this.ViewModel.MapAreasRaw.FirstOrDefault(m => m.Id == areaId);
            var path = MapAreaManager.GetOfflineMapPath(areaId);
            MobileMapPackage offlineMapPackage = await MobileMapPackage.OpenAsync(path);

            var parameters = new MapParameters()
            {
                Map = offlineMapPackage.Maps.FirstOrDefault()
            };

            this.Frame.Navigate(typeof(MapViewer), parameters);
        }

        private void ClearCachedMapsButton_Click(object sender, RoutedEventArgs e)
        {
            MapAreaManager.ClearMapsAsync();
            foreach (var area in this.ViewModel.MapAreasRaw)
            {
                area.IsDownloaded = false;
            }
        }
    }
}
