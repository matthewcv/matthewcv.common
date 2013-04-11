using System;
using System.Web;

namespace matthewcv.common.Infrastructure
{
    public static class Extensions
    {
        public static bool Bool(this HttpSessionStateBase session, string key)
        {
            object val = session[key];
            if (val == null)
            {
                return false;
            }

            return Convert.ToBoolean(val);
        }

        public static bool TestAndRemove(this HttpSessionStateBase session, string key)
        {
            bool val = session.Bool(key);
            if (val)
            {
                session.Remove(key);
            }
            return val;
        }
    }
}