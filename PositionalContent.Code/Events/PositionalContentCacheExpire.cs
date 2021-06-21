using Umbraco.Core;
using Umbraco.Core.Services;


namespace Hifi.PositionalContent
{
    public class PositionalContentCacheExpire : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            DataTypeService.Saved += ExpirePositionalContentCache;
        }

        private void ExpirePositionalContentCache(IDataTypeService sender, global::Umbraco.Core.Events.SaveEventArgs<global::Umbraco.Core.Models.IDataTypeDefinition> e)
        {
            foreach (var dataType in e.SavedEntities)
            {
                ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheItem(
                    Constants.CacheKey_GetTargetDataTypeDefinition + dataType.Id);
            }
        }
    }
}
