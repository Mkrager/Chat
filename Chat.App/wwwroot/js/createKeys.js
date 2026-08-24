async function generateAndSavePublicKey() {

    const existingPrivateKey = await getPrivateKey();

    if (existingPrivateKey) {
        console.log("Key pair already exists.");

        return existingPrivateKey;
    }

    const keyPair = await crypto.subtle.generateKey(
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        false,
        ["deriveKey", "deriveBits"]
    );

    await savePrivateKey(keyPair.privateKey);

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

    console.log("Key pair successfully created.");

    return keyPair;
}

generateAndSavePublicKey();