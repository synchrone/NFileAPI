using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NFileAPI
{
    
    internal class ConstrainedStreamWrapper
    {
        internal Stream underlyingStream;

        public ConstrainedStreamWrapper(Stream s) : this(s, 0, s.Length){}
        public ConstrainedStreamWrapper(Stream s, long start) : this(s, start, s.Length - start){}
        public ConstrainedStreamWrapper(Stream s, long start, long end)  {
            this.underlyingStream = s;
            startOffset = start;
            if (end > s.Length)
            {
                endOffset = 0;
            }
            else {
                endOffset = s.Length - end;
            }
        }

        private long startOffset;
        private long endOffset;
        public long Length
        {
            get
            {
                return underlyingStream.Length - (startOffset + endOffset);
            }
        }
        public void SetLength(long value)
        {
            underlyingStream.SetLength(value);
        }

        public bool CanRead { 
            get {
                return underlyingStream.CanRead;
            } 
        }

        public bool CanSeek {
            get {
                return underlyingStream.CanSeek;
            }
        }

        [ComVisible(false)]
        public virtual bool CanTimeout
        {
            get
            {
                return underlyingStream.CanTimeout;
            }
        }

        public bool CanWrite
        {
            get
            {
                return underlyingStream.CanWrite;
            }
        }

        public long Position
        {
            get
            {
                return underlyingStream.Position;
            }
            set
            {
                if (value > Length) { throw new InvalidOperationException("Can't set Position outside the stream size"); }
                underlyingStream.Position = value;
            }
        }

        [ComVisible(false)]
        public virtual int ReadTimeout
        {
            get
            {
                return underlyingStream.ReadTimeout;
            }
            set {
                underlyingStream.ReadTimeout = value;
            }
        }

        [ComVisible(false)]
        public virtual int WriteTimeout
        {
            get
            {
                return underlyingStream.WriteTimeout;
            }
            set
            {
                underlyingStream.WriteTimeout = value;
            }
        }

        public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {       
            return underlyingStream.BeginRead(buffer, offset, count, callback, state);
        }
        public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            return underlyingStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public virtual void Close() {
            underlyingStream.Close();
        }

        public void Dispose() {
            underlyingStream.Dispose();
        }
       
        public virtual int EndRead(IAsyncResult asyncResult) {
            return underlyingStream.EndRead(asyncResult);
        }

        public virtual void EndWrite(IAsyncResult asyncResult) {
            underlyingStream.EndWrite(asyncResult);
        }

        public void Flush() {
            underlyingStream.Flush();
        }

        public int Read(byte[] buffer, int offset, int count) {
            if (underlyingStream.Position + count > underlyingStream.Length) {
                count = (int)(underlyingStream.Length - underlyingStream.Position);
            }
            return underlyingStream.Read(buffer, offset, count);
        }


        public virtual int ReadByte() {
            return underlyingStream.ReadByte();
        }

        public long Seek(long offset, SeekOrigin origin) {
            return underlyingStream.Seek(offset, origin);
        }

        public void Write(byte[] buffer, int offset, int count) {
            underlyingStream.Write(buffer, offset, count);
        }

        public virtual void WriteByte(byte value) {
            underlyingStream.WriteByte(value);
        }
    }
}
