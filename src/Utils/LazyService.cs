using System;
using Microsoft.Extensions.DependencyInjection;

namespace Utils
{
    public class Lazier<T> : Lazy<T> where T : class
    {

        public Lazier(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }

        public Lazier(Func<T> valueFactory) : base(valueFactory)
        {
        }
    }
}
