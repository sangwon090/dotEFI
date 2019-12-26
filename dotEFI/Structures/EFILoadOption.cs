using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace dotEFI.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EFILoadOption
    {
        public LoadOption attributes;
        public ushort filePathListLength;
        public string description;
        public EFIDevicePathProtocol[] filePathList;
        public byte[] optionalData;

        public EFILoadOption(byte[] data)
        {
            using(BinaryReader reader = new BinaryReader(new MemoryStream(data)))
            {
                this.attributes         = (LoadOption) reader.ReadUInt32();
                this.filePathListLength = reader.ReadUInt16();

                StringBuilder stringBuilder = new StringBuilder();
                while (true)
                {
                    ushort currentChar = reader.ReadUInt16();

                    if (currentChar == 0)
                    {
                        break;
                    }

                    stringBuilder.Append(Convert.ToChar(currentChar));
                }

                this.description = stringBuilder.ToString();

                List<EFIDevicePathProtocol> filePathList = new List<EFIDevicePathProtocol>();
                while (true)
                {
                    EFIDevicePathProtocol filePath = new EFIDevicePathProtocol();
                    filePath.type    = (DeviceType) reader.ReadByte();
                    filePath.subtype = reader.ReadByte();
                    filePath.length  = reader.ReadUInt16();
                    filePath.data    = reader.ReadBytes(filePath.length - 4);

                    filePathList.Add(filePath);
                    if(filePath.type == DeviceType.EndOfHardware && filePath.subtype == 0xFF)
                    {
                        break;
                    }
                }

                this.filePathList = filePathList.ToArray();
                this.optionalData = reader.ReadBytes((int)(data.Length - reader.BaseStream.Position));
            }
        }
    }

    public enum LoadOption : uint
    {
        Active         = 0x00000001,
        ForceReconnect = 0x00000002,
        Hidden         = 0x00000008,
        Category       = 0x00001F00,
        CategoryBoot   = 0x00000000,
        CategoryApp    = 0x00000100
    }
}
