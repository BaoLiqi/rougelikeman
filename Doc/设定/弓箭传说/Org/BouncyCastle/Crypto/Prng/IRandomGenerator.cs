﻿namespace Org.BouncyCastle.Crypto.Prng
{
    using System;

    public interface IRandomGenerator
    {
        void AddSeedMaterial(byte[] seed);
        void AddSeedMaterial(long seed);
        void NextBytes(byte[] bytes);
        void NextBytes(byte[] bytes, int start, int len);
    }
}

