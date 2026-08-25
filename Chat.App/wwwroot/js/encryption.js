async function importPublicKey(publicKeyBase64) {
    const binaryString = atob(publicKeyBase64);

    const bytes = new Uint8Array(binaryString.length);

    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }

    return await crypto.subtle.importKey(
        "raw",
        bytes.buffer,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        false,
        []
    );
}


async function deriveSharedKey(otherPublicKey) {

    const myPrivateKey = await getPrivateKey();

    if (!myPrivateKey) {
        throw new Error("Private key not found.");
    }

    return await crypto.subtle.deriveKey(
        {
            name: "ECDH",
            public: otherPublicKey
        },
        myPrivateKey,
        {
            name: "AES-GCM",
            length: 256
        },
        false,
        ["encrypt", "decrypt"]
    );
}


async function encryptMessage(message, sharedKey) {

    const encoder = new TextEncoder();

    const data = encoder.encode(message);

    const iv = crypto.getRandomValues(
        new Uint8Array(12)
    );

    const encryptedData = await crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        sharedKey,
        data
    );

    return {
        ciphertext: arrayBufferToBase64(encryptedData),
        iv: arrayBufferToBase64(iv)
    };
}


async function decryptMessage(ciphertextBase64, ivBase64, sharedKey) {

    const ciphertext = base64ToArrayBuffer(ciphertextBase64);
    const iv = base64ToArrayBuffer(ivBase64);

    const decryptedData = await crypto.subtle.decrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        sharedKey,
        ciphertext
    );

    const decoder = new TextDecoder();

    return decoder.decode(decryptedData);
}


function arrayBufferToBase64(buffer) {

    const bytes = new Uint8Array(buffer);

    let binary = "";

    for (let i = 0; i < bytes.length; i++) {
        binary += String.fromCharCode(bytes[i]);
    }

    return btoa(binary);
}


function base64ToArrayBuffer(base64) {

    const binaryString = atob(base64);

    const bytes = new Uint8Array(binaryString.length);

    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }

    return bytes.buffer;
}

async function getSharedKey(userId) {
    const publicKeyBase64 = await getPublicKey(userId);

    const otherPublicKey =
        await importPublicKey(publicKeyBase64);

    return await deriveSharedKey(otherPublicKey);
}