using KTEngine;
using System;
using System.Threading;

namespace Chip
{
    public static class RunThread
    {
        public static void Start(Action method, string name, bool highPriority = false)
        {
            var thread = new Thread(() => RunThreadWithLogging(method))
            {
                Name = name,
                IsBackground = true
            };
            if (highPriority)
                thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        private static void RunThreadWithLogging(Action method)
        {
            try
            {
                method();
            }
            catch (Exception)
            {
                //ErrorLog.Write(ex);
                //ErrorLog.Open();
#if !__IOS__ && !__TVOS__
                Engine.Instance.Exit();
#endif
            }
        }
    }
}