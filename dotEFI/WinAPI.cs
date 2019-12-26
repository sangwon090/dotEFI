using System;
using System.Runtime.InteropServices;
using dotEFI.Structures;

namespace dotEFI
{
    internal class WinAPI
    {
        internal const uint TOKEN_QUERY = 0x00000008;
        internal const uint TOKEN_ADJUST_PRIVILEGES = 0x00000020;

        internal const string SE_SYSTEM_ENVIRONMENT_NAME = "SeSystemEnvironmentPrivilege";
        internal const uint SE_PRIVILEGE_ENABLED = 0x00000002;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref ulong lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokenPrivileges NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetFirmwareEnvironmentVariableA(string lpName, string lpGuid, IntPtr pValue, uint nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint GetFirmwareEnvironmentVariableA(string lpName, string lpGuid, IntPtr pBuffer, uint nSize);
    }
}
