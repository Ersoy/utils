using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;

namespace Utils.Common {

    public static class UriBuilderExtensions {

        public static void SetQueryStringParameter(this UriBuilder source, string key, object value) {
            if (String.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException("key");
            }
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            var converter = TypeDescriptor.GetConverter(value.GetType());
            var coll = source.Uri.ParseQueryString();
            coll[key] = converter.ConvertToString(value);

            const string KEY_VALUE_CONCATENATOR = "=";
            const string QUERY_CONCATENATOR = "&";

            var result = coll.AllKeys.Select(k => String.Concat(k, KEY_VALUE_CONCATENATOR, coll[k]));
            source.Query = String.Join(QUERY_CONCATENATOR, result);
        }
    }

}
