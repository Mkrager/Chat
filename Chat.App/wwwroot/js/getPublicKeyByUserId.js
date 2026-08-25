async function getPublicKey(userId) {
    const response = await fetch(
        `/user/${encodeURIComponent(userId)}/public-key`
    );

    if (!response.ok) {
        throw new Error("Failed to get public key.");
    }

    const data = await response.json();

    return data.publicKey;
}