using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tools;
public static class ObjectFunc
{
    public static void Action<T>(this T agg, Action<T> func)
    {
        func(agg);
    }
}
