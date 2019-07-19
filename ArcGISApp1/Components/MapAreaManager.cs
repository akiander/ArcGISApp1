using ArcGISApp1.ViewModels;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Offline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISApp1.Components
{
    public class MapAreaManager
    {
        public static async Task<Map> DownloadMapAsync(MapArea mapArea)
        {
            mapArea.IsDownloading = true;

            var downloadParams =
                    await mapArea.OfflineTask.CreateDefaultDownloadPreplannedOfflineMapParametersAsync(mapArea.Payload);
            downloadParams.ContinueOnErrors = true;
            downloadParams.IncludeBasemap = true; //not sure why this seems to be ignored

            //downloadParams.ReferenceBasemapDirectory = GetOfflineMapPath("reference_base_map", true);
            //downloadParams.ReferenceBasemapFilename = "???";

            string path = GetOfflineMapPath(mapArea.Id);

            DownloadPreplannedOfflineMapJob preplannedMapJob =
                mapArea.OfflineTask.DownloadPreplannedOfflineMap(downloadParams, path);

            var preplannedMapResult = await preplannedMapJob.GetResultAsync();
            if (!preplannedMapResult.HasErrors)
            {
                // Job completed successfully and all content was generated.
                Debug.WriteLine("Map " +
                preplannedMapResult.MobileMapPackage.Item.Title +
                " was saved to " +
                preplannedMapResult.MobileMapPackage.Path);
            }
            else
            {
                // Job is finished but one or more layers or tables had errors.
                if (preplannedMapResult.LayerErrors.Count > 0)
                {
                    // Show layer errors.
                    foreach (var layerError in preplannedMapResult.LayerErrors)
                    {
                        Debug.WriteLine("Error occurred when taking " +
                        layerError.Key.Name +
                        " offline. Error : " +
                        layerError.Value.Message);
                    }
                }
                if (preplannedMapResult.TableErrors.Count > 0)
                {
                    // Show table errors.
                    foreach (var tableError in preplannedMapResult.TableErrors)
                    {
                        Debug.WriteLine("Error occurred when taking " +
                        tableError.Key.TableName +
                        " offline. Error : " +
                        tableError.Value.Message);
                    }
                }
            }

            mapArea.IsDownloading = false;

            return preplannedMapResult.OfflineMap;
        }

        public static string GetOfflineMapPath(string identifier, bool create = false)
        {
            var root = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Constants.AppName);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var path = Path.Combine(root, identifier);

            if (create && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        internal static void ClearMapsAsync()
        {
            var root = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Constants.AppName);

            foreach (var directory in Directory.EnumerateDirectories(root))
            {
                try
                {
                    Directory.Delete(Path.Combine(root, directory), true);
                } catch
                {
                    //Sometimes these map files are locked by the App
                }
            }
        }
    }
}
