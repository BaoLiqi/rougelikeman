﻿namespace Org.BouncyCastle.Asn1
{
    using System;

    public class DerSetParser : Asn1SetParser, IAsn1Convertible
    {
        private readonly Asn1StreamParser _parser;

        internal DerSetParser(Asn1StreamParser parser)
        {
            this._parser = parser;
        }

        public IAsn1Convertible ReadObject() => 
            this._parser.ReadObject();

        public Asn1Object ToAsn1Object() => 
            new DerSet(this._parser.ReadVector(), false);
    }
}

