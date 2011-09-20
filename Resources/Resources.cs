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

namespace NFileAPI.Resources
{
    public class Resources
    {
        public Resources() { }

        private static Strings strings = new Strings();

        public Strings Strings
        { 
            get 
            {
                return Resources.strings; 
            }
        }
    }
}
