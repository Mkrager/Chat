async function generateAndSavePublicKey() {
    const keyPair = await crypto.subtle.generateKey(
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        ["deriveKey", "deriveBits"]
    );

    const publicKey = await crypto.subtle.exportKey(
        "raw",
        keyPair.publicKey
    );

    const publicKeyBase64 = btoa(
        String.fromCharCode(...new Uint8Array(publicKey))
    );

    const response = await fetch("/user/public-key", {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(publicKeyBase64)
    });

    if (!response.ok) {
        throw new Error("Failed to save the public key.");
    }

    console.log("Public key successfully saved.");
    
    return keyPair;
}

generateAndSavePublicKey();