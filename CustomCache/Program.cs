using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomCache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDataDownloader dataDownloader = new SlowDataDownloader();

            Console.WriteLine(dataDownloader.DownloadData("id1"));
            Console.WriteLine(dataDownloader.DownloadData("id2"));
            Console.WriteLine(dataDownloader.DownloadData("id3"));
            Console.WriteLine(dataDownloader.DownloadData("id1"));
            Console.WriteLine(dataDownloader.DownloadData("id3"));
            Console.WriteLine(dataDownloader.DownloadData("id1"));
            Console.WriteLine(dataDownloader.DownloadData("id2"));

            Console.ReadKey();
        }
        public class Cache<TID, TData>
        {
            private readonly Dictionary<TID, TData> _cacheStorage = new Dictionary<TID, TData>();

            public bool ContainsCache(TID id)
            {
                return _cacheStorage.ContainsKey(id);
            }
            public TData LoadCache(TID id)
            {
                return _cacheStorage[id];
            }
            public void SaveCache(TID id, TData data)
            {
                _cacheStorage.Add(id, data);
            }
        }
        public interface IDataDownloader
        {
            string DownloadData(string resourceId);
        }

        public class SlowDataDownloader : IDataDownloader
        {
            private Cache<string, string> _cache = new Cache<string, string>();
            public string DownloadData(string resourceId)
            {
                string resourceData = string.Empty;

                if (!_cache.ContainsCache(resourceId))
                {
                    resourceData = "Data of " + resourceId;
                    Thread.Sleep(1000);

                    _cache.SaveCache(resourceId, resourceData);
                }
                else
                {
                    resourceData = _cache.LoadCache(resourceId);
                }

                return $"Some data for {resourceId} - {resourceData}";
            }
        }
    }
}
