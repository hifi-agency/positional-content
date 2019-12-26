using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Core.Cache;
using Umbraco.Web.Composing;

namespace Hifi.PositionalContent
{
    public class PositionalContentCacheExpire : Umbraco.Core.Composing.IComponent
    {
        public void Initialize()
        {
            DataTypeService.Saved += (ds, e) =>
            {
                foreach(var dataType in e.SavedEntities)
                {
                    Current.AppCaches.RuntimeCache.ClearByKey(Constants.CacheKey_GetTargetDataTypeDefinition + dataType.Id);
                }
            };
        }

        public void Terminate()
        { }
    }
}
