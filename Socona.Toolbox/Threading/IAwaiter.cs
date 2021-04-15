using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Socona.ToolBox.Threading
{
    public interface IAwaiter : INotifyCompletion // or ICriticalNotifyCompletion
    {
        // INotifyCompletion has one method: void OnCompleted(Action continuation);

        // ICriticalNotifyCompletion implements INotifyCompletion,
        // also has this method: void UnsafeOnCompleted(Action continuation);

        bool IsCompleted { get; }

        void GetResult();
    }

    public interface ICriticalAwaiter : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    public interface IAwaiter<out TResult> : INotifyCompletion // or ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        TResult GetResult();
    }

    public interface ICriticalAwaiter<out TResult> : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        TResult GetResult();
    }

}
