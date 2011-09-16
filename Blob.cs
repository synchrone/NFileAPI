using System;
using System.Windows.Browser;
using System.IO;

namespace NFileAPI
{
    [ScriptableType]
    public class Blob
    {
        public virtual ulong  size { 
            get {
                return (ulong)this.GetStream().Length;
            }
        }
        public virtual string type { get; protected set; }
        protected Blob() { }

        internal ConstrainedStreamWrapper stream;
        protected Blob(Stream stream, ulong relativeStart, ulong span, string contentType = "")
        {
            this.stream = new ConstrainedStreamWrapper(stream, (long)relativeStart, (long)span);
        }

        /** <summary>
         * Returns a new Blob object with bytes ranging from the optional start parameter 
         * upto but not including the optional end parameter, and with a type attribute that is
         * the value of the optional contentType parameter
         * </summary> **/
        public virtual Blob slice (ulong? start, ulong? end, string contentType = "")
        {
            ulong relativeStart;
            if(!start.HasValue){ //nullable primitive is null
                relativeStart = 0;
            }else if(start < 0){
                relativeStart = Math.Max(this.size + start.Value,0);
            }else{
                relativeStart = Math.Min(this.size, start.Value);
            }

            ulong relativeEnd;
            if (!end.HasValue){ //nullable primitive is null
                relativeEnd = this.size;
            }else if(end < 0){
                relativeEnd = Math.Max(this.size + end.Value, 0);
            }else{
                relativeEnd = Math.Min(end.Value, this.size);
            }

            if(contentType == null){
                contentType = "";
            }

            ulong span = Math.Max(relativeEnd - relativeStart,0);
            return new Blob(this.GetStream().underlyingStream, relativeStart, span, contentType);
        }

        internal virtual ConstrainedStreamWrapper GetStream() {
            return this.stream;
        }
    }
}
