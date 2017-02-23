/*
Open source MIT License
Copyright(c) 2017 Fred A Ekstrand Jr.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

8Y95-1833S-L669M-H0PD9-3333W-2313E-U9D7
*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Ekstrand.Net
{
    /// <summary>
    /// Allow for HTTP web request header section that are set by the system or set by properties or methods to be directly edited.
    /// </summary>
    /// <remarks>Source from: http://stackoverflow.com/questions/239725/cannot-set-some-http-headers-when-using-system-net-webrequest</remarks>
    public static class HttpWebRequestExtension
    {
        
        static string[] RestrictedHeaders = new string[]
        {
            "Accept",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "Keep-Alive",
            "Proxy-Connection",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent"
        };

        static Dictionary<string, PropertyInfo> HeaderProperties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);


        static HttpWebRequestExtension()
        {
            Type type = typeof(HttpWebRequest);
            foreach (string header in RestrictedHeaders)
            {
                string propertyName = header.Replace("-", "");
                PropertyInfo headerProperty = type.GetProperty(propertyName);
                HeaderProperties[header] = headerProperty;
            }
        }

        /// <summary>
        /// Set the HTTP web request header values including restricted access headers values.
        /// </summary>
        /// <param name="request">WebRequest</param>
        /// <param name="name">Header name item</param>
        /// <param name="value">Header item value</param>
        public static void SetRawHeader(this HttpWebRequest request, string name, string value)
        {
            if (HeaderProperties.ContainsKey(name))
            {
                PropertyInfo property = HeaderProperties[name];
                if (property.PropertyType == typeof(DateTime))
                    property.SetValue(request, DateTime.Parse(value), null);
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(request, Boolean.Parse(value), null);
                else if (property.PropertyType == typeof(long))
                    property.SetValue(request, Int64.Parse(value), null);
                else
                    property.SetValue(request, value, null);
            }
            else
            {
                request.Headers[name] = value;
            }
        }
    }
}
