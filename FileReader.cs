using System.Windows.Browser;
using System;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace NFileAPI
{
    [ScriptableType]
    public class FileReader
    {
        public FileReader(){
            this.readyState = EMPTY;
        }
        
        internal void readAsArrayBufferSync(ScriptObject scriptBlob){
            Blob blob = scriptBlob.ConvertTo<Blob>();

            if (readyState == LOADING) {
                throw new OperationNotAllowedException();
            }
            try
            {
                ConstrainedStreamWrapper s = blob.GetStream();
                
                this.readyState = LOADING;
                if(this.onloadstart != null){
                    this.onloadstart(this,new ProgressEvent(0,s.Length));
                }

                byte[] buf = new byte[(int)s.Length];
                int bytesRead = 0;
                int offset = 0;
                s.Seek(0, SeekOrigin.Begin); //rewinding stream to read from the start
                while ((bytesRead = s.Read(buf, offset, 1024)) > 0)
                {
                    offset += bytesRead;
                    this.onprogress.FireAsync(this, new ProgressEvent(offset, s.Length));
                    //TODO: 6.4.5.2 violation! A .result should be populated with partial blob data while it's being read
                    //prolly methods, using this impl shoud subscribe to onprogress and modify result as we go
                }

                this.readyState = DONE;
                if (this.onloadend != null)
                {
                    this.onloadend.FireAsync(this, new ProgressEvent(offset, s.Length));
                }
                this.result = buf;
            }
            catch (Exception e) {
                this.readyState = DONE;
                this.result = null;
                switch (e.GetType().Name){
                    default:
                    case "IOException":
                        this.error = FileError.NOT_READABLE_ERR;
                    break;
                    case "FileNotFoundException":
                        this.error = FileError.NOT_FOUND_ERR;
                    break;
                    case "SecurityException":
                        this.error = FileError.SECURITY_ERR;
                    break;
                }
                if (this.onerror != null) {
                    this.onerror.FireAsync(this, null);
                }
                if (this.onloadend != null)
                {
                    this.onloadend.FireAsync(this, new ProgressEvent(0));
                }
                return;
            }

           
        }
        public void readAsArrayBuffer(ScriptObject scriptBlob)
        {
            App.Current.RootVisual.Dispatcher.BeginInvoke((Action<ScriptObject>)delegate(ScriptObject blob)
            {
                this.readAsArrayBufferSync(blob);

            }, new object[]{scriptBlob});
        }

        internal void readAsTextSync(ScriptObject blob, string encoding)
        {
            this.readAsArrayBufferSync(blob);
            byte[] arrayBuffer = (byte[])this.result;
            this.result = System.Text.Encoding.GetEncoding(encoding).GetString(arrayBuffer, 0, arrayBuffer.Length);
        }
        internal void readAsTextSync(ScriptObject blob) {
            readAsTextSync(blob, "UTF-8");
        }
        public void readAsText(ScriptObject scriptBlob, string encoding)
        {
            App.Current.RootVisual.Dispatcher.BeginInvoke((Action<ScriptObject, string>)delegate(ScriptObject blob, string enc)
            {
                this.readAsTextSync(blob, enc);
            }, new object[] { scriptBlob, encoding });
        }
        public void readAsText(ScriptObject scriptBlob) {
            readAsText(scriptBlob, "UTF-8");
        }
        #region NotImplemented
        public void readAsDataURL(ScriptObject blob)
        {
            throw new NotImplementedException();
        }
        public void readAsBinaryString(ScriptObject blob)
        {
            throw new NotImplementedException();
        }

        public void abort() {
            this.onabort.FireAsync(this, null);
            throw new NotImplementedException();
        }
        #endregion

        // states
        public const ushort EMPTY = 0;
        public const ushort LOADING = 1;
        public const ushort DONE = 2;
    
        public ushort readyState{get; protected set;}

        // File or Blob data
        public object result {get; protected set;}
  
        public FileError error {get; protected set;}

        #region Events
        [ScriptableMember]
        public event EventHandler<ProgressEvent> onloadstart;
        [ScriptableMember]
        public event EventHandler<ProgressEvent> onprogress;
        [ScriptableMember]
        public event EventHandler<ProgressEvent> onabort;
        [ScriptableMember]
        public event EventHandler<ProgressEvent> onerror;
        [ScriptableMember]
        public event EventHandler<ProgressEvent> onload;
        [ScriptableMember] 
        public event EventHandler<ProgressEvent> onloadend;
        #endregion
    }
}