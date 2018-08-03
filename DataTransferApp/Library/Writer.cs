namespace DataTransferApp.Library
{
    using System;

    /// <summary>
    /// The helper.
    /// </summary>
    internal class Writer
    {
        /// <summary>
        /// The write line information.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void WriteLineInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// The write line exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void WriteLineException(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// The write line warning.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void WriteLineWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
