using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.IO;

namespace NFileAPI
{
    [ScriptableType()]
    public partial class App : Application
    {

        public App()
        {   
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
            HtmlPage.RegisterScriptableObject("input", this.RootVisual);
            HtmlPage.RegisterCreateableType("FileReader", typeof(FileReader));
            HtmlPage.RegisterCreateableType("BlobBuilder", typeof(BlobBuilder));

            foreach (ScriptObject property in HtmlPage.Plugin.Children) {
                if (property.GetProperty("name") == null || property.GetProperty("value") == null
                 || property.GetProperty("name").ToString() != "onStartup"
                ) { continue; }

                string handlerFuncName = property.GetProperty("value").ToString();
                
                try
                {
                    HtmlPage.Window.Eval(String.Format("{0}();", handlerFuncName));
                }
                catch
                {
                    //don't give a damn, it's handler error, not mine
                }
            }
            
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
