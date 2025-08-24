function scrollChatToBottom() {
    const chat = document.getElementById("chatContainer");
    chat.scrollTop = chat.scrollHeight;
}

window.addEventListener("load", () => {
    scrollChatToBottom();
});

async function getMessages(userId) {
    try {
        console.log(userId);
        const response = await fetch(`/Chat/GetMessages?userId=${encodeURIComponent(userId)}`, {
            method: 'GET'
        });

        if (!response.ok) throw new Error('Failed to load messages');

        const messages = await response.json();
        const chatDiv = document.querySelector('.chat');
        chatDiv.innerHTML = '';

        messages.forEach(msg => {
            const messageDiv = document.createElement('div');
            messageDiv.classList.add('message');

            messageDiv.innerHTML = `
                <h2>${msg.senderUserName}</h2>
                <p>${msg.content}</p>
                <p><small>${new Date(msg.sendDate).toLocaleString()}</small></p>
            `;
            chatDiv.appendChild(messageDiv);
        });
        scrollChatToBottom();
    } catch (err) {
        console.error(err);
    }
}
