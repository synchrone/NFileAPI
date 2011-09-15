using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Runtime.InteropServices;

namespace NFileAPI
{
    [ScriptableType]
    public class ProgressEvent : EventArgs
    {
        public bool lengthComputable;
        public long loaded{get; protected set;}
        public long total{get; protected set;}

        public ProgressEvent(long loaded, long total) {
            this.loaded = loaded;
            this.total = total;
            this.lengthComputable = true;
        }
        public ProgressEvent(long loaded) {
            this.loaded = loaded;
            this.lengthComputable = false;
        }
    }
}
