using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace SSQE
{
    public class QueryResult : DynamicObject
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool  TryGetMember(GetMemberBinder binder, out object result)
        {
            result = properties[binder.Name];
 	        return true;
        }

        public QueryResult()
        {

        }

        public QueryResult(Dictionary<string, object> properties)
        {
            this.properties = properties;
        }
    }
}
