using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public abstract class PositionalContentModelBase
    {
        public virtual object content { get; set; }

        public virtual object settings { get; set; }

        [JsonIgnore]
        public virtual bool HasContent
        {
            get
            {
                return content != null && content.ToString() != string.Empty;
            }
        }

        [JsonIgnore]
        public virtual bool HasSettings
        {
            get
            {
                return settings != null && settings.ToString() != string.Empty;
            }
        }

        protected virtual T Content<T>(Guid dtdGuid, PositionalContentDataTypes type)
        {
            return PositionalContentHelper.ContentValue<T>(dtdGuid, content, type);
        }

        protected virtual T Settings<T>(Guid dtdGuid, PositionalContentDataTypes type)
        {
            return PositionalContentHelper.ContentValue<T>(dtdGuid, settings, type);
        }
    }

}

