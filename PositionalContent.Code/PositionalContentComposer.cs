using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Hifi.PositionalContent
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class PositionalContentComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<PositionalContentCacheExpire>();
            composition.Components().Append<PositionalContentServerVariableParser>();
            composition.Components().Append<PositionalContentRouting>();

            composition.Register<PositionalContentChildDataTypeService>(Lifetime.Singleton);
            composition.Register<PositionalContentPreviewController>(Lifetime.Request);
            composition.Register<PositionalContentContentService>(Lifetime.Singleton);
        }
    }
}
