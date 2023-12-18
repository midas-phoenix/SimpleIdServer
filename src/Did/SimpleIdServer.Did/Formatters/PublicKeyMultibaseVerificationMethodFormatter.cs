﻿using SimpleIdServer.Did.Crypto;
using SimpleIdServer.Did.Crypto.Multicodec;
using SimpleIdServer.Did.Models;
using System.Linq;

namespace SimpleIdServer.Did.Builders;

/// <summary>
/// https://www.w3.org/community/reports/credentials/CG-FINAL-di-eddsa-2020-20220724/#bib-rfc8032
/// </summary>
public class PublicKeyMultibaseVerificationMethodFormatter : IVerificationMethodFormatter
{
    public const string JSON_LD_CONTEXT = "https://w3id.org/security/suites/ed25519-2020/v1";
    private readonly IMulticodecSerializer _serializer;

    public PublicKeyMultibaseVerificationMethodFormatter(IMulticodecSerializer serializer)
    {
        _serializer = serializer;
    }

    public string JSONLDContext => JSON_LD_CONTEXT;

    public string Type => "Ed25519VerificationKey2020";

    public DidDocumentVerificationMethod Format(DidDocument idDocument, ISignatureKey signatureKey)
    {
        return new DidDocumentVerificationMethod
        {
            Id = $"{idDocument.Id}#keys-{(idDocument.VerificationMethod.Where(m => m.Type == Type).Count() + 1)}",
            PublicKeyMultibase = _serializer.Serialize(signatureKey)
        };
    }

    public ISignatureKey Extract(DidDocumentVerificationMethod didDocumentVerificationMethod)
    {
        // return _serializer.GetPublicKey(didDocumentVerificationMethod.PublicKeyMultibase);
        return null;
    }
}
