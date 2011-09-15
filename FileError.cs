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

namespace NFileAPI
{
    public enum FileError : int
    {
        /**
         * <summary>
         * User agents MUST use this code if the File or Blob resource could not be found 
         * at the time the read was processed
         * </summary>
         **/
        NOT_FOUND_ERR = 1,	
        /**
         * <summary>
         * User agents MAY use this code if:
         *   it is determined that certain files are unsafe for access within a Web application
         *   it is determined that too many read calls are being made on File or Blob resources
         *   it is determined that the file has changed on disk since the user selected it
         *   This is a security error code to be used in situations not covered by any other error codes.
         * </summary>
         **/
        SECURITY_ERR = 2,
        /**
         * <summary>
         * User agents MUST use this code if the read operation was aborted, typically with a call to abort()
         * </summary>
         **/
        ABORT_ERR = 3,
        /**
         * <summary>
         * User agents MUST use this code if the File or Blob cannot be read, typically due due to permission problems 
         * that occur after a reference to a File or Blob has been acquired (e.g. concurrent lock with another application).
         * </summary>
         **/
        NOT_READABLE_ERR = 4,
        /**
         * <summary>
         * User agents MAY use this code if URL length limitations for Data URLs in their implementations place 
         * limits on the File or Blob data that can be represented as a Data URL [DataURL]. 
         * User agents MUST NOT use this code for the asynchronous readAsText() call and MUST NOT use this code for 
         * the synchronous readAsText() call, since encoding is determined by the encoding determination algorithm.
         * </summary>
         **/
        ENCODING_ERR = 5	
    }
}
