using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.Threading
{
    public class LazyAsync<T> : Lazy<Task<T>>
    {
        public LazyAsync(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        { }

        public LazyAsync(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())
        { }

        public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    }
}
