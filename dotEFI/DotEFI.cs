using System;
using System.Runtime.InteropServices;
using dotEFI.Structures;

namespace dotEFI
{
    public class DotEFI
    {
        private const int BUFFER_SIZE = 1024;

        private static DotEFI instance = null;

        private DotEFI()
        {
            IntPtr tokenHandle = IntPtr.Zero;
            TokenPrivileges tokenPrivileges;

            if (!WinAPI.OpenProcessToken(WinAPI.GetCurrentProcess(), WinAPI.TOKEN_ADJUST_PRIVILEGES | WinAPI.TOKEN_QUERY, ref tokenHandle))
            {
                int errorCode = Marshal.GetLastWin32Error();

                throw new Exception($"OpenProcessToken 0x{errorCode.ToString("X8")}");
            }

            tokenPrivileges.luid = 0;

            if (!WinAPI.LookupPrivilegeValue(null, WinAPI.SE_SYSTEM_ENVIRONMENT_NAME, ref tokenPrivileges.luid))
            {
                int errorCode = Marshal.GetLastWin32Error();

                throw new Exception($"LookupPrivilegeValue 0x{errorCode.ToString("X8")}");
            }

            tokenPrivileges.privilegeCount = 1;
            tokenPrivileges.attributes = WinAPI.SE_PRIVILEGE_ENABLED;

            if (!WinAPI.AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {

                int errorCode = Marshal.GetLastWin32Error();

                throw new Exception($"AdjustTokenPrivileges 0x{errorCode.ToString("X8")}");

            }
        }

        public static DotEFI GetInstance()
        {
            if (instance == null)
                instance = new DotEFI();

            return instance;
        }



        public void SetVariable(string name, string guid, IntPtr value, uint size)
        {
            bool result = WinAPI.SetFirmwareEnvironmentVariableA(name, guid, value, size);

            if (!result)
            {
                int errorCode = Marshal.GetLastWin32Error();

                throw new Exception($"SetFirmwareEnvironmentVariableA 0x{errorCode.ToString("X8")}");
            }
        }

        public uint GetVariable(string name, string guid, IntPtr buffer, uint size)
        {
            uint result = WinAPI.GetFirmwareEnvironmentVariableA(name, guid, buffer, size);

            if (result == 0)
            {
                int errorCode = Marshal.GetLastWin32Error();

                throw new Exception($"GetFirmwareEnvironmentVariableA 0x{errorCode.ToString("X8")}");
            }
            else
            {
                return result;
            }
        }



        public short GetBootCurrent()
        {
            IntPtr pointer = Marshal.AllocHGlobal(2);
            short[] result = new short[1];

            GetVariable("BootCurrent", "{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}", pointer, 2);
            Marshal.Copy(pointer, result, 0, 1);

            return result[0];
        }

        public short[] GetBootOrder()
        {
            IntPtr pointer = Marshal.AllocHGlobal(BUFFER_SIZE);
            int length = (int) GetVariable("BootOrder", "{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}", pointer, BUFFER_SIZE);
            short[] result = new short[length / 2];

            Marshal.Copy(pointer, result, 0, length / 2);

            return result;
        }

        public EFILoadOption GetBootInformation(short number)
        {
            IntPtr pointer = Marshal.AllocHGlobal(BUFFER_SIZE);
            int length = (int)GetVariable($"Boot{number.ToString("X4")}", "{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}", pointer, BUFFER_SIZE);
            byte[] buffer = new byte[length];

            Marshal.Copy(pointer, buffer, 0, length);

            return new EFILoadOption(buffer);
        }
    }
}
