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
        public int length { get {
            return this.list.Count;
        }}
    }
}
