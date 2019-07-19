using ArcGISApp1.Models;
using ArcGISApp1.ViewModels;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Tasks.Offline;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ArcGISApp1.Components
{
    public static class WebMapManager
    {
        public static async Task<WebMap> GetMapAsync(string mapId)
        {
            if (string.IsNullOrWhiteSpace(mapId))
            {
                throw new ArgumentException("mapUrl is required");
            }

            ArcGISPortal arcGISOnline = await ArcGISPortal.CreateAsync();
            var portalItem = await PortalItem.CreateAsync(arcGISOnline, mapId);

            //Could not get this to work as expected
            //await DownloadMapAsync(portalItem);
            
            // load the model
            var model = new WebMap()
            {
                Id = mapId,
                Title = portalItem.Title,
                Snippet = portalItem.Snippet,
                Thumbnail = new BitmapImage(portalItem.ThumbnailUri)
            };

            return model;
        }

        private static async Task DownloadMapAsync(PortalItem portalItem)
        {
            Map onlineMap = new Map(portalItem);
            var takeMapOfflineTask = await OfflineMapTask.CreateAsync(onlineMap);

            // Create the job to generate an offline map, pass in the parameters and a path to
            //store the map package.
            var parameters = new GenerateOfflineMapParameters()
            {
                MaxScale = 5000,
                IncludeBasemap = true,
            };
            var path = MapAreaManager.GetOfflineMapPath("reference_base_map", true);

            GenerateOfflineMapJob generateMapJob =
            takeMapOfflineTask.GenerateOfflineMap(parameters, path);

            // Generate the offline map and download it.
            GenerateOfflineMapResult offlineMapResult = await generateMapJob.GetResultAsync();
            if (!offlineMapResult.HasErrors)
            {
                // Job completed successfully and all content was generated.
                Debug.WriteLine("Map " +
                offlineMapResult.MobileMapPackage.Item.Title +
                " was saved to " +
                offlineMapResult.MobileMapPackage.Path);
            }
            else
            {
                // Job is finished but one or more layers or tables had errors.
                if (offlineMapResult.LayerErrors.Count > 0)
                {
                    // Show layer errors.
                    foreach (var layerError in offlineMapResult.LayerErrors)
                    {
                        Debug.WriteLine("Error occurred when taking " +
                        layerError.Key.Name +
                        " offline. Error : " +
                        layerError.Value.Message);
                    }
                }
                if (offlineMapResult.TableErrors.Count > 0)
                {
                    // Show table errors.
                    foreach (var tableError in offlineMapResult.TableErrors)
                    {
                        Debug.WriteLine("Error occurred when taking " +
                        tableError.Key.TableName +
                        " offline. Error : " +
                        tableError.Value.Message);
                    }
                }
            }
        }

        internal static async Task<List<MapArea>> GetMapAreasAsync(string mapId)
        {
            ArcGISPortal arcGISOnline = await ArcGISPortal.CreateAsync();
            var portalItem = await PortalItem.CreateAsync(arcGISOnline, mapId);

            var offlineTask = await OfflineMapTask.CreateAsync(portalItem); 

            var mapAreas = await offlineTask.GetPreplannedMapAreasAsync();

            var model = new List<MapArea>();

            foreach (PreplannedMapArea mapArea in mapAreas)
            {
                await mapArea.LoadAsync();
                PortalItem preplannedMapItem = mapArea.PortalItem;
                var thumbnail = await preplannedMapItem.Thumbnail.ToImageSourceAsync();

                var path = MapAreaManager.GetOfflineMapPath(preplannedMapItem.ItemId);
                var offlineMapPackage = default(MobileMapPackage);
                try
                {
                    offlineMapPackage = await MobileMapPackage.OpenAsync(path);
                } catch(FileNotFoundException)
                {
                    /* this is expected when the map has not been downloaded yet */
                }
                
                model.Add(new MapArea()
                {
                    Id = preplannedMapItem.ItemId,
                    Title = preplannedMapItem.Title,
                    Thumbnail = thumbnail,
                    Snippet = preplannedMapItem.Snippet,
                    Payload = mapArea,
                    OfflineTask = offlineTask,
                    IsDownloaded = (offlineMapPackage != null),
                    IsDownloading = false
                });
            }

            return model;
        }
    }
}
