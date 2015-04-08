using Isolite.IOPackage;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace PWRTM
{
    public class Soldier
    {
        private readonly RWStream _r;
        public bool Edited = false;

        public int Type
        {
            get
            { _r.Position = 0; return _r.ReadInt32(true) - 1; }
            set { _r.Position = 0; _r.WriteInt32(value + 1, true); }
        }

        public int Sex
        {
            get
            { _r.Position = 0x1C; return _r.ReadInt32(true); }
            set { _r.Position = 0x1C; _r.WriteInt32(value, true); }
        }

        public decimal GMP
        {
            get { _r.Position = 0x20; return _r.ReadUInt32(true); }
            set { _r.Position = 0x20; _r.WriteUInt32((uint)value, true); }
        }

        //public byte Team
        //{
        //    get { return Buffer[0x18]; }
        //    set { Buffer[0x18] = value; }
        //}

        public byte Job
        {
            get { return (byte)(Buffer[0x19] - 1); }
            set { Buffer[0x19] = (byte)(value + 1); }
        }

        public string Skill1
        {
            get { return Enums.Skills.FirstOrDefault(x => x.Value == Buffer[0x80]).Key; }
            set
            {
                Enums.Skills.TryGetValue(value, out int i);
                Buffer[0x80] = (byte)i;
            }
        }

        public string Skill2
        {
            get { return Enums.Skills.FirstOrDefault(x => x.Value == Buffer[0x81]).Key; }
            set
            {
                Enums.Skills.TryGetValue(value, out int i);
                Buffer[0x81] = (byte)i;
            }
        }

        public string Skill3
        {
            get { return Enums.Skills.FirstOrDefault(x => x.Value == Buffer[0x82]).Key; }
            set
            {
                Enums.Skills.TryGetValue(value, out int i);
                Buffer[0x82] = (byte)i;
            }
        }

        public string Skill4
        {
            get { return Enums.Skills.FirstOrDefault(x => x.Value == Buffer[0x83]).Key; }
            set
            {
                Enums.Skills.TryGetValue(value, out int i);
                Buffer[0x83] = (byte)i;
            }
        }

        public decimal HP
        {
            get { _r.Position = 0x2A; return _r.ReadUInt16(true); }
            set { _r.Position = 0x2A; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal HPMax
        {
            get { _r.Position = 0x2C; return _r.ReadUInt16(true); }
            set { _r.Position = 0x2C; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal HPGrowth
        {
            get { _r.Position = 0x2E; return _r.ReadUInt16(true); }
            set { _r.Position = 0x2E; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Psyche
        {
            get { _r.Position = 0x32; return _r.ReadUInt16(true); }
            set { _r.Position = 0x32; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal PsycheMax
        {
            get { _r.Position = 0x34; return _r.ReadUInt16(true); }
            set { _r.Position = 0x34; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal PsycheGrowth
        {
            get { _r.Position = 0x36; return _r.ReadUInt16(true); }
            set { _r.Position = 0x36; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Shoot
        {
            get { _r.Position = 0x40; return _r.ReadUInt16(true); }
            set { _r.Position = 0x40; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Walk
        {
            get { _r.Position = 0x3c; return _r.ReadUInt16(true); }
            set { _r.Position = 0x3c; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Run
        {
            get { _r.Position = 0x3a; return _r.ReadUInt16(true); }
            set { _r.Position = 0x3a; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Throw
        {
            get { _r.Position = 0x44; return _r.ReadUInt16(true); }
            set { _r.Position = 0x44; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Place
        {
            get { _r.Position = 0x46; return _r.ReadUInt16(true); }
            set { _r.Position = 0x46; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Reload
        {
            get { _r.Position = 0x42; return _r.ReadUInt16(true); }
            set { _r.Position = 0x42; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Fight
        {
            get { _r.Position = 0x3e; return _r.ReadUInt16(true); }
            set { _r.Position = 0x3e; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Defense
        {
            get { _r.Position = 0x48; return _r.ReadUInt16(true); }
            set { _r.Position = 0x48; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal RnD
        {
            get { _r.Position = 0x54; return _r.ReadUInt16(true); }
            set { _r.Position = 0x54; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Mess
        {
            get { _r.Position = 0x4c; return _r.ReadUInt16(true); }
            set { _r.Position = 0x4c; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Med
        {
            get { _r.Position = 0x50; return _r.ReadUInt16(true); }
            set { _r.Position = 0x50; _r.WriteUInt16((ushort)value, true); }
        }

        public decimal Intel
        {
            get { _r.Position = 0x58; return _r.ReadUInt16(true); }
            set { _r.Position = 0x58; _r.WriteUInt16((ushort)value, true); }
        }

        public string Name
        {
            get
            { _r.Position = 8; return _r.ReadString(StringType.Ascii, 0x16); }
            set { _r.Position = 8; _r.WriteString(value, StringType.Ascii, 0x16); }
        }

        public byte[] Buffer;

        public Soldier(ref byte[] buffer)
        {
            Buffer = buffer;
            _r = new RWStream(Buffer, true) { Position = 0 };
            //Type = _r.ReadInt32()-1;
            //_r.Position += 4;
            //Name = _r.ReadString(StringType.Ascii, 0x16).Trim((char)0x0);
            //Team = (byte)_r.ReadInt8();
            //Job = (byte)_r.ReadInt8();
            //_r.Position += 2;
            //Sex = (uint)_r.ReadInt32(true);
            //GMP = _r.ReadUInt32(true);
            //_r.Position += 6;
            //HPGrowth = _r.ReadUInt16(true);
            //HP = _r.ReadUInt16(true);
            //HPMax = _r.ReadUInt16(true);
            //_r.Position += 2;
            //PsycheGrowth = _r.ReadUInt16(true);
            //Psyche = _r.ReadUInt16(true);
            //PsycheMax = _r.ReadUInt16(true);
            //_r.Position += 2;
            //Run = _r.ReadUInt16(true);
            //Walk = _r.ReadUInt16(true);
            //Fight = _r.ReadUInt16(true);
            //Shoot = _r.ReadUInt16(true);
            //Reload = _r.ReadUInt16(true);
            //Throw = _r.ReadUInt16(true);
            //Place = _r.ReadUInt16(true);
            //Defense = _r.ReadUInt16(true);
            //_r.Position += 2;
            //Mess = _r.ReadUInt32(true);
            //Med = _r.ReadUInt32(true);
            //RnD = _r.ReadUInt32(true);
            //Intel = _r.ReadUInt32(true);
            //_r.Position = 0x80;
            //Skill1 = (byte)_r.ReadInt8();
            //Skill2 = (byte)_r.ReadInt8();
            //Skill3 = (byte)_r.ReadInt8();
            //Skill4 = (byte)_r.ReadInt8();
        }
    }

    public static class Utilities
    {
        public static string GetString(int startingindex, Soldier w)
        {
            byte[] b = new byte[0x42];
            for (int index = 0; index < b.Length; index++)
            {
                b[index] = w.Buffer[startingindex++];
            }
            return Encoding.Unicode.GetString(b).TrimEnd();
        }

        public static void SetShort(short value, int startingindex, Soldier w)
        {
            var b = BitConverter.GetBytes(EndianSwap(value));
            w.Buffer[startingindex] = b[0];
            w.Buffer[startingindex + 1] = b[1];
        }

        public static short GetShort(int startingindex, Soldier w)
        {
            return (short)((w.Buffer[startingindex] << 8) | w.Buffer[startingindex + 1]);
        }

        public static uint Getuint(int index, Soldier w)
        {
            return (uint)(w.Buffer[index] << 32 | w.Buffer[index + 1] << 24 | w.Buffer[index + 2] << 16 | w.Buffer[index + 3]);
        }

        public static void Setuint(uint value, int index, Soldier w)
        {
            var b = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(value));
            w.Buffer[index] = b[0];
            w.Buffer[index + 1] = b[1];
            w.Buffer[index + 2] = b[2];
            w.Buffer[index + 3] = b[3];
        }

        public static short EndianSwap(short i)
        {
            return (short)((i << 8) + (i >> 8));
        }

        public static uint EndianSwap(uint i)
        {
            i = ((i << 8) & 0xFF00FF00) | ((i >> 8) & 0xFF00FF);
            return (i << 16) | (i >> 16);
        }
    }
}