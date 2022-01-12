using System.Collections.Generic;
using Topelab.Core.Resolver.DTO;

namespace Topelab.Core.Resolver.Interfaces
{
    public interface IResolver
    {
        T Get<T>();
        T Get<T, T1>(T1 arg1);
        T Get<T, T1, T2>(T1 arg1, T2 arg2);
        T Get<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
        T Get<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        T Get<T, T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        T Get<T, T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
        T Get<T>(string key);
        T Get<T, T1>(string key, T1 arg1);
        T Get<T, T1, T2>(string key, T1 arg1, T2 arg2);
        T Get<T, T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3);
        T Get<T, T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        T Get<T, T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        T Get<T, T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    }
}
