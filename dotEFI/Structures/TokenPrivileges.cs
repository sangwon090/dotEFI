using System.Runtime.InteropServices;

namespace dotEFI.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TokenPrivileges
    {
        internal uint privilegeCount;
        internal ulong luid;
        internal uint attributes;
    }
}
