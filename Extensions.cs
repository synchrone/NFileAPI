using System;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace NFileAPI
{
    public static class Extensions
    {
        public static void FireAsync(this EventHandler<ProgressEvent> that, object sender, ProgressEvent e){
            App.Current.RootVisual.Dispatcher.BeginInvoke(delegate(){
                if (that != null) {
                    that(sender, e);
                }
            });
        }
    }
}
