const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7184/hub/chat", {
        accessTokenFactory: () => window.jwtToken
    })
    .build();

let currentGroup = null;

connection.on("SendMessage", (msg) => {
    addMessageToChat(msg.senderUserName, msg.content, msg.createdDate);
});

connection.on("GroupJoined", groupName => {
    currentGroup = groupName;
    loadMessages(groupName);
});

connection.start()
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
        messages.forEach(msg => addMessageToChat(msg.senderUserName, msg.content, msg.createdDate));
    } catch (err) {
        console.error(err);
    }
}

function selectUser(userId) {
    if (currentGroup) leaveGroup(currentGroup);
    joinGroup(userId);

    const textarea = document.querySelector('.textarea');
    if (userId) {
        textarea.style.display = 'flex';
    } else {
        textarea.style.display = 'none';
    }
}


const input = document.getElementById("messageInput");

input.addEventListener('keydown', function (e) {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        sendMessage();
    }
});