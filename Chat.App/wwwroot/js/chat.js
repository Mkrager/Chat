const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7184/hub/chat", {
        accessTokenFactory: () => window.jwtToken
    })
    .build();

let currentGroup = null;
let currentReceiverId = null;
let currentSharedKey = null;

connection.on("SendMessage", async (msg) => {
    try {

        const decryptedMessage = await decryptMessage(
            msg.ciphertext,
            msg.iv,
            currentSharedKey
        );

        console.log("Decrypted:", decryptedMessage);

        addMessageToChat(
            msg.senderUserName,
            decryptedMessage,
            msg.createdDate
        );

    }
    catch (err) {

        console.error(
            "Message decryption error:",
            err
        );
    }
});

connection.on("GroupJoined", (groupName) => {

    console.log(
        "Joined group:",
        groupName
    );

    currentGroup = groupName;

    loadMessages(currentReceiverId);
});


connection.on("GroupLeft", (groupName) => {

    console.log(
        "Left group:",
        groupName
    );
});

async function startConnection() {

    try {

        await connection.start();

        console.log(
            "SignalR connected"
        );

    }
    catch (err) {

        console.error(
            "SignalR connection error:",
            err
        );

        setTimeout(
            startConnection,
            5000
        );
    }
}

startConnection();

async function joinGroup(receiverId) {

    if (
        connection.state !==
        signalR.HubConnectionState.Connected
    ) {

        console.log(
            "SignalR is not connected"
        );

        return;
    }

    try {

        await connection.invoke(
            "JoinGroup",
            receiverId
        );

    }
    catch (err) {

        console.error(
            "JoinGroup error:",
            err
        );
    }
}

async function leaveGroup(receiverId) {

    if (
        connection.state !==
        signalR.HubConnectionState.Connected
    ) {
        return;
    }

    try {

        await connection.invoke(
            "LeaveGroup",
            receiverId
        );

    }
    catch (err) {

        console.error(
            "LeaveGroup error:",
            err
        );
    }
}

async function sendMessage() {

    const input =
        document.getElementById(
            "messageInput"
        );

    const message =
        input.value.trim();

    if (
        !message ||
        !currentReceiverId
    ) {
        return;
    }

    if (!currentSharedKey) {

        console.error(
            "Shared key is not available."
        );

        return;
    }

    if (
        connection.state !==
        signalR.HubConnectionState.Connected
    ) {

        console.log(
            "SignalR is not connected"
        );

        return;
    }

    try {
        const encrypted =
            await encryptMessage(
                message,
                currentSharedKey
            );

        console.log(
            "Encrypted message:",
            encrypted
        );

        await connection.invoke(
            "SendMessageToGroup",
            currentReceiverId,
            encrypted.ciphertext,
            encrypted.iv
        );

        input.value = "";

    }
    catch (err) {

        console.error(
            "SendMessageToGroup error:",
            err
        );
    }
}

async function loadMessages(receiverId) {

    try {

        const response =
            await fetch(
                `/Chat/GetMessages?userId=${encodeURIComponent(receiverId)}`
            );

        if (!response.ok) {

            throw new Error(
                "Failed to load messages"
            );
        }

        const messages =
            await response.json();


        const chatDiv =
            document.getElementById(
                "chatContainer"
            );

        chatDiv.innerHTML = "";

        console.log(
            "Encrypted messages from server:",
            messages
        );

        for (const msg of messages) {

            try {

                const decryptedMessage =
                    await decryptMessage(
                        msg.ciphertext,
                        msg.iv,
                        currentSharedKey
                    );


                console.log(
                    "Decrypted message:",
                    decryptedMessage
                );


                addMessageToChat(
                    msg.senderUsername,
                    decryptedMessage,
                    msg.createdDate
                );

            }
            catch (err) {

                console.error(
                    "Failed to decrypt message:",
                    msg,
                    err
                );
            }
        }

    }
    catch (err) {

        console.error(
            "Load messages error:",
            err
        );
    }
}

async function selectUser(userId) {

    if (!userId) {
        return;
    }

    if (currentReceiverId) {

        await leaveGroup(
            currentReceiverId
        );
    }

    currentReceiverId = userId;

    console.log(
        "Selected user:",
        userId
    );

    try {

        currentSharedKey =
            await getSharedKey(
                userId
            );

        console.log(
            "Shared key for current chat:",
            currentSharedKey
        );

        await joinGroup(
            userId
        );

        const textarea =
            document.querySelector(
                ".textarea"
            );

        if (textarea) {

            textarea.style.display =
                "flex";
        }

    }
    catch (err) {

        console.error(
            "Failed to initialize E2EE chat:",
            err
        );

        currentSharedKey = null;
    }
}

const input =
    document.getElementById(
        "messageInput"
    );


if (input) {

    input.addEventListener(
        "keydown",
        function (e) {

            if (
                e.key === "Enter" &&
                !e.shiftKey
            ) {

                e.preventDefault();

                sendMessage();
            }
        }
    );
}

function addMessageToChat(
    sender,
    content,
    sendDate
) {

    const chatDiv =
        document.getElementById(
            "chatContainer"
        );


    const messageDiv =
        document.createElement(
            "div"
        );


    messageDiv.classList.add(
        "message"
    );


    messageDiv.innerHTML = `
        <h2>${sender}</h2>

        <p>${content}</p>

        <p>
            <small>
                ${new Date(sendDate).toLocaleString()}
            </small>
        </p>
    `;


    chatDiv.appendChild(
        messageDiv
    );


    chatDiv.scrollTop =
        chatDiv.scrollHeight;
}