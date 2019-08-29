﻿namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP256R1Point : AbstractFpPoint
    {
        public SecP256R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public SecP256R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
        }

        internal SecP256R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override ECPoint Add(ECPoint b)
        {
            uint[] numArray5;
            uint[] numArray6;
            uint[] numArray7;
            uint[] numArray8;
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this;
            }
            if (this == b)
            {
                return this.Twice();
            }
            ECCurve curve = this.Curve;
            SecP256R1FieldElement rawXCoord = (SecP256R1FieldElement) base.RawXCoord;
            SecP256R1FieldElement rawYCoord = (SecP256R1FieldElement) base.RawYCoord;
            SecP256R1FieldElement element3 = (SecP256R1FieldElement) b.RawXCoord;
            SecP256R1FieldElement element4 = (SecP256R1FieldElement) b.RawYCoord;
            SecP256R1FieldElement element5 = (SecP256R1FieldElement) base.RawZCoords[0];
            SecP256R1FieldElement element6 = (SecP256R1FieldElement) b.RawZCoords[0];
            uint[] zz = Nat256.CreateExt();
            uint[] numArray2 = Nat256.Create();
            uint[] numArray3 = Nat256.Create();
            uint[] x = Nat256.Create();
            bool isOne = element5.IsOne;
            if (isOne)
            {
                numArray5 = element3.x;
                numArray6 = element4.x;
            }
            else
            {
                numArray6 = numArray3;
                SecP256R1Field.Square(element5.x, numArray6);
                numArray5 = numArray2;
                SecP256R1Field.Multiply(numArray6, element3.x, numArray5);
                SecP256R1Field.Multiply(numArray6, element5.x, numArray6);
                SecP256R1Field.Multiply(numArray6, element4.x, numArray6);
            }
            bool flag2 = element6.IsOne;
            if (flag2)
            {
                numArray7 = rawXCoord.x;
                numArray8 = rawYCoord.x;
            }
            else
            {
                numArray8 = x;
                SecP256R1Field.Square(element6.x, numArray8);
                numArray7 = zz;
                SecP256R1Field.Multiply(numArray8, rawXCoord.x, numArray7);
                SecP256R1Field.Multiply(numArray8, element6.x, numArray8);
                SecP256R1Field.Multiply(numArray8, rawYCoord.x, numArray8);
            }
            uint[] z = Nat256.Create();
            SecP256R1Field.Subtract(numArray7, numArray5, z);
            uint[] numArray10 = numArray2;
            SecP256R1Field.Subtract(numArray8, numArray6, numArray10);
            if (Nat256.IsZero(z))
            {
                if (Nat256.IsZero(numArray10))
                {
                    return this.Twice();
                }
                return curve.Infinity;
            }
            uint[] numArray11 = numArray3;
            SecP256R1Field.Square(z, numArray11);
            uint[] numArray12 = Nat256.Create();
            SecP256R1Field.Multiply(numArray11, z, numArray12);
            uint[] numArray13 = numArray3;
            SecP256R1Field.Multiply(numArray11, numArray7, numArray13);
            SecP256R1Field.Negate(numArray12, numArray12);
            Nat256.Mul(numArray8, numArray12, zz);
            SecP256R1Field.Reduce32(Nat256.AddBothTo(numArray13, numArray13, numArray12), numArray12);
            SecP256R1FieldElement element7 = new SecP256R1FieldElement(x);
            SecP256R1Field.Square(numArray10, element7.x);
            SecP256R1Field.Subtract(element7.x, numArray12, element7.x);
            SecP256R1FieldElement y = new SecP256R1FieldElement(numArray12);
            SecP256R1Field.Subtract(numArray13, element7.x, y.x);
            SecP256R1Field.MultiplyAddToExt(y.x, numArray10, zz);
            SecP256R1Field.Reduce(zz, y.x);
            SecP256R1FieldElement element9 = new SecP256R1FieldElement(z);
            if (!isOne)
            {
                SecP256R1Field.Multiply(element9.x, element5.x, element9.x);
            }
            if (!flag2)
            {
                SecP256R1Field.Multiply(element9.x, element6.x, element9.x);
            }
            ECFieldElement[] zs = new ECFieldElement[] { element9 };
            return new SecP256R1Point(curve, element7, y, zs, base.IsCompressed);
        }

        protected override ECPoint Detach() => 
            new SecP256R1Point(null, this.AffineXCoord, this.AffineYCoord);

        public override ECPoint Negate()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            return new SecP256R1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
        }

        public override ECPoint ThreeTimes()
        {
            if (!base.IsInfinity && !base.RawYCoord.IsZero)
            {
                return this.Twice().Add(this);
            }
            return this;
        }

        public override ECPoint Twice()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            SecP256R1FieldElement rawYCoord = (SecP256R1FieldElement) base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return curve.Infinity;
            }
            SecP256R1FieldElement rawXCoord = (SecP256R1FieldElement) base.RawXCoord;
            SecP256R1FieldElement element3 = (SecP256R1FieldElement) base.RawZCoords[0];
            uint[] z = Nat256.Create();
            uint[] numArray2 = Nat256.Create();
            uint[] numArray3 = Nat256.Create();
            SecP256R1Field.Square(rawYCoord.x, numArray3);
            uint[] numArray4 = Nat256.Create();
            SecP256R1Field.Square(numArray3, numArray4);
            bool isOne = element3.IsOne;
            uint[] x = element3.x;
            if (!isOne)
            {
                x = numArray2;
                SecP256R1Field.Square(element3.x, x);
            }
            SecP256R1Field.Subtract(rawXCoord.x, x, z);
            uint[] numArray6 = numArray2;
            SecP256R1Field.Add(rawXCoord.x, x, numArray6);
            SecP256R1Field.Multiply(numArray6, z, numArray6);
            SecP256R1Field.Reduce32(Nat256.AddBothTo(numArray6, numArray6, numArray6), numArray6);
            uint[] numArray7 = numArray3;
            SecP256R1Field.Multiply(numArray3, rawXCoord.x, numArray7);
            SecP256R1Field.Reduce32(Nat.ShiftUpBits(8, numArray7, 2, 0), numArray7);
            SecP256R1Field.Reduce32(Nat.ShiftUpBits(8, numArray4, 3, 0, z), z);
            SecP256R1FieldElement element4 = new SecP256R1FieldElement(numArray4);
            SecP256R1Field.Square(numArray6, element4.x);
            SecP256R1Field.Subtract(element4.x, numArray7, element4.x);
            SecP256R1Field.Subtract(element4.x, numArray7, element4.x);
            SecP256R1FieldElement y = new SecP256R1FieldElement(numArray7);
            SecP256R1Field.Subtract(numArray7, element4.x, y.x);
            SecP256R1Field.Multiply(y.x, numArray6, y.x);
            SecP256R1Field.Subtract(y.x, z, y.x);
            SecP256R1FieldElement element6 = new SecP256R1FieldElement(numArray6);
            SecP256R1Field.Twice(rawYCoord.x, element6.x);
            if (!isOne)
            {
                SecP256R1Field.Multiply(element6.x, element3.x, element6.x);
            }
            return new SecP256R1Point(curve, element4, y, new ECFieldElement[] { element6 }, base.IsCompressed);
        }

        public override ECPoint TwicePlus(ECPoint b)
        {
            if (this == b)
            {
                return this.ThreeTimes();
            }
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this.Twice();
            }
            if (base.RawYCoord.IsZero)
            {
                return b;
            }
            return this.Twice().Add(b);
        }
    }
}

