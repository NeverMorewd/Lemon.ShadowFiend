using Avalonia.Controls.Platform;
using Avalonia.Platform;
using Lemon.ShadowFiend.PInvoke;
using System;

namespace Lemon.ShadowFiend.NativeHost
{
    internal class Win32WindowControlHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
    {
        public Win32WindowControlHandle(IntPtr handle, string descriptor) : base(handle, descriptor)
        {

        }

        public void Destroy()
        {
            _ = Win32Api.DestroyWindow(Handle);
        }
    }
}
