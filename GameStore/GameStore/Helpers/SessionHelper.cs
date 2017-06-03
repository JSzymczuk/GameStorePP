using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace GameStore.Helpers
{
    public static class SessionExtensionMethods
    {
        public static bool IsSet(this HttpSessionStateBase session, string key)
        {
            return session[key] == null;
        }

        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            if (session[key] == null)
                return default(T);
            else
                return (T)session[key];
        }

        public static void Set<T>(this HttpSessionStateBase session, string key, T value)
        {
            session[key] = value;
        }
    }
}