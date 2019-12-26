using System.Runtime.InteropServices;

namespace dotEFI.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EFIDevicePathProtocol
    {
        public DeviceType type;
        public ushort subtype;
        public ushort length;
        public byte[] data;
    }

    public enum DeviceType : byte
    {
        Hardware      = 0x01,
        ACPI          = 0x02,
        Messaging     = 0x03,
        Media         = 0x04,
        BIOS          = 0x05,
        EndOfHardware = 0x7F
    }
}
