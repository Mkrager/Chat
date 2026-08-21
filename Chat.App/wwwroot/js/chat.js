const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7184/hub/chat", {
        accessTokenFactory: () => window.jwtToken
    })
    .build();

let currentGroup = null;
let currentReceiverId = null;


// =========================
// SignalR events
// =========================

connection.on("SendMessage", (msg) => {
    addMessageToChat(
        msg.senderUserName,
        msg.content,
        msg.createdDate
    );
});

connection.on("GroupJoined", (groupName) => {
    console.log("Joined group:", groupName);

    currentGroup = groupName;

    loadMessages(currentReceiverId);
});

connection.on("GroupLeft", (groupName) => {
    console.log("Left group:", groupName);
});


// =========================
// SignalR connection
// =========================

async function startConnection() {
    try {
        await connection.start();

        console.log("SignalR connected");
    }
    catch (err) {
        console.error("SignalR connection error:", err);

        // Спробувати підключитися знову через 5 секунд
        setTimeout(startConnection, 5000);
    }
}

startConnection();


// =========================
// Groups
// =========================

async function joinGroup(receiverId) {
    if (connection.state !== signalR.HubConnectionState.Connected) {
        console.log("SignalR is not connected");
        return;
    }

    try {
        await connection.invoke("JoinGroup", receiverId);
    }
    catch (err) {
        console.error("JoinGroup error:", err);
    }
}

async function leaveGroup(receiverId) {
    if (connection.state !== signalR.HubConnectionState.Connected) {
        return;
    }

    try {
        await connection.invoke("LeaveGroup", receiverId);
    }
    catch (err) {
        console.error("LeaveGroup error:", err);
    }
}


// =========================
// Send message
// =========================

async function sendMessage() {
    const input = document.getElementById("messageInput");
    const message = input.value.trim();

    if (!message || !currentReceiverId) {
        return;
    }

    if (connection.state !== signalR.HubConnectionState.Connected) {
        console.log("SignalR is not connected");
        return;
    }

    try {
        await connection.invoke(
            "SendMessageToGroup",
            currentReceiverId,
            message,
            currentReceiverId
        );

        input.value = "";
    }
    catch (err) {
        console.error("SendMessageToGroup error:", err);
    }
}


// =========================
// Messages
// =========================

function addMessageToChat(sender, content, sendDate) {
    const chatDiv = document.getElementById("chatContainer");

    const messageDiv = document.createElement("div");
    messageDiv.classList.add("message");

    messageDiv.innerHTML = `
        <h2>${sender}</h2>
        <p>${content}</p>
        <p>
            <small>
                ${new Date(sendDate).toLocaleString()}
            </small>
        </p>
    `;

    chatDiv.appendChild(messageDiv);
    chatDiv.scrollTop = chatDiv.scrollHeight;
}

async function loadMessages(receiverId) {
    try {
        const response = await fetch(
            `/Chat/GetMessages?userId=${encodeURIComponent(receiverId)}`
        );

        if (!response.ok) {
            throw new Error("Failed to load messages");
        }

        const messages = await response.json();

        const chatDiv = document.getElementById("chatContainer");
        chatDiv.innerHTML = "";

        messages.forEach(msg => {
            addMessageToChat(
                msg.senderUserName,
                msg.content,
                msg.createdDate
            );
        });
    }
    catch (err) {
        console.error("Load messages error:", err);
    }
}


// =========================
// User selection
// =========================

async function selectUser(userId) {

    if (!userId) {
        return;
    }

    // Якщо вже були в іншій групі
    if (currentReceiverId) {
        await leaveGroup(currentReceiverId);
    }

    // Запам'ятовуємо ID співрозмовника
    currentReceiverId = userId;

    console.log("Selected user:", userId);

    // Приєднуємося до групи
    await joinGroup(userId);

    // Показуємо поле вводу
    const textarea = document.querySelector(".textarea");

    if (textarea) {
        textarea.style.display = "flex";
    }
}


// =========================
// Enter -> send message
// =========================

const input = document.getElementById("messageInput");

if (input) {
    input.addEventListener("keydown", function (e) {

        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();

            sendMessage();
        }
    });
}