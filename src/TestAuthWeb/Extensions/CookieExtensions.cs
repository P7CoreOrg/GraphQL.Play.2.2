using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Mvc
{
    public static class CookieExtensions
    {
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public static void SetJsonCookie<T>(this HttpResponse response,string key, T value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            response.Cookies.Append(key, JsonConvert.SerializeObject(value), option);
        }
        public static void SetJsonCookie<T>(this ControllerBase controllerBase, string key, T value, int? expireTime)
        {
            controllerBase.Response.SetJsonCookie<T>(key,value,expireTime);
        }
        public static void SetJsonCookie<T>(this PageModel pageModel, string key, T value, int? expireTime)
        {
            pageModel.Response.SetJsonCookie<T>(key, value, expireTime);
        }
        
        public static T GetJsonCookie<T>(this HttpRequest request, string key) where T : class
        {
            //read cookie from Request object  
            string cookieValueFromReq = request.Cookies[key];
            if (string.IsNullOrWhiteSpace(cookieValueFromReq))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(cookieValueFromReq);

        }
        public static T GetJsonCookie<T>(this ControllerBase controllerBase, string key) where T : class
        {
            //read cookie from Request object  
            return controllerBase.Request.GetJsonCookie<T>(key);
        }
        public static T GetJsonCookie<T>(this PageModel pageModel, string key) where T : class
        {
            //read cookie from Request object  
            return pageModel.Request.GetJsonCookie<T>(key);
        }
        

    }
}