using System;
using System.IO;
using System.Windows.Browser;

namespace NFileAPI
{
    [ScriptableType]
    public class File : Blob
    {
        public string name { get; protected set; }
        public DateTime lastModifiedDate { get; protected set; }

        private FileInfo finfo;
        internal File(FileInfo i){
            this.finfo = i;
            this.name = i.Name;
            //this.lastModifiedDate = i.LastWriteTime; 
            //FIXME: Silverlight won't let us get that param
            this.lastModifiedDate = DateTime.Now;
        }
        internal override ConstrainedStreamWrapper GetStream()
        {
            if (this.stream == null) {
                this.stream = new ConstrainedStreamWrapper(this.finfo.OpenRead());
            }
            return this.stream;
        }
    }
}
