const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7184/hub/chat", {
        accessTokenFactory: () => window.jwtToken
    })
    .build();


let currentGroup = null;
let currentReceiverId = null;
let currentSharedKey = null;

let currentPage = 1;
const pageSize = 50;

let isLoadingMessages = false;
let hasMoreMessages = true;

connection.on("SendMessage", async (msg) => {

    try {

        if (msg.receiverId !== currentReceiverId &&
            msg.senderId !== currentReceiverId) {
            return;
        }

        if (!currentSharedKey) {
            return;
        }

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


connection.on("GroupJoined", async (groupName) => {

    console.log(
        "Joined group:",
        groupName
    );

    currentGroup = groupName;

    currentPage = 1;
    hasMoreMessages = true;
    isLoadingMessages = false;

    await loadMessages(
        currentReceiverId,
        1
    );
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

async function loadMessages(
    receiverId,
    page = 1
) {

    if (!receiverId) {
        return;
    }


    if (isLoadingMessages) {
        return;
    }


    if (
        !hasMoreMessages &&
        page > 1
    ) {

        return;
    }


    if (!currentSharedKey) {

        console.error(
            "Shared key is not available."
        );

        return;
    }


    isLoadingMessages = true;


    const chatDiv =
        document.getElementById(
            "chatContainer"
        );


    const oldScrollHeight =
        chatDiv.scrollHeight;

    const oldScrollTop =
        chatDiv.scrollTop;
    const sharedKey = currentSharedKey;
    const selectedReceiverId = currentReceiverId;


    try {

        const response = await fetch(
            `/Chat/GetMessages?userId=${encodeURIComponent(receiverId)}&page=${page}&pageSize=${pageSize}`
        );


        if (!response.ok) {

            throw new Error(
                "Failed to load messages"
            );
        }


        const messages =
            await response.json();

        if (
            currentReceiverId !==
            selectedReceiverId
        ) {

            return;
        }


        console.log(
            `Loaded page ${page}:`,
            messages
        );


        if (
            messages.length < pageSize
        ) {

            hasMoreMessages = false;
        }

        if (page === 1) {

            chatDiv.innerHTML = "";


            for (const msg of messages) {

                await decryptAndAddMessage(
                    msg,
                    false,
                    sharedKey
                );
            }

            currentPage = 1;

            chatDiv.scrollTop =
                chatDiv.scrollHeight;
        }
        else {
            const olderMessages =
                [...messages].reverse();


            for (
                const msg of olderMessages
            ) {

                await decryptAndAddMessage(
                    msg,
                    true,
                    sharedKey
                );
            }


            currentPage = page;
            chatDiv.scrollTop =
                chatDiv.scrollHeight -
                oldScrollHeight +
                oldScrollTop;
        }

    }
    catch (err) {

        console.error(
            "Load messages error:",
            err
        );
    }
    finally {

        isLoadingMessages = false;
    }
}

async function decryptAndAddMessage(
    msg,
    prepend = false,
    sharedKey = currentSharedKey
) {

    try {

        if (!sharedKey) {

            console.error(
                "Shared key is not available."
            );

            return;
        }


        const decryptedMessage =
            await decryptMessage(
                msg.ciphertext,
                msg.iv,
                sharedKey
            );


        console.log(
            "Decrypted message:",
            decryptedMessage
        );


        addMessageToChat(
            msg.senderUsername,
            decryptedMessage,
            msg.createdDate,
            prepend
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
    currentGroup = null;
    currentPage = 1;
    hasMoreMessages = true;
    isLoadingMessages = false;
    currentSharedKey = null;


    console.log(
        "Selected user:",
        userId
    );


    try {
        const sharedKey =
            await getSharedKey(
                userId
            );

        if (
            currentReceiverId !==
            userId
        ) {

            return;
        }

        currentSharedKey =
            sharedKey;

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

function addMessageToChat(
    sender,
    content,
    sendDate,
    prepend = false
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
                ${new Date(
        sendDate
    ).toLocaleString()}
            </small>
        </p>
    `;


    if (prepend) {

        chatDiv.prepend(
            messageDiv
        );

    }
    else {

        chatDiv.appendChild(
            messageDiv
        );
    }
}

const chatContainer =
    document.getElementById(
        "chatContainer"
    );


if (chatContainer) {

    chatContainer.addEventListener(
        "scroll",
        async () => {

            if (
                chatContainer.scrollTop <= 100 &&
                !isLoadingMessages &&
                hasMoreMessages &&
                currentReceiverId
            ) {

                await loadMessages(
                    currentReceiverId,
                    currentPage + 1
                );
            }
        }
    );
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