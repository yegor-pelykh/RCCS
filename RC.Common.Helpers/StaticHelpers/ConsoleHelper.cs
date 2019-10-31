using System;

namespace RC.Common.Helpers.StaticHelpers
{
    public static class ConsoleHelper
    {
        public static void PrintException(Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
            if (e.InnerException != null)
                Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
        }

    }

}
