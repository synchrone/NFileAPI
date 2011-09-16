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
using System.IO;

namespace NFileAPI
{
    [ScriptableType]
    public class BlobBuilder
    {
        protected MemoryBlob data;

        public BlobBuilder() {
            this.data = new MemoryBlob();
        }

        public Blob getBlob() {
            return this.getBlob("");
        }
        public Blob getBlob(string contentType) {
            return this.data;
        }
        public void append(string text)
        {
            append(text, "transparent");
        }
        public void append(string text, string endings) { 
            if(endings != "transparent"){ throw new NotSupportedException(); }
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(text);
            this.data.GetStream().Write(byteValue, 0, byteValue.Length);
        }
        public void append(Blob data) {
            byte[] buf = new byte[1024];
            int offset = 0;
            int bytesRead = 0;
            while((bytesRead = data.GetStream().Read(buf,offset,buf.Length)) > 0){
                this.data.GetStream().Write(buf, offset, bytesRead);
                offset += bytesRead;
            }
        }

        public void append(byte[] data) {
            this.data.GetStream().Write(data, 0, data.Length);
        }
    }
}
