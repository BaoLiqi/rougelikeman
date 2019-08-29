﻿namespace Org.BouncyCastle.Security
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Iana;
    using Org.BouncyCastle.Asn1.Pkcs;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Paddings;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public sealed class MacUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();

        static MacUtilities()
        {
            algorithms[IanaObjectIdentifiers.HmacMD5.Id] = "HMAC-MD5";
            algorithms[IanaObjectIdentifiers.HmacRipeMD160.Id] = "HMAC-RIPEMD160";
            algorithms[IanaObjectIdentifiers.HmacSha1.Id] = "HMAC-SHA1";
            algorithms[IanaObjectIdentifiers.HmacTiger.Id] = "HMAC-TIGER";
            algorithms[PkcsObjectIdentifiers.IdHmacWithSha1.Id] = "HMAC-SHA1";
            algorithms[PkcsObjectIdentifiers.IdHmacWithSha224.Id] = "HMAC-SHA224";
            algorithms[PkcsObjectIdentifiers.IdHmacWithSha256.Id] = "HMAC-SHA256";
            algorithms[PkcsObjectIdentifiers.IdHmacWithSha384.Id] = "HMAC-SHA384";
            algorithms[PkcsObjectIdentifiers.IdHmacWithSha512.Id] = "HMAC-SHA512";
            algorithms["DES"] = "DESMAC";
            algorithms["DES/CFB8"] = "DESMAC/CFB8";
            algorithms["DES64"] = "DESMAC64";
            algorithms["DESEDE"] = "DESEDEMAC";
            algorithms[PkcsObjectIdentifiers.DesEde3Cbc.Id] = "DESEDEMAC";
            algorithms["DESEDE/CFB8"] = "DESEDEMAC/CFB8";
            algorithms["DESISO9797MAC"] = "DESWITHISO9797";
            algorithms["DESEDE64"] = "DESEDEMAC64";
            algorithms["DESEDE64WITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
            algorithms["DESEDEISO9797ALG1MACWITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
            algorithms["DESEDEISO9797ALG1WITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
            algorithms["ISO9797ALG3"] = "ISO9797ALG3MAC";
            algorithms["ISO9797ALG3MACWITHISO7816-4PADDING"] = "ISO9797ALG3WITHISO7816-4PADDING";
            algorithms["SKIPJACK"] = "SKIPJACKMAC";
            algorithms["SKIPJACK/CFB8"] = "SKIPJACKMAC/CFB8";
            algorithms["IDEA"] = "IDEAMAC";
            algorithms["IDEA/CFB8"] = "IDEAMAC/CFB8";
            algorithms["RC2"] = "RC2MAC";
            algorithms["RC2/CFB8"] = "RC2MAC/CFB8";
            algorithms["RC5"] = "RC5MAC";
            algorithms["RC5/CFB8"] = "RC5MAC/CFB8";
            algorithms["GOST28147"] = "GOST28147MAC";
            algorithms["VMPC"] = "VMPCMAC";
            algorithms["VMPC-MAC"] = "VMPCMAC";
            algorithms["SIPHASH"] = "SIPHASH-2-4";
            algorithms["PBEWITHHMACSHA"] = "PBEWITHHMACSHA1";
            algorithms["1.3.14.3.2.26"] = "PBEWITHHMACSHA1";
        }

        private MacUtilities()
        {
        }

        public static byte[] CalculateMac(string algorithm, ICipherParameters cp, byte[] input)
        {
            IMac mac = GetMac(algorithm);
            mac.Init(cp);
            mac.BlockUpdate(input, 0, input.Length);
            return DoFinal(mac);
        }

        public static byte[] DoFinal(IMac mac)
        {
            byte[] output = new byte[mac.GetMacSize()];
            mac.DoFinal(output, 0);
            return output;
        }

        public static byte[] DoFinal(IMac mac, byte[] input)
        {
            mac.BlockUpdate(input, 0, input.Length);
            return DoFinal(mac);
        }

        public static string GetAlgorithmName(DerObjectIdentifier oid) => 
            ((string) algorithms[oid.Id]);

        public static IMac GetMac(DerObjectIdentifier id) => 
            GetMac(id.Id);

        public static IMac GetMac(string algorithm)
        {
            string str = Platform.ToUpperInvariant(algorithm);
            string source = (string) algorithms[str];
            if (source == null)
            {
                source = str;
            }
            if (Platform.StartsWith(source, "PBEWITH"))
            {
                source = source.Substring("PBEWITH".Length);
            }
            if (Platform.StartsWith(source, "HMAC"))
            {
                string str3;
                if (Platform.StartsWith(source, "HMAC-") || Platform.StartsWith(source, "HMAC/"))
                {
                    str3 = source.Substring(5);
                }
                else
                {
                    str3 = source.Substring(4);
                }
                return new HMac(DigestUtilities.GetDigest(str3));
            }
            switch (source)
            {
                case "AESCMAC":
                    return new CMac(new AesFastEngine());

                case "DESMAC":
                    return new CbcBlockCipherMac(new DesEngine());

                case "DESMAC/CFB8":
                    return new CfbBlockCipherMac(new DesEngine());

                case "DESMAC64":
                    return new CbcBlockCipherMac(new DesEngine(), 0x40);

                case "DESEDECMAC":
                    return new CMac(new DesEdeEngine());

                case "DESEDEMAC":
                    return new CbcBlockCipherMac(new DesEdeEngine());

                case "DESEDEMAC/CFB8":
                    return new CfbBlockCipherMac(new DesEdeEngine());

                case "DESEDEMAC64":
                    return new CbcBlockCipherMac(new DesEdeEngine(), 0x40);

                case "DESEDEMAC64WITHISO7816-4PADDING":
                    return new CbcBlockCipherMac(new DesEdeEngine(), 0x40, new ISO7816d4Padding());

                case "DESWITHISO9797":
                case "ISO9797ALG3MAC":
                    return new ISO9797Alg3Mac(new DesEngine());

                case "ISO9797ALG3WITHISO7816-4PADDING":
                    return new ISO9797Alg3Mac(new DesEngine(), new ISO7816d4Padding());

                case "SKIPJACKMAC":
                    return new CbcBlockCipherMac(new SkipjackEngine());

                case "SKIPJACKMAC/CFB8":
                    return new CfbBlockCipherMac(new SkipjackEngine());

                case "IDEAMAC":
                    return new CbcBlockCipherMac(new IdeaEngine());

                case "IDEAMAC/CFB8":
                    return new CfbBlockCipherMac(new IdeaEngine());

                case "RC2MAC":
                    return new CbcBlockCipherMac(new RC2Engine());

                case "RC2MAC/CFB8":
                    return new CfbBlockCipherMac(new RC2Engine());

                case "RC5MAC":
                    return new CbcBlockCipherMac(new RC532Engine());

                case "RC5MAC/CFB8":
                    return new CfbBlockCipherMac(new RC532Engine());

                case "GOST28147MAC":
                    return new Gost28147Mac();

                case "VMPCMAC":
                    return new VmpcMac();
            }
            if (source != "SIPHASH-2-4")
            {
                throw new SecurityUtilityException("Mac " + source + " not recognised.");
            }
            return new SipHash();
        }
    }
}

