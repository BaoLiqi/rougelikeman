﻿namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RsaPublicKeyStructure : Asn1Encodable
    {
        private BigInteger modulus;
        private BigInteger publicExponent;

        private RsaPublicKeyStructure(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count);
            }
            this.modulus = DerInteger.GetInstance(seq[0]).PositiveValue;
            this.publicExponent = DerInteger.GetInstance(seq[1]).PositiveValue;
        }

        public RsaPublicKeyStructure(BigInteger modulus, BigInteger publicExponent)
        {
            if (modulus == null)
            {
                throw new ArgumentNullException("modulus");
            }
            if (publicExponent == null)
            {
                throw new ArgumentNullException("publicExponent");
            }
            if (modulus.SignValue <= 0)
            {
                throw new ArgumentException("Not a valid RSA modulus", "modulus");
            }
            if (publicExponent.SignValue <= 0)
            {
                throw new ArgumentException("Not a valid RSA public exponent", "publicExponent");
            }
            this.modulus = modulus;
            this.publicExponent = publicExponent;
        }

        public static RsaPublicKeyStructure GetInstance(object obj)
        {
            if ((obj == null) || (obj is RsaPublicKeyStructure))
            {
                return (RsaPublicKeyStructure) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid RsaPublicKeyStructure: " + Platform.GetTypeName(obj));
            }
            return new RsaPublicKeyStructure((Asn1Sequence) obj);
        }

        public static RsaPublicKeyStructure GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { new DerInteger(this.Modulus), new DerInteger(this.PublicExponent) });

        public BigInteger Modulus =>
            this.modulus;

        public BigInteger PublicExponent =>
            this.publicExponent;
    }
}

