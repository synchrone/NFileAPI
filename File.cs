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
        internal File(FileInfo i, string relativeContentType = ""){
            this.finfo = i;
            this.name = i.Name;
            //this.lastModifiedDate = i.LastWriteTime; 
            //FIXME: Silverlight won't let us get that param
            this.lastModifiedDate = DateTime.Now;

            this.size = (ulong)i.Length;
            this.type = relativeContentType;
        }

        internal ulong relativeStart = 0;
        private File(FileInfo i, ulong relativeStart, ulong span, string relativeContentType = "") : this(i, relativeContentType)
        {
            this.relativeStart = relativeStart;
            this.size = span;
        }
        protected override Blob _slice(ulong relativeStart, ulong span, string relativeContentType = "") {
            return new File(this.finfo, relativeStart, span, relativeContentType);
        }
        internal override ConstrainedStreamWrapper GetStream()
        {
            ConstrainedStreamWrapper fs = new ConstrainedStreamWrapper(this.finfo.OpenRead(), (long)relativeStart, (long)this.size);
            return fs;
        }

    }
}
