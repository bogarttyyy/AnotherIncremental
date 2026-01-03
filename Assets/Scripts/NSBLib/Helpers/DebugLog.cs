using UnityEngine;

namespace NSBLib.Helpers
{
    public static class NSBLogger
    {
        public static void Log(string message)
        {
            if(Debug.isDebugBuild)
                Debug.Log(message);
        }
    }
}