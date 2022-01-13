'use strict';

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

const sendButton = document.getElementById('send-button');
//Disable send button until connection is established
sendButton.disabled = true;

connection.on('ReceiveMessage', function (user, message, userFullName, image = '') {
    var msg = message;
    let dateTime = new Date();
    let formattedDate = dateTime.toLocaleString('bg-BG');

    var li = document.createElement('li');

    li.classList.add('chat-message-left', 'pb-4');
    li.innerHTML = `<div>
                               <img src="~/img/NoAvatarProfileImage.png" class="rounded-circle mr-1 img-sm" alt="Avatar">
                               <div class="text-muted small text-nowrap mt-2">${formattedDate}</div>
                         </div>
                         <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                               <div class="font-weight-bold mb-1">${user}</div>
                               ${msg}
                        </div>`;

    document.getElementById('message-list').appendChild(li);

    updateScroll();
    let oldCount = parseInt(document.getElementById('messagesSkipCount').value)
    document.getElementById('messagesSkipCount').value = oldCount + 1;
});

connection.on('SendMessage', function (user, message, userFullName, image = '') {
    var msg = message;
    let dateTime = new Date()
    let formattedDate = dateTime.toLocaleString('bg-BG');

    var li = document.createElement("li");

    li.classList.add('chat-message-right', 'pb-4');
    li.innerHTML = `<div>
                               <img src="~/img/NoAvatarProfileImage.png" class="rounded-circle mr-1 img-sm" alt="Avatar">
                               <div class="text-muted small text-nowrap mt-2">${formattedDate}</div>
                        </div>
                        <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                               <div class="font-weight-bold mb-1">${user}</div>
                               ${msg}
                        </div>`;

    document.getElementById('message-list').appendChild(li);

    updateScroll();
    let oldCount = parseInt(document.getElementById('messagesSkipCount').value)
    document.getElementById('messagesSkipCount').value = oldCount + 1;
});

connection.start().then(function () {
    sendButton.disabled = false;
    var toUser = document.getElementById('receiver').textContent;
    var fromUser = document.getElementById('sender').textContent;
    var senderFullName = document.getElementById('sender-fullname').textContent;
    var group = document.getElementById('group-name').textContent;

    connection.invoke('AddToGroup', `${group}`, toUser, fromUser, senderFullName).catch(function (err) {
        return console.error(err.message.toString());
    });

    connection.invoke('UpdateMessageNotifications', toUser, fromUser).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

sendButton.addEventListener('click', function (event) {
    let toUser = document.getElementById('receiver').textContent;
    let fromUser = document.getElementById('sender').textContent;
    let senderFullName = document.getElementById('sender-fullname').textContent;
    let message = document.getElementById('chat-text').value;
    let group = document.getElementById('group-name').textContent;
    let images = document.getElementById('upload-image').files;
    let files = document.getElementById('upload-file').files;
    let data = new FormData();

    for (var i = 0; i < images.length; i++) {
        data.append('files', images[i]);
    }

    for (var i = 0; i < files.length; i++) {
        data.append('files', files[i]);
    }

    if (message && images.length == 0 && files.length == 0) {
        connection.invoke('SendMessage', fromUser, toUser, message, group, senderFullName).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke('ReceiveMessage', fromUser, message, group, senderFullName).catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById('chat-text').value = '';
    } else {
        data.append('toUsername', toUser);
        data.append('fromUsername', fromUser);
        data.append('group', group);
        data.append('message', message);

        const imageBtn = document.querySelector('#image-button i');
        const imageSpinner = document.getElementById('imageSpinner');
        const imageUpload = document.getElementById('upload-image');

        const fileBtn = document.querySelector('#file-button i');
        const fileSpinner = document.getElementById('fileSpinner');
        const fileUpload = document.getElementById('upload-file');
        if (images.length > 0 || files.length > 0) {
            if (images.length > 0) {
                imageSpinner.style.display = 'block';
                imageBtn.classList = '';
                imageBtn.classList.add('fas', 'fa-cloud-upload-alt');
                imageUpload.disabled = true;
            }

            if (files.length > 0) {
                fileSpinner.style.display = 'block';
                fileBtn.classList = '';
                fileBtn.classList.add('fas', 'fa-file-upload');
                fileUpload.disabled = true;
            }

            $.ajax({
                url: `/Chat/With/${toUser}/Group/${group}/SendFiles`,
                processData: false,
                contentType: false,
                type: 'POST',
                data: data,
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                success: function (result) {
                    if (result.haveImages) {
                        imageSpinner.style.display = 'none';
                        imageBtn.classList = '';
                        imageBtn.classList.add('fa', 'fa-camera');
                        let imageBadge = document.querySelector('span.select-image-badge');
                        imageBadge.style.boxShadow = "";
                        imageBadge.style.animation = "";
                        imageBadge.textContent = "0";
                        imageUpload.disabled = false;
                    }

                    if (result.haveFiles) {
                        fileSpinner.style.display = 'none';
                        fileBtn.classList = '';
                        fileBtn.classList.add("fa", "fa-paperclip");
                        let fileBadge = document.querySelector(".select-file-badge");
                        fileBadge.style.boxShadow = "";
                        fileBadge.style.animation = "";
                        fileBadge.textContent = "0";
                        fileUpload.disabled = false;
                    }
                },
                error: function (err) {
                    console.log(err.statusText);
                }
            });

            imageUpload.value = '';
            fileUpload.value = '';
        }
    }

    document.getElementById('chat-text').innerHTML = '';
    $('#chat-text').css('padding-left', '10px');
    event.preventDefault();
});

function updateInputScroller() {
    let scroller = document.getElementById('chat-text');
    scroller.scrollTop = scroller.scrollHeight;
}

function updateScroll() {
    if (document.getElementById('scrollBottomButton').style.visibility != "visible") {
        let element = document.getElementById('chat-body');
        element.scrollTop = element.scrollHeight;
    }
}

function zoomChatImage(imageUrl) {
    document.querySelector(".modalChatImage").style.display = "block";
    document.querySelector("#image-content").src = imageUrl;
}

function closeChatZoomedImage() {
    document.querySelector(".modalChatImage").style.display = "none";
}