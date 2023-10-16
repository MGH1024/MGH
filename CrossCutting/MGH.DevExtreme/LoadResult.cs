using System.ComponentModel;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace MGH.DevExtreme
{
    public sealed class LoadResult<TEntity> : IConvertToActionResult
    {
        private readonly LoadResult _loadResult;
        public LoadResult(LoadResult loadResult)
        {
            this._loadResult = loadResult;
        }

      

        public static implicit operator LoadResult<TEntity>(LoadResult value)
        {
            return new LoadResult<TEntity>(value);
        }

        public IActionResult Convert()
        {
            return new OkObjectResult(_loadResult);
        }

        public IEnumerable<TEntity> data { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int totalCount { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]

        public int groupCount { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object[] summary { get; set; }
    }
}
