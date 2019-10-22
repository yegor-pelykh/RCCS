using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace RC.Common.Certification
{
    public static class Certification
    {
        #region Public Methods

        public static X509Certificate2 CreateCertificateWithPrivateKey(string certificatePath, string keyPath)
        {
            var textCert = File.ReadAllText(certificatePath);
            var textKey = File.ReadAllText(keyPath);

            var certBuffer = GetBytesFromPEM(textCert, SectionCertificate);
            var keyBuffer = GetBytesFromPEM(textKey, SectionRSAPrivateKey);

            var provider = DecodeRSAPrivateKey(keyBuffer);

            return new X509Certificate2(certBuffer).CopyWithPrivateKey(provider);
        }

        #endregion

        #region Private Methods

        private static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            var inputBytesSize = inputBytes.Length;
            if ((alignSize == -1) || (inputBytesSize >= alignSize))
                return inputBytes;

            var buf = new byte[alignSize];
            for (var i = 0; i < inputBytesSize; ++i)
                buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
            return buf;
        }

        private static int DecodeIntegerSize(BinaryReader reader)
        {
            int count;

            var byteValue = reader.ReadByte();
            if (byteValue != 0x02)
                return 0;

            byteValue = reader.ReadByte();
            if (byteValue == 0x81)
                count = reader.ReadByte();
            else if (byteValue == 0x82)
            {
                var hi = reader.ReadByte();
                var lo = reader.ReadByte();
                count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                count = byteValue;
            }

            while (reader.ReadByte() == 0x00)
                count -= 1;
            reader.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

            return count;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privateKeyBytes)
        {
            var stream = new MemoryStream(privateKeyBytes);
            var reader = new BinaryReader(stream);

            try
            {
                var shortValue = reader.ReadUInt16();
                switch (shortValue)
                {
                    case 0x8130:
                        reader.ReadByte();
                        break;
                    case 0x8230:
                        reader.ReadInt16();
                        break;
                    default:
                        Debug.Assert(false);
                        return null;
                }

                shortValue = reader.ReadUInt16();
                if (shortValue != 0x0102)
                {
                    Debug.Assert(false);
                    return null;
                }

                var byteValue = reader.ReadByte();
                if (byteValue != 0x00)
                {
                    Debug.Assert(false);
                    return null;
                }

                var parms = new CspParameters
                {
                    Flags = CspProviderFlags.NoFlags,
                    KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant(),
                    ProviderType = ((Environment.OSVersion.Version.Major > 5) ||
                                    ((Environment.OSVersion.Version.Major == 5) &&
                                     (Environment.OSVersion.Version.Minor >= 1)))
                        ? 0x18
                        : 1
                };

                var rsa = new RSACryptoServiceProvider(parms);
                var rsaParameters = new RSAParameters
                {
                    Modulus = reader.ReadBytes(DecodeIntegerSize(reader))
                };

                var traits = new RSAParameterTraits(rsaParameters.Modulus.Length * 8);

                rsaParameters.Modulus = AlignBytes(rsaParameters.Modulus, traits.SizeMod);
                rsaParameters.Exponent = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeExp);
                rsaParameters.D = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeD);
                rsaParameters.P = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeP);
                rsaParameters.Q = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeQ);
                rsaParameters.DP = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeDP);
                rsaParameters.DQ = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeDQ);
                rsaParameters.InverseQ = AlignBytes(reader.ReadBytes(DecodeIntegerSize(reader)), traits.SizeInvQ);

                rsa.ImportParameters(rsaParameters);
                return rsa;
            }
            catch (Exception)
            {
                Debug.Assert(false);
                return null;
            }
            finally
            {
                reader.Close();
            }
        }

        private static byte[] GetBytesFromPEM(string str, string section)
        {
            var header = $"-----BEGIN {section}-----";
            var footer = $"-----END {section}-----";

            var start = str.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var end = str.IndexOf(footer, start, StringComparison.Ordinal) - start;
            return Convert.FromBase64String(str.Substring(start, end));
        }

        #endregion

        #region Constants

        private const string SectionCertificate = "CERTIFICATE";
        private const string SectionRSAPrivateKey = "RSA PRIVATE KEY";

        #endregion

    }

}
