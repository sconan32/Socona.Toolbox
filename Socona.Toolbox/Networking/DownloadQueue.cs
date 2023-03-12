



using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socona.ToolBox.Networking
{
    public class DownloadQueue
    {
        public static DownloadQueue Shared { get; private set; } = new DownloadQueue();

        private ConcurrentDictionary<int, DownloadQueueItem> _queueDictionary = new ConcurrentDictionary<int, DownloadQueueItem>();

        private ConcurrentDictionary<string, Task> _runningTasks = new ConcurrentDictionary<string, Task>();

        private ConcurrentDictionary<DownloadQueuePriority, int> _queueTaskCount = new ConcurrentDictionary<DownloadQueuePriority, int>();


        private Thread _scheduleThread;

        private CancellationTokenSource _cts;

        public int MaxParallelThread { get; set; } = 10;

        private int _freeWorkerCount = 0;



        private DownloadQueue()
        {
            _cts = new CancellationTokenSource();
            _scheduleThread = new Thread(OnStartQueue);
            _scheduleThread.IsBackground = true;

            _freeWorkerCount = MaxParallelThread - _runningTasks.Count;
            _scheduleThread.Start();
            _queueTaskCount[DownloadQueuePriority.Idle] = 0;
            _queueTaskCount[DownloadQueuePriority.Low] = 0;
            _queueTaskCount[DownloadQueuePriority.Medium] = 0;
            _queueTaskCount[DownloadQueuePriority.High] = 0;
            _queueTaskCount[DownloadQueuePriority.Critical] = 0;
        }

        public bool TryEnqueue(DownloadQueueItem item)
        {
            int count = _queueTaskCount[item.Priority];
            int id =  count+ (int)item.Priority;
            
            return _queueTaskCount.TryUpdate(item.Priority, count + 1, count) && _queueDictionary.TryAdd(id, item);
        }


        private void OnStartQueue()
        {

            while (!_cts.IsCancellationRequested)
            {
                if (_freeWorkerCount > 0)
                {
                    int maxKey = _queueDictionary.Keys.Max();
                    if(_queueDictionary.TryRemove(maxKey, out DownloadQueueItem item))
                    {

                    }
                }
            }

        }


        public void RequestCancel()
        {
            _cts.Cancel();
        }






    }


}