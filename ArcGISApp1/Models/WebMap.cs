using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ArcGISApp1.Models {

    public class WebMap {

        public string Id { get; set; }
        public string Title { get; set; }
        public ImageSource Thumbnail { get; set; }
        public string Snippet { get; set; }
    }
}
