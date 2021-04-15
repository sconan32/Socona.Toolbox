using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Threading
{
    public interface IAwaitable
    {
        IAwaiter GetAwaiter();
    }
    public interface IAwaitable<out TResult>
    {
        IAwaiter<TResult> GetAwaiter();
    }
}
