


using System;

namespace Socona.ToolBox.Networking
{
    public class DownloadQueueItem
    {
       public DownloadQueuePriority Priority { get; set; }

        public string Domain { get; set; }

        public string RemotePath { get; set; }

        public Uri RemoteUri => new Uri($"{Domain}{RemotePath}");

        public string FullLocalPath { get; set; }

        public string FileName { get; set; }


    }

    public enum DownloadQueuePriority { Idle = 0, Low = 10_000_000, Medium = 10_000_000, High = 20_000_000, Critical=40_000_000,  };
}