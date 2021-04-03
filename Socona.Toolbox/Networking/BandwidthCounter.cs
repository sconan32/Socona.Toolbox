using System;
using System.Threading;

namespace Socona.ToolBox.Tools
{
    public class BandwidthCounter
    {
        private MiniCounter _perSecond;
        private long _totalBytes;

        public long TotalBytes => _totalBytes;

        /// <summary>
        ///     Empty constructor, because thats constructive
        /// </summary>
        public BandwidthCounter()
        {
            _perSecond = new MiniCounter { LastRead = DateTime.Now, Totalbytes = 0 };
        }

        /// <summary>
        ///     Accesses the current transfer rate, returning the text
        /// </summary>
        /// <returns></returns>
        public string GetPerSecondString()
        {
            return $"{ToUserString(GetPerSecond())}/s";
        }

        public long GetPerSecond()
        {
            var s = _perSecond.PerSecond;
            _perSecond = new MiniCounter { LastRead = DateTime.Now, Totalbytes = 0 };
            return s;
        }

        /// <summary>
        ///     Adds bytes to the total transfered
        /// </summary>
        /// <param name="count">Byte count</param>
        public void AddBytes(long count)
        {
            long currentTotal;
            long newtotal;
            do
            {
                currentTotal = TotalBytes;
                newtotal = currentTotal + count;
            } while (Interlocked.CompareExchange(ref _totalBytes, newtotal, currentTotal) != currentTotal);
            // overflow max
            _perSecond.AddBytes(count);
        }

        /// <summary>
        ///     Prints out a relevant string for the bits transfered
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToUserString(TotalBytes);
        }


        /// <summary>
        ///     Class to manage an adapters current transfer rate
        /// </summary>
        private struct MiniCounter
        {
            private long totalbytes;

            public long Totalbytes
            {
                get { return totalbytes; }
                set { totalbytes = value; }
            }

            public DateTime LastRead { get; set; }

            /// <summary>
            ///     Adds bits(total misnomer because bits per second looks a lot better than bytes per second)
            /// </summary>
            /// <param name="count">The number of bits to add</param>
            public void AddBytes(long count)
            {
                long currentTotal;
                long newtotal;
                do
                {
                    currentTotal = Totalbytes;
                    newtotal = currentTotal + count;
                } while (Interlocked.CompareExchange(ref totalbytes, newtotal, currentTotal) != currentTotal);
            }

            public long PerSecond => (long)(Totalbytes / ((DateTime.Now - LastRead).TotalSeconds +float.Epsilon));

            /// <summary>
            ///     Returns the bits per second since the last time this function was called
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return ToUserString(PerSecond);
            }
        }
        public static string ToUserString(long totalbytes)
        {
            var pbyte = totalbytes >> 50;
            var tbyte = totalbytes >> 40;
            if (pbyte > 0)
            {
                return $"{(double)tbyte / 1024:F2} PB";
            }
            var gbyte = totalbytes >> 30;
            if (tbyte > 0)
            {
                return $"{(double)gbyte / 1024:F2} TB";
            }
            var mbyte = totalbytes >> 20;
            if (gbyte > 0)
            {
                return $"{(double)mbyte / 1024:F2} GB";
            }
            var kbyte = totalbytes >> 10;
            if (mbyte > 0)
            {
                return $"{(double)kbyte / 1024:F2} MB";
            }
            if (kbyte > 0)
            {
                return $"{(double)totalbytes / 1024:F2} KB"; 
            }
            else
            {
                return $"{totalbytes} B";
            }
        }
    }
}