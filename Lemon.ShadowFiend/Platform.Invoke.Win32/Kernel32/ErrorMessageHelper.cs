namespace Platform.Invoke.Win32.Kernel32;

public static class ErrorMessageHelper
{
    public static string GetErrorMessage(int errorCode)
    {
        var FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
        var FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        var FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

        var msgSize = 255;
        var lpMsgBuf = "";
        var dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

        var lpSource = nint.Zero;
        var lpArguments = nint.Zero;
        var returnVal = Kernel32Declaration.FormatMessage(dwFlags, ref lpSource, errorCode, 0, ref lpMsgBuf, msgSize,
            ref lpArguments);

        if (returnVal == 0)
        {
            throw new Exception("Failed to format message for error code " + errorCode + ". ");
        }

        return lpMsgBuf;
    }
}