﻿namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RijndaelEngine : IBlockCipher
    {
        private static readonly int MAXROUNDS = 14;
        private static readonly int MAXKC = 0x40;
        private static readonly byte[] Logtable = new byte[] { 
            0, 0, 0x19, 1, 50, 2, 0x1a, 0xc6, 0x4b, 0xc7, 0x1b, 0x68, 0x33, 0xee, 0xdf, 3,
            100, 4, 0xe0, 14, 0x34, 0x8d, 0x81, 0xef, 0x4c, 0x71, 8, 200, 0xf8, 0x69, 0x1c, 0xc1,
            0x7d, 0xc2, 0x1d, 0xb5, 0xf9, 0xb9, 0x27, 0x6a, 0x4d, 0xe4, 0xa6, 0x72, 0x9a, 0xc9, 9, 120,
            0x65, 0x2f, 0x8a, 5, 0x21, 15, 0xe1, 0x24, 0x12, 240, 130, 0x45, 0x35, 0x93, 0xda, 0x8e,
            150, 0x8f, 0xdb, 0xbd, 0x36, 0xd0, 0xce, 0x94, 0x13, 0x5c, 210, 0xf1, 0x40, 70, 0x83, 0x38,
            0x66, 0xdd, 0xfd, 0x30, 0xbf, 6, 0x8b, 0x62, 0xb3, 0x25, 0xe2, 0x98, 0x22, 0x88, 0x91, 0x10,
            0x7e, 110, 0x48, 0xc3, 0xa3, 0xb6, 30, 0x42, 0x3a, 0x6b, 40, 0x54, 250, 0x85, 0x3d, 0xba,
            0x2b, 0x79, 10, 0x15, 0x9b, 0x9f, 0x5e, 0xca, 0x4e, 0xd4, 0xac, 0xe5, 0xf3, 0x73, 0xa7, 0x57,
            0xaf, 0x58, 0xa8, 80, 0xf4, 0xea, 0xd6, 0x74, 0x4f, 0xae, 0xe9, 0xd5, 0xe7, 230, 0xad, 0xe8,
            0x2c, 0xd7, 0x75, 0x7a, 0xeb, 0x16, 11, 0xf5, 0x59, 0xcb, 0x5f, 0xb0, 0x9c, 0xa9, 0x51, 160,
            0x7f, 12, 0xf6, 0x6f, 0x17, 0xc4, 0x49, 0xec, 0xd8, 0x43, 0x1f, 0x2d, 0xa4, 0x76, 0x7b, 0xb7,
            0xcc, 0xbb, 0x3e, 90, 0xfb, 0x60, 0xb1, 0x86, 0x3b, 0x52, 0xa1, 0x6c, 170, 0x55, 0x29, 0x9d,
            0x97, 0xb2, 0x87, 0x90, 0x61, 190, 220, 0xfc, 0xbc, 0x95, 0xcf, 0xcd, 0x37, 0x3f, 0x5b, 0xd1,
            0x53, 0x39, 0x84, 60, 0x41, 0xa2, 0x6d, 0x47, 20, 0x2a, 0x9e, 0x5d, 0x56, 0xf2, 0xd3, 0xab,
            0x44, 0x11, 0x92, 0xd9, 0x23, 0x20, 0x2e, 0x89, 180, 0x7c, 0xb8, 0x26, 0x77, 0x99, 0xe3, 0xa5,
            0x67, 0x4a, 0xed, 0xde, 0xc5, 0x31, 0xfe, 0x18, 13, 0x63, 140, 0x80, 0xc0, 0xf7, 0x70, 7
        };
        private static readonly byte[] Alogtable = new byte[] { 
            0, 3, 5, 15, 0x11, 0x33, 0x55, 0xff, 0x1a, 0x2e, 0x72, 150, 0xa1, 0xf8, 0x13, 0x35,
            0x5f, 0xe1, 0x38, 0x48, 0xd8, 0x73, 0x95, 0xa4, 0xf7, 2, 6, 10, 30, 0x22, 0x66, 170,
            0xe5, 0x34, 0x5c, 0xe4, 0x37, 0x59, 0xeb, 0x26, 0x6a, 190, 0xd9, 0x70, 0x90, 0xab, 230, 0x31,
            0x53, 0xf5, 4, 12, 20, 60, 0x44, 0xcc, 0x4f, 0xd1, 0x68, 0xb8, 0xd3, 110, 0xb2, 0xcd,
            0x4c, 0xd4, 0x67, 0xa9, 0xe0, 0x3b, 0x4d, 0xd7, 0x62, 0xa6, 0xf1, 8, 0x18, 40, 120, 0x88,
            0x83, 0x9e, 0xb9, 0xd0, 0x6b, 0xbd, 220, 0x7f, 0x81, 0x98, 0xb3, 0xce, 0x49, 0xdb, 0x76, 0x9a,
            0xb5, 0xc4, 0x57, 0xf9, 0x10, 0x30, 80, 240, 11, 0x1d, 0x27, 0x69, 0xbb, 0xd6, 0x61, 0xa3,
            0xfe, 0x19, 0x2b, 0x7d, 0x87, 0x92, 0xad, 0xec, 0x2f, 0x71, 0x93, 0xae, 0xe9, 0x20, 0x60, 160,
            0xfb, 0x16, 0x3a, 0x4e, 210, 0x6d, 0xb7, 0xc2, 0x5d, 0xe7, 50, 0x56, 250, 0x15, 0x3f, 0x41,
            0xc3, 0x5e, 0xe2, 0x3d, 0x47, 0xc9, 0x40, 0xc0, 0x5b, 0xed, 0x2c, 0x74, 0x9c, 0xbf, 0xda, 0x75,
            0x9f, 0xba, 0xd5, 100, 0xac, 0xef, 0x2a, 0x7e, 130, 0x9d, 0xbc, 0xdf, 0x7a, 0x8e, 0x89, 0x80,
            0x9b, 0xb6, 0xc1, 0x58, 0xe8, 0x23, 0x65, 0xaf, 0xea, 0x25, 0x6f, 0xb1, 200, 0x43, 0xc5, 0x54,
            0xfc, 0x1f, 0x21, 0x63, 0xa5, 0xf4, 7, 9, 0x1b, 0x2d, 0x77, 0x99, 0xb0, 0xcb, 70, 0xca,
            0x45, 0xcf, 0x4a, 0xde, 0x79, 0x8b, 0x86, 0x91, 0xa8, 0xe3, 0x3e, 0x42, 0xc6, 0x51, 0xf3, 14,
            0x12, 0x36, 90, 0xee, 0x29, 0x7b, 0x8d, 140, 0x8f, 0x8a, 0x85, 0x94, 0xa7, 0xf2, 13, 0x17,
            0x39, 0x4b, 0xdd, 0x7c, 0x84, 0x97, 0xa2, 0xfd, 0x1c, 0x24, 0x6c, 180, 0xc7, 0x52, 0xf6, 1,
            3, 5, 15, 0x11, 0x33, 0x55, 0xff, 0x1a, 0x2e, 0x72, 150, 0xa1, 0xf8, 0x13, 0x35, 0x5f,
            0xe1, 0x38, 0x48, 0xd8, 0x73, 0x95, 0xa4, 0xf7, 2, 6, 10, 30, 0x22, 0x66, 170, 0xe5,
            0x34, 0x5c, 0xe4, 0x37, 0x59, 0xeb, 0x26, 0x6a, 190, 0xd9, 0x70, 0x90, 0xab, 230, 0x31, 0x53,
            0xf5, 4, 12, 20, 60, 0x44, 0xcc, 0x4f, 0xd1, 0x68, 0xb8, 0xd3, 110, 0xb2, 0xcd, 0x4c,
            0xd4, 0x67, 0xa9, 0xe0, 0x3b, 0x4d, 0xd7, 0x62, 0xa6, 0xf1, 8, 0x18, 40, 120, 0x88, 0x83,
            0x9e, 0xb9, 0xd0, 0x6b, 0xbd, 220, 0x7f, 0x81, 0x98, 0xb3, 0xce, 0x49, 0xdb, 0x76, 0x9a, 0xb5,
            0xc4, 0x57, 0xf9, 0x10, 0x30, 80, 240, 11, 0x1d, 0x27, 0x69, 0xbb, 0xd6, 0x61, 0xa3, 0xfe,
            0x19, 0x2b, 0x7d, 0x87, 0x92, 0xad, 0xec, 0x2f, 0x71, 0x93, 0xae, 0xe9, 0x20, 0x60, 160, 0xfb,
            0x16, 0x3a, 0x4e, 210, 0x6d, 0xb7, 0xc2, 0x5d, 0xe7, 50, 0x56, 250, 0x15, 0x3f, 0x41, 0xc3,
            0x5e, 0xe2, 0x3d, 0x47, 0xc9, 0x40, 0xc0, 0x5b, 0xed, 0x2c, 0x74, 0x9c, 0xbf, 0xda, 0x75, 0x9f,
            0xba, 0xd5, 100, 0xac, 0xef, 0x2a, 0x7e, 130, 0x9d, 0xbc, 0xdf, 0x7a, 0x8e, 0x89, 0x80, 0x9b,
            0xb6, 0xc1, 0x58, 0xe8, 0x23, 0x65, 0xaf, 0xea, 0x25, 0x6f, 0xb1, 200, 0x43, 0xc5, 0x54, 0xfc,
            0x1f, 0x21, 0x63, 0xa5, 0xf4, 7, 9, 0x1b, 0x2d, 0x77, 0x99, 0xb0, 0xcb, 70, 0xca, 0x45,
            0xcf, 0x4a, 0xde, 0x79, 0x8b, 0x86, 0x91, 0xa8, 0xe3, 0x3e, 0x42, 0xc6, 0x51, 0xf3, 14, 0x12,
            0x36, 90, 0xee, 0x29, 0x7b, 0x8d, 140, 0x8f, 0x8a, 0x85, 0x94, 0xa7, 0xf2, 13, 0x17, 0x39,
            0x4b, 0xdd, 0x7c, 0x84, 0x97, 0xa2, 0xfd, 0x1c, 0x24, 0x6c, 180, 0xc7, 0x52, 0xf6, 1, 0
        };
        private static readonly byte[] S = new byte[] { 
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 1, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
            0xca, 130, 0xc9, 0x7d, 250, 0x59, 0x47, 240, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
            4, 0xc7, 0x23, 0xc3, 0x18, 150, 5, 0x9a, 7, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
            9, 0x83, 0x2c, 0x1a, 0x1b, 110, 90, 160, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
            0x53, 0xd1, 0, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 190, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
            0xd0, 0xef, 170, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 2, 0x7f, 80, 60, 0x9f, 0xa8,
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 210,
            0xcd, 12, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 100, 0x5d, 0x19, 0x73,
            0x60, 0x81, 0x4f, 220, 0x22, 0x2a, 0x90, 0x88, 70, 0xee, 0xb8, 20, 0xde, 0x5e, 11, 0xdb,
            0xe0, 50, 0x3a, 10, 0x49, 6, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
            0xe7, 200, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 8,
            0xba, 120, 0x25, 0x2e, 0x1c, 0xa6, 180, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
            0x70, 0x3e, 0xb5, 0x66, 0x48, 3, 0xf6, 14, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 30, 0x87, 0xe9, 0xce, 0x55, 40, 0xdf,
            140, 0xa1, 0x89, 13, 0xbf, 230, 0x42, 0x68, 0x41, 0x99, 0x2d, 15, 0xb0, 0x54, 0xbb, 0x16
        };
        private static readonly byte[] Si = new byte[] { 
            0x52, 9, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 130, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 50, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 11, 0x42, 250, 0xc3, 0x4e,
            8, 0x2e, 0xa1, 0x66, 40, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 100, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 80, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 70, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0, 140, 0xbc, 0xd3, 10, 0xf7, 0xe4, 0x58, 5, 0xb8, 0xb3, 0x45, 6,
            0xd0, 0x2c, 30, 0x8f, 0xca, 0x3f, 15, 2, 0xc1, 0xaf, 0xbd, 3, 1, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 220, 0xea, 0x97, 0xf2, 0xcf, 0xce, 240, 180, 230, 0x73,
            150, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 110,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 14, 170, 0x18, 190, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 210, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 120, 0xcd, 90, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 7, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 13, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            160, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 200, 0xeb, 0xbb, 60, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 4, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 20, 0x63, 0x55, 0x21, 12, 0x7d
        };
        private static readonly byte[] rcon = new byte[] { 
            1, 2, 4, 8, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f,
            0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 250, 0xef, 0xc5, 0x91, 0, 0
        };
        private static readonly byte[][] shifts0 = new byte[][] { new byte[] { 0, 8, 0x10, 0x18 }, new byte[] { 0, 8, 0x10, 0x18 }, new byte[] { 0, 8, 0x10, 0x18 }, new byte[] { 0, 8, 0x10, 0x20 }, new byte[] { 0, 8, 0x18, 0x20 } };
        private static readonly byte[][] shifts1 = new byte[][] { new byte[] { 0, 0x18, 0x10, 8 }, new byte[] { 0, 0x20, 0x18, 0x10 }, new byte[] { 0, 40, 0x20, 0x18 }, new byte[] { 0, 0x30, 40, 0x18 }, new byte[] { 0, 0x38, 40, 0x20 } };
        private int BC;
        private long BC_MASK;
        private int ROUNDS;
        private int blockBits;
        private long[][] workingKey;
        private long A0;
        private long A1;
        private long A2;
        private long A3;
        private bool forEncryption;
        private byte[] shifts0SC;
        private byte[] shifts1SC;

        public RijndaelEngine() : this(0x80)
        {
        }

        public RijndaelEngine(int blockBits)
        {
            if (blockBits != 0x80)
            {
                if (blockBits != 160)
                {
                    if (blockBits != 0xc0)
                    {
                        if (blockBits != 0xe0)
                        {
                            if (blockBits != 0x100)
                            {
                                throw new ArgumentException("unknown blocksize to Rijndael");
                            }
                            this.BC = 0x40;
                            this.BC_MASK = -1L;
                            this.shifts0SC = shifts0[4];
                            this.shifts1SC = shifts1[4];
                        }
                        else
                        {
                            this.BC = 0x38;
                            this.BC_MASK = 0xffffffffffffffL;
                            this.shifts0SC = shifts0[3];
                            this.shifts1SC = shifts1[3];
                        }
                    }
                    else
                    {
                        this.BC = 0x30;
                        this.BC_MASK = 0xffffffffffffL;
                        this.shifts0SC = shifts0[2];
                        this.shifts1SC = shifts1[2];
                    }
                    goto Label_014D;
                }
            }
            else
            {
                this.BC = 0x20;
                this.BC_MASK = 0xffffffffL;
                this.shifts0SC = shifts0[0];
                this.shifts1SC = shifts1[0];
                goto Label_014D;
            }
            this.BC = 40;
            this.BC_MASK = 0xffffffffffL;
            this.shifts0SC = shifts0[1];
            this.shifts1SC = shifts1[1];
        Label_014D:
            this.blockBits = blockBits;
        }

        private long ApplyS(long r, byte[] box)
        {
            long num = 0L;
            for (int i = 0; i < this.BC; i += 8)
            {
                num |= (box[(int) ((r >> (i & 0x3f)) & 0xffL)] & 0xff) << i;
            }
            return num;
        }

        private void DecryptBlock(long[][] rk)
        {
            this.KeyAddition(rk[this.ROUNDS]);
            this.Substitution(Si);
            this.ShiftRow(this.shifts1SC);
            for (int i = this.ROUNDS - 1; i > 0; i--)
            {
                this.KeyAddition(rk[i]);
                this.InvMixColumn();
                this.Substitution(Si);
                this.ShiftRow(this.shifts1SC);
            }
            this.KeyAddition(rk[0]);
        }

        private void EncryptBlock(long[][] rk)
        {
            this.KeyAddition(rk[0]);
            for (int i = 1; i < this.ROUNDS; i++)
            {
                this.Substitution(S);
                this.ShiftRow(this.shifts0SC);
                this.MixColumn();
                this.KeyAddition(rk[i]);
            }
            this.Substitution(S);
            this.ShiftRow(this.shifts0SC);
            this.KeyAddition(rk[this.ROUNDS]);
        }

        private long[][] GenerateWorkingKey(byte[] key)
        {
            int num;
            int num3 = 0;
            int num4 = key.Length * 8;
            byte[,] buffer = new byte[4, MAXKC];
            long[][] numArray = new long[MAXROUNDS + 1][];
            for (int i = 0; i < (MAXROUNDS + 1); i++)
            {
                numArray[i] = new long[4];
            }
            switch (num4)
            {
                case 0x80:
                    num = 4;
                    break;

                case 160:
                    num = 5;
                    break;

                case 0xc0:
                    num = 6;
                    break;

                case 0xe0:
                    num = 7;
                    break;

                case 0x100:
                    num = 8;
                    break;

                default:
                    throw new ArgumentException("Key length not 128/160/192/224/256 bits.");
            }
            if (num4 >= this.blockBits)
            {
                this.ROUNDS = num + 6;
            }
            else
            {
                this.ROUNDS = (this.BC / 8) + 6;
            }
            int num6 = 0;
            for (int j = 0; j < key.Length; j++)
            {
                buffer[j % 4, j / 4] = key[num6++];
            }
            int num2 = 0;
            int num8 = 0;
            while ((num8 < num) && (num2 < ((this.ROUNDS + 1) * (this.BC / 8))))
            {
                for (int k = 0; k < 4; k++)
                {
                    numArray[num2 / (this.BC / 8)][k] |= (buffer[k, num8] & 0xff) << ((num2 * 8) % this.BC);
                }
                num8++;
                num2++;
            }
            while (num2 < ((this.ROUNDS + 1) * (this.BC / 8)))
            {
                for (int k = 0; k < 4; k++)
                {
                    byte num1 = buffer[k, 0];
                    num1[0] = (byte) (num1[0] ^ S[buffer[(k + 1) % 4, num - 1] & 0xff]);
                }
                byte num20 = buffer[0, 0];
                num20[0] = (byte) (num20[0] ^ rcon[num3++]);
                if (num <= 6)
                {
                    for (int m = 1; m < num; m++)
                    {
                        for (int n = 0; n < 4; n++)
                        {
                            byte num21 = buffer[n, m];
                            num21[0] = (byte) (num21[0] ^ buffer[n, m - 1]);
                        }
                    }
                }
                else
                {
                    for (int m = 1; m < 4; m++)
                    {
                        for (int num14 = 0; num14 < 4; num14++)
                        {
                            byte num22 = buffer[num14, m];
                            num22[0] = (byte) (num22[0] ^ buffer[num14, m - 1]);
                        }
                    }
                    for (int n = 0; n < 4; n++)
                    {
                        byte num23 = buffer[n, 4];
                        num23[0] = (byte) (num23[0] ^ S[buffer[n, 3] & 0xff]);
                    }
                    for (int num16 = 5; num16 < num; num16++)
                    {
                        for (int num17 = 0; num17 < 4; num17++)
                        {
                            byte num24 = buffer[num17, num16];
                            num24[0] = (byte) (num24[0] ^ buffer[num17, num16 - 1]);
                        }
                    }
                }
                int num18 = 0;
                while ((num18 < num) && (num2 < ((this.ROUNDS + 1) * (this.BC / 8))))
                {
                    for (int m = 0; m < 4; m++)
                    {
                        numArray[num2 / (this.BC / 8)][m] |= (buffer[m, num18] & 0xff) << ((num2 * 8) % this.BC);
                    }
                    num18++;
                    num2++;
                }
            }
            return numArray;
        }

        public virtual int GetBlockSize() => 
            (this.BC / 2);

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!typeof(KeyParameter).IsInstanceOfType(parameters))
            {
                throw new ArgumentException("invalid parameter passed to Rijndael init - " + Platform.GetTypeName(parameters));
            }
            this.workingKey = this.GenerateWorkingKey(((KeyParameter) parameters).GetKey());
            this.forEncryption = forEncryption;
        }

        private void InvMixColumn()
        {
            long num2;
            long num3;
            long num4;
            long num = num2 = num3 = num4 = 0L;
            for (int i = 0; i < this.BC; i += 8)
            {
                int b = (int) ((this.A0 >> i) & 0xffL);
                int num7 = (int) ((this.A1 >> i) & 0xffL);
                int num8 = (int) ((this.A2 >> i) & 0xffL);
                int num9 = (int) ((this.A3 >> i) & 0xffL);
                b = (b == 0) ? -1 : (Logtable[b & 0xff] & 0xff);
                num7 = (num7 == 0) ? -1 : (Logtable[num7 & 0xff] & 0xff);
                num8 = (num8 == 0) ? -1 : (Logtable[num8 & 0xff] & 0xff);
                num9 = (num9 == 0) ? -1 : (Logtable[num9 & 0xff] & 0xff);
                num |= ((((this.Mul0xe(b) ^ this.Mul0xb(num7)) ^ this.Mul0xd(num8)) ^ this.Mul0x9(num9)) & 0xff) << i;
                num2 |= ((((this.Mul0xe(num7) ^ this.Mul0xb(num8)) ^ this.Mul0xd(num9)) ^ this.Mul0x9(b)) & 0xff) << i;
                num3 |= ((((this.Mul0xe(num8) ^ this.Mul0xb(num9)) ^ this.Mul0xd(b)) ^ this.Mul0x9(num7)) & 0xff) << i;
                num4 |= ((((this.Mul0xe(num9) ^ this.Mul0xb(b)) ^ this.Mul0xd(num7)) ^ this.Mul0x9(num8)) & 0xff) << i;
            }
            this.A0 = num;
            this.A1 = num2;
            this.A2 = num3;
            this.A3 = num4;
        }

        private void KeyAddition(long[] rk)
        {
            this.A0 ^= rk[0];
            this.A1 ^= rk[1];
            this.A2 ^= rk[2];
            this.A3 ^= rk[3];
        }

        private void MixColumn()
        {
            long num2;
            long num3;
            long num4;
            long num = num2 = num3 = num4 = 0L;
            for (int i = 0; i < this.BC; i += 8)
            {
                int b = (int) ((this.A0 >> i) & 0xffL);
                int num7 = (int) ((this.A1 >> i) & 0xffL);
                int num8 = (int) ((this.A2 >> i) & 0xffL);
                int num9 = (int) ((this.A3 >> i) & 0xffL);
                num |= ((((this.Mul0x2(b) ^ this.Mul0x3(num7)) ^ num8) ^ num9) & 0xff) << i;
                num2 |= ((((this.Mul0x2(num7) ^ this.Mul0x3(num8)) ^ num9) ^ b) & 0xff) << i;
                num3 |= ((((this.Mul0x2(num8) ^ this.Mul0x3(num9)) ^ b) ^ num7) & 0xff) << i;
                num4 |= ((((this.Mul0x2(num9) ^ this.Mul0x3(b)) ^ num7) ^ num8) & 0xff) << i;
            }
            this.A0 = num;
            this.A1 = num2;
            this.A2 = num3;
            this.A3 = num4;
        }

        private byte Mul0x2(int b)
        {
            if (b != 0)
            {
                return Alogtable[0x19 + (Logtable[b] & 0xff)];
            }
            return 0;
        }

        private byte Mul0x3(int b)
        {
            if (b != 0)
            {
                return Alogtable[1 + (Logtable[b] & 0xff)];
            }
            return 0;
        }

        private byte Mul0x9(int b)
        {
            if (b >= 0)
            {
                return Alogtable[0xc7 + b];
            }
            return 0;
        }

        private byte Mul0xb(int b)
        {
            if (b >= 0)
            {
                return Alogtable[0x68 + b];
            }
            return 0;
        }

        private byte Mul0xd(int b)
        {
            if (b >= 0)
            {
                return Alogtable[0xee + b];
            }
            return 0;
        }

        private byte Mul0xe(int b)
        {
            if (b >= 0)
            {
                return Alogtable[0xdf + b];
            }
            return 0;
        }

        private void PackBlock(byte[] bytes, int off)
        {
            int num = off;
            for (int i = 0; i != this.BC; i += 8)
            {
                bytes[num++] = (byte) (this.A0 >> i);
                bytes[num++] = (byte) (this.A1 >> i);
                bytes[num++] = (byte) (this.A2 >> i);
                bytes[num++] = (byte) (this.A3 >> i);
            }
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("Rijndael engine not initialised");
            }
            Check.DataLength(input, inOff, this.BC / 2, "input buffer too short");
            Check.OutputLength(output, outOff, this.BC / 2, "output buffer too short");
            this.UnPackBlock(input, inOff);
            if (this.forEncryption)
            {
                this.EncryptBlock(this.workingKey);
            }
            else
            {
                this.DecryptBlock(this.workingKey);
            }
            this.PackBlock(output, outOff);
            return (this.BC / 2);
        }

        public virtual void Reset()
        {
        }

        private long Shift(long r, int shift)
        {
            ulong num = (ulong) (r >> shift);
            if (shift > 0x1f)
            {
                num &= 0xffffffffL;
            }
            return ((((long) num) | (r << (this.BC - shift))) & this.BC_MASK);
        }

        private void ShiftRow(byte[] shiftsSC)
        {
            this.A1 = this.Shift(this.A1, shiftsSC[1]);
            this.A2 = this.Shift(this.A2, shiftsSC[2]);
            this.A3 = this.Shift(this.A3, shiftsSC[3]);
        }

        private void Substitution(byte[] box)
        {
            this.A0 = this.ApplyS(this.A0, box);
            this.A1 = this.ApplyS(this.A1, box);
            this.A2 = this.ApplyS(this.A2, box);
            this.A3 = this.ApplyS(this.A3, box);
        }

        private void UnPackBlock(byte[] bytes, int off)
        {
            int num = off;
            this.A0 = bytes[num++] & 0xff;
            this.A1 = bytes[num++] & 0xff;
            this.A2 = bytes[num++] & 0xff;
            this.A3 = bytes[num++] & 0xff;
            for (int i = 8; i != this.BC; i += 8)
            {
                this.A0 |= (bytes[num++] & 0xff) << i;
                this.A1 |= (bytes[num++] & 0xff) << i;
                this.A2 |= (bytes[num++] & 0xff) << i;
                this.A3 |= (bytes[num++] & 0xff) << i;
            }
        }

        public virtual string AlgorithmName =>
            "Rijndael";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

