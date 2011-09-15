using System;
using System.Windows.Browser;
using System.IO;

namespace NFileAPI
{
    [ScriptableType]
    public abstract class Blob
    {
        public virtual ulong  size { get; protected set; }
        public virtual string type { get; protected set; }

        protected abstract Blob _slice (ulong start, ulong end, string contentType = "");
        
        /** <summary>
         * Returns a new Blob object with bytes ranging from the optional start parameter 
         * upto but not including the optional end parameter, and with a type attribute that is
         * the value of the optional contentType parameter
         * </summary> **/
        public virtual Blob  slice (ulong? start, ulong? end, string contentType = "")
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
            return this._slice(relativeStart, span, contentType);
        }

        internal abstract ConstrainedStreamWrapper GetStream();
    }
}
