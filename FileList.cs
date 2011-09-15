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
using System.Collections.Generic;
using System.Windows.Browser;

namespace NFileAPI
{
    [ScriptableType]
    public class FileList
    {
        List<System.IO.FileInfo> list;
        internal FileList(List<System.IO.FileInfo> list) {
            this.list = list;
        }
        public File item(int index)
        {
            return new File(this.list[index]);
        }
        public File this[int index]{
            get{
                return item(index);
            }
        }
    }
}
