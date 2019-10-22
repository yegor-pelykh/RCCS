using System;
using System.Diagnostics;

namespace RC.Common.Certification
{
    internal class RSAParameterTraits
    {
        internal RSAParameterTraits(int modulusLengthInBits)
        {
            var assumedLength = -1;
            var logbase = Math.Log(modulusLengthInBits, 2);
            if (logbase == (int)logbase)
                assumedLength = modulusLengthInBits;
            else
            {
                assumedLength = (int)(logbase + 1.0);
                assumedLength = (int)(Math.Pow(2, assumedLength));
                Debug.Assert(false);
            }

            switch (assumedLength)
            {
                case 1024:
                    SizeMod = 0x80;
                    SizeExp = -1;
                    SizeD = 0x80;
                    SizeP = 0x40;
                    SizeQ = 0x40;
                    SizeDP = 0x40;
                    SizeDQ = 0x40;
                    SizeInvQ = 0x40;
                    break;
                case 2048:
                    SizeMod = 0x100;
                    SizeExp = -1;
                    SizeD = 0x100;
                    SizeP = 0x80;
                    SizeQ = 0x80;
                    SizeDP = 0x80;
                    SizeDQ = 0x80;
                    SizeInvQ = 0x80;
                    break;
                case 4096:
                    SizeMod = 0x200;
                    SizeExp = -1;
                    SizeD = 0x200;
                    SizeP = 0x100;
                    SizeQ = 0x100;
                    SizeDP = 0x100;
                    SizeDQ = 0x100;
                    SizeInvQ = 0x100;
                    break;
                default:
                    Debug.Assert(false); // Unknown key size?
                    break;
            }
        }

        #region Fields

        internal int SizeMod = -1;
        internal int SizeExp = -1;
        internal int SizeD = -1;
        internal int SizeP = -1;
        internal int SizeQ = -1;
        internal int SizeDP = -1;
        internal int SizeDQ = -1;
        internal int SizeInvQ = -1;

        #endregion

    }

}
