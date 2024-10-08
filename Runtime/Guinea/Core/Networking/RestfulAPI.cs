using System.Text;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;

namespace Guinea.Core
{
    public class RestfulAPI
    {
        public static UniTask<UnityWebRequest> Get(string url, CancellationToken cancellationToken = default, Dictionary<string, string> headers = null, int timeout = 0)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            AddHeaders(request, headers);
            Logger.Log($"RestfulAPI::Get(): {url}");
            return SendRequest(request, timeout, cancellationToken);
        }

        public static UniTask<UnityWebRequest> Post(string url, string data, CancellationToken cancellationToken = default, Dictionary<string, string> headers = null, int timeout = 0)
        {
            UnityWebRequest request = UnityWebRequest.Put(url, data);
            request.method = UnityWebRequest.kHttpVerbPOST;
            AddHeaders(request, headers);
            Logger.Log($"RestfulAPI::Post(): {url}");
            return SendRequest(request, timeout, cancellationToken);
        }

        public static UniTask<UnityWebRequest> Put(string url, string data, CancellationToken cancellationToken = default, Dictionary<string, string> headers = null, int timeout = 0)
        {
            UnityWebRequest request = UnityWebRequest.Put(url, data);
            AddHeaders(request, headers);
            Logger.Log($"RestfulAPI::Put(): {url}");
            return SendRequest(request, timeout, cancellationToken);
        }

        public static void AddHeaders(UnityWebRequest request, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }
        }
        
        public static string BuildQueryString(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            var fields = o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            var queryString = new StringBuilder("?");

            foreach (var field in fields)
            {
                var value = field.GetValue(o);
                if (value != null)
                {
                    queryString.AppendFormat("{0}={1}&", field.Name, Uri.EscapeDataString(value.ToString()));
                }
            }

            // Remove the last '&' if present
            if (queryString.Length > 1)
            {
                queryString.Length--;
            }

            return queryString.ToString();
        }

        private static UniTask<UnityWebRequest> SendRequest(UnityWebRequest request, int timeout, CancellationToken cancellationToken = default)
        {
            request.timeout = timeout;
            return request.SendWebRequest().WithCancellation(cancellationToken);
        }

    }

    [System.Serializable]
    public class Response
    {
        public readonly string data;
        public readonly string error;
        public readonly long responseCode;

        public bool HasError => error != null;

        public Response(string data, long responseCode, string error = null)
        {
            this.data = data;
            this.error = error;
            this.responseCode = responseCode;
        }
    }
}