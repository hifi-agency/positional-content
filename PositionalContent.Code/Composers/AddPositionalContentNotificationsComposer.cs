using Hifi.PositionalContent;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace PositionalContent.Code.Composers;



public class AddPositionalContentNotificationsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationAsyncHandler<DataTypeSavedNotification, PositionalContentCacheExpire>();
        builder.AddNotificationHandler<ServerVariablesParsingNotification, PositionalContentServerVariableParser>();
        

    }
}