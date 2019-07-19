using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISApp1.Components {
    public static class MapManager {

        public static async Task<Map> GetMapAsync(string mapUrl) {

            if (string.IsNullOrWhiteSpace(mapUrl)) {
                throw new ArgumentException("mapUrl is required");
            }

            var map = await Map.LoadFromUriAsync(new Uri(mapUrl));

            return map;
        }
    }
}
