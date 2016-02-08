using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHttp
{
    /// <summary>
    /// The data object can be used to set content in a request body and as a dynamic return object for json object-based responses
    /// </summary>
    public class DataObject : DynamicObject
    {
        private Dictionary<string, object> _dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        #region DynamicObject overrides

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dict.Keys.ToArray();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dict.ContainsKey(binder.Name))
            {
                result = _dict[binder.Name];
                return true;
            }
            else
            {
                if (binder.ReturnType.IsValueType)
                {
                    result = Activator.CreateInstance(binder.ReturnType);
                }
                else
                {
                    result = null;
                }
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dict[binder.Name] = value;
            return true;
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
                return _dict.Remove(binder.Name);
        }

        #endregion

        #region public static methods

        public static string ToJson(DataObject obj, bool indented = false)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }

        public static string ToJson(IEnumerable<DataObject> obj, bool indented = false)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }


        #endregion

        #region public Methods

        public object Get(string memberName)
        {
            if (_dict.ContainsKey(memberName))
            {
                return _dict[memberName];
            }
            return null;
        }
        public T Get<T>(string memberName)
        {
            if (_dict.ContainsKey(memberName))
            {
                return (T)_dict[memberName];
            }
            else
            {
                return default(T);
            }
        }

        public void Set(string memberName, object value)
        {
            _dict[memberName] = value;
        }

        #endregion
    }
}
