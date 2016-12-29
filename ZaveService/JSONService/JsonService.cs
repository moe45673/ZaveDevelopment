using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using ZaveModel.ZDF;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using Prism.Events;
using Microsoft.Practices.Unity;
using ZaveModel.ZDFEntry;

namespace ZaveService.JSONService
{
    public interface IJsonService
    {

        
        Task<ZDFSingleton> ReturnJsonAsZDFAsync(StreamReader file, List<string> jsonHeaderNames);
        JsonWriter SaveAsJson(StreamWriter file);
    }

    public class JsonService : IJsonService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;

        public JsonService(IEventAggregator agg, IUnityContainer cont)
        {
            _eventAggregator = agg;
            _container = cont;
        }



        private async Task<ZDFSingleton> ReturnJsonAsZDF(StreamReader file, List<string> jsonHeaderNames)


        {
            ZDFSingleton activeZDF;
            using (JsonReader wr = new JsonTextReader(file))
            {


                try
                {
                    JObject jObject = JObject.Load(wr);

                    activeZDF = _container.Resolve<ZDFSingleton>();

                    //activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());

                    JArray ja = (JArray)jObject[jsonHeaderNames[0]][jsonHeaderNames[1]];

                    await Task.Run(() => activeZDF.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZaveModel.ZDFEntry.ZDFEntry>>()));
                    activeZDF = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ZDFSingleton>(jObject.ToString()));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return activeZDF;
        }

        public JsonWriter SaveAsJson(StreamWriter file)
        {
            throw new NotImplementedException();
        }

        public Task<ZDFSingleton> ReturnJsonAsZDFAsync(StreamReader file, List<string> jsonHeaderNames)
        {
            return ReturnJsonAsZDF(file, jsonHeaderNames);
        }
    }
}
