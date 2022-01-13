$(document).ready(function () {
    $('#chat-body').scroll(function () {
        const chatBody = document.getElementById('chat-body');
        const scrollBottomButton = document.getElementById('scrollBottomButton');
        let scrollHeight = chatBody.scrollHeight;
        let scrollDistanceToTop = chatBody.scrollTop;
        let chatBoyHeight = chatBody.offsetHeight;
        let distanceToBottom = scrollHeight - scrollDistanceToTop - chatBoyHeight;

        if (distanceToBottom > 400) {
            scrollBottomButton.style.visibility = 'visible';
        } else if (distanceToBottom >= 0 && distanceToBottom <= 100) {
            scrollBottomButton.style.visibility = 'hidden';
        }

        if ($(chatBody).scrollTop() == 0) {
            let messagesSkipCount = document.getElementById('messagesSkipCount').value;
            let username = document.getElementById('receiver').textContent;
            let receiverFullname = document.getElementById('receiver-fullname').textContent;
            let senderFullname = document.getElementById('sender-fullname').textContent;
            let group = document.getElementById('group-name').textContent;

            if (messagesSkipCount && username && group) {
                $.ajax({
                    type: 'GET',
                    url: `/Chat/With/${username}/Group/${group}/LoadMoreMessages/${messagesSkipCount}`,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: {
                        'username': username,
                        'group': group,
                        'messagesSkipCount': messagesSkipCount,
                        receiverFullname,
                        senderFullname
                    },
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    success: function (data) {
                        if (data.length > 0) {
                            const messagesSkipCount = document.getElementById('messagesSkipCount');
                            let oldCount = parseInt(messagesSkipCount.value);
                            messagesSkipCount.value = oldCount + data.length;
                            let oldScrollHeight = chatBody.scrollHeight;

                            for (var message of data) {
                                let newMessage = document.createElement("li");
                                newMessage.id = message.id;

                                if (message.fromUsername == message.currentUsername) {
                                    newMessage.classList.add('chat-message-right', 'pb-4');
                                    newMessage.innerHTML += `
                                        <div>
                                            <img src="~/img/NoAvatarProfileImage.png" class="rounded-circle mr-1 img-sm" alt="Avatar">
                                            <div class="text-muted small text-nowrap mt-2">${message.sendedOn}</div>
                                        </div>
                                        <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                                            <div class="font-weight-bold mb-1">${message.fromUsername}</div>
                                            ${message.content}
                                        </div>`;
                                } else {
                                    newMessage.classList.add('chat-message-left', 'pb-4');
                                    newMessage.innerHTML += `
                                           <div>
                                              <img src="~/img/NoAvatarProfileImage.png" class="rounded-circle mr-1 img-sm" alt="Avatar">
                                              <div class="text-muted small text-nowrap mt-2">${message.sendedOn}</div>
                                            </div>
                                            <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                                                <div class="font-weight-bold mb-1">${message.receiverUsername}</div>
                                                ${message.content}
                                            </div>`;
                                }

                                let firstMessage = document.getElementById('message-list').firstChild;
                                document.getElementById('message-list').insertBefore(newMessage, firstMessage);
                            }

                            let newScrollTop = chatBody.scrollHeight - oldScrollHeight;
                            chatBody.scrollTop = newScrollTop;
                        }
                    },
                    error: function (msg) {
                        console.error(msg);
                    }
                });
            }
        }
    });
});

function scrollChatToBottom() {
    $('#chat-body').animate({
        scrollTop: document.getElementById('chat-body').scrollHeight
    }, 800);
}