using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Hifi.PositionalContent
{
    public class PositionalContentCacheExpire : INotificationAsyncHandler<DataTypeSavedNotification>
    {
        private readonly AppCaches _caches;

        public PositionalContentCacheExpire(AppCaches caches)
        {
            _caches = caches;
        }
        public async Task HandleAsync(DataTypeSavedNotification notification, CancellationToken cancellationToken)
        {
            foreach(var dataType in notification.SavedEntities)
            {
                _caches.RuntimeCache.ClearByKey(Constants.CacheKey_GetTargetDataTypeDefinition + dataType.Id);
            }
        }
    }
}
