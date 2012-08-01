using System;
using System.IO;

namespace Calculator.Controls
{
    public class MyEvtArgs<T> : EventArgs
    {
        public T Value
        {
            get;
            private set;
        }
        public MyEvtArgs(T value)
        {
            this.Value = value;
        }
    }

    public class NullStream : Stream
    {

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return 0; }
        }
        

        public override long Position
        {
            get { return 0; }
            set { }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < buffer.Length; i++) buffer[i] = 0;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }
    }


    public class EventRaisingStreamWriter : StreamWriter
    {
        #region Event
        public event EventHandler<MyEvtArgs<string>> StringWritten;
        #endregion

        #region CTOR
        public EventRaisingStreamWriter(Stream s)
            : base(s)
        { }
        #endregion

        #region Private Methods
        private void LaunchEvent(string txtWritten)
        {
            if (StringWritten != null)
            {
                StringWritten(this, new MyEvtArgs<string>(txtWritten));
            }
        }
        #endregion


        #region Overrides

        public override void Write(string value)
        {
            //base.Write(value);
            LaunchEvent(value);
        }
        #endregion
    }

}
