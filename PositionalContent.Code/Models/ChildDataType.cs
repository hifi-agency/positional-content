using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Hifi.PositionalContent
{ 
    public class ChildDataType
    {
        public IDataType DataTypeDefinition { get; set; }
        public IDataEditor PropertyEditor { get; set; }
        public PropertyType PropertyType { get; set; }
        public IDataValueEditor ValueEditor { get; set; }
        public object PreValues { get; set; }
        public IDictionary<string, object> Config { get; set; }
    }
    
}
