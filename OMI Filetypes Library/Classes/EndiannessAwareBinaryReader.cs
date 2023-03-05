﻿/* Copyright (c) 2023-present miku-666
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
**/
using System;
using System.IO;
using System.Text;

/*
 * Credits: <https://stackoverflow.com/questions/8620885/c-sharp-binary-reader-in-big-endian>
*/
namespace OMI
{
    internal class EndiannessAwareBinaryReader : BinaryReader
    {
        private readonly Endianness _endianness = Endianness.LittleEndian;
        private readonly Encoding _encoding;

        public EndiannessAwareBinaryReader(Stream input) : base(input)
        {
            _encoding = Encoding.UTF8;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
            _encoding = encoding;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            _encoding = encoding;
        }

        public EndiannessAwareBinaryReader(Stream input, Endianness endianness) : base(input)
        {
            _endianness = endianness;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, Endianness endianness) : base(input, encoding)
        {
            _endianness = endianness;
            _encoding = encoding;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness) : base(input, encoding, leaveOpen)
        {
            _endianness = endianness;
            _encoding = encoding;
        }

        public override short ReadInt16() => ReadInt16(_endianness);

        public override int ReadInt32() => ReadInt32(_endianness);

        public override long ReadInt64() => ReadInt64(_endianness);

        public override ushort ReadUInt16() => ReadUInt16(_endianness);

        public override uint ReadUInt32() => ReadUInt32(_endianness);

        public override ulong ReadUInt64() => ReadUInt64(_endianness);

        public override float ReadSingle() => ReadSingle(_endianness);

        public short ReadInt16(Endianness endianness) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), endianness), 0);

        public int ReadInt32(Endianness endianness) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), endianness), 0);

        public long ReadInt64(Endianness endianness) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), endianness), 0);

        public ushort ReadUInt16(Endianness endianness) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), endianness), 0);

        public uint ReadUInt32(Endianness endianness) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), endianness), 0);

        public ulong ReadUInt64(Endianness endianness) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), endianness), 0);

        public float ReadSingle(Endianness endianness) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), endianness), 0);

        public string ReadString(int length) => ReadString(length, _encoding);
        public string ReadString(int length, Encoding encoding)
        {
            return encoding.GetString(ReadBytes(length), 0, length).Trim('\0');
        }

        private byte[] ReadForEndianness(int bytesToRead, Endianness endianness)
        {
            var bytesRead = ReadBytes(bytesToRead);

            if ((endianness == Endianness.LittleEndian && !BitConverter.IsLittleEndian)
                || (endianness == Endianness.BigEndian && BitConverter.IsLittleEndian))
            {
                Array.Reverse(bytesRead);
            }
            return bytesRead;
        }
    }
}
