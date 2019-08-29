﻿namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class PrfAlgorithm
    {
        public const int tls_prf_legacy = 0;
        public const int tls_prf_sha256 = 1;
        public const int tls_prf_sha384 = 2;

        protected PrfAlgorithm()
        {
        }
    }
}

