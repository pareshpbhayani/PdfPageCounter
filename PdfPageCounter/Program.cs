namespace PdfPageCounter
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Run(new FrmHome());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            MessageBox.Show(ex.Exception.ToString(), "PDF Page Counter", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            MessageBox.Show((ex.ExceptionObject as Exception)?.ToString(), "PDF Page Counter", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}