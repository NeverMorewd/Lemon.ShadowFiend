using System;
using System.Configuration;

namespace Lemon.ShadowFiend.NativeHost;

public interface INativeHostContextProvider<out T>
{
    void SetContext(INativeHostContext<T> nativeHost);
}

public interface INativeHostContext<in T>
{
    nint Handle { get; }
}