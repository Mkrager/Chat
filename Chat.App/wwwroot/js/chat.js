const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7184/hub/chat", {
        accessTokenFactory: () => window.jwtToken
    })
    .build();

let currentGroup = null;

connection.on("SendMessage", (message, senderUserName, sendDate) => {
    addMessageToChat(senderUserName, message, sendDate);
});

connection.on("GroupJoined", groupName => {
    currentGroup = groupName;
    console.log("Joined group:", groupName);
    loadMessages(groupName);
});

connection.start()
    .then(() => console.log("SignalR connected"))
    .catch(err => console.error(err));

function joinGroup(groupName) {
    connection.invoke("JoinGroup", groupName)
        .catch(err => console.error(err));
}

function leaveGroup(groupName) {
    connection.invoke("LeaveGroup", groupName)
        .catch(err => console.error(err));
}

function sendMessage() {
    const message = document.getElementById("messageInput").value;
    if (!message || !currentGroup) return;

    const receiverUserId = currentGroup;

    connection.invoke("SendMessageToGroup", currentGroup, message, receiverUserId)
        .catch(err => console.error(err));

    document.getElementById("messageInput").value = '';
}


function addMessageToChat(sender, content, sendDate) {
    const chatDiv = document.getElementById("chatContainer");
    const messageDiv = document.createElement('div');
    messageDiv.classList.add('message');
    messageDiv.innerHTML = `
            <h2>${sender}</h2>
            <p>${content}</p>
            <p><small>${new Date(sendDate).toLocaleString()}</small></p>
        `;
    chatDiv.appendChild(messageDiv);
    chatDiv.scrollTop = chatDiv.scrollHeight;
}

async function loadMessages(groupName) {
    try {
        const response = await fetch(`/Chat/GetMessages?userId=${encodeURIComponent(groupName)}`);
        if (!response.ok) throw new Error("Failed to load messages");
        const messages = await response.json();
        const chatDiv = document.getElementById("chatContainer");
        chatDiv.innerHTML = '';
        messages.forEach(msg => addMessageToChat(msg.senderUserName, msg.content, msg.sendDate));
    } catch (err) {
        console.error(err);
    }
}

function selectUser(userId) {
    if (currentGroup) leaveGroup(currentGroup);
    joinGroup(userId);
}