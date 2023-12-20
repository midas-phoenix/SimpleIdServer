﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Sec;

namespace SimpleIdServer.Did.Crypto;

public class ES256SignatureKey : BaseESSignatureKey
{
    private ES256SignatureKey() : base(SecNamedCurves.GetByName("secp256r1")) { }

    public override string Name => Constants.SupportedSignatureKeyAlgs.ES256;

    protected override string CurveName => "P-256";

    public static ES256SignatureKey Generate()
    {
        var result = new ES256SignatureKey();
        result.Random();
        return result;
    }

    public static ES256SignatureKey From(byte[] publicKey, byte[] privateKey)
    {
        var result = new ES256SignatureKey();
        result.Import(publicKey, privateKey);
        return result;
    }

    public static ES256SignatureKey From(JsonWebKey jwk)
    {
        var result = new ES256SignatureKey();
        result.Import(jwk);
        return result;
    }
}
