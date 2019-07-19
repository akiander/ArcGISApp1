using ArcGISApp1.ViewModels;
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

namespace ArcGISApp1.Pages {

    public sealed partial class MapViewer : Page {

        public MapViewer() {
            this.InitializeComponent();
        }

        public MapViewerModel ViewModel { get; } = new MapViewerModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (MapParameters)e.Parameter;

            if (!string.IsNullOrWhiteSpace(parameters.MapId))
            {
                this.ViewModel.Load(parameters.MapId);
            } else if (parameters.Map != null)
            {
                this.ViewModel.Load(parameters.Map);
            }

            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
