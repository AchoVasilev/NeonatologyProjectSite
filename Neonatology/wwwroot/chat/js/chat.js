$('#messagesList')[0].scrollTop = $('#messagesList')[0].scrollHeight;

$('#messageInput').on('keypress',
    function (e) {
        if (e.which === 13 && !e.shiftKey) {
            e.preventDefault();
            $('#sendButton').click();
        }
    });

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.on("ReceiveMessage",
    function (message) {
        location.reload();
    });

//$("#sendButton").click(function () {
//    var message = $.trim($('#messageInput').val());
//    $('#messageInput').val('');
//    connection.invoke("SendMessage", message, '@Model.User.Id');
//});

document.getElementById("sendButton").addEventListener("click", function (event) {

    let message = document.getElementById("messageInput").value;
    let images = document.getElementById("uploadImage").files;
    let files = document.getElementById("uploadFile").files;
    let data = new FormData();

    for (var i = 0; i < images.length; i++) {
        data.append('files', images[i]);
    }

    for (var i = 0; i < files.length; i++) {
        data.append('files', files[i]);
    }

    if (message && images.length == 0 && files.length == 0) {
        connection.invoke("SendMessage", message, '@Model.User.Id').catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById("messageInput").value = "";
    } else {
        data.append('message', message);

        if (images.length > 0 || files.length > 0) {
            if (images.length > 0) {
                document.getElementById("imageSpinner").style.display = "block";
                document.querySelector("#imageButton i").classList = "";
                document.querySelector("#imageButton i").classList.add("fas", "fa-cloud-upload-alt");
                document.getElementById("uploadImage").disabled = true;
            }

            if (files.length > 0) {
                document.getElementById("fileSpinner").style.display = "block";
                document.querySelector("#fileButton i").classList = "";
                document.querySelector("#fileButton i").classList.add("fas", "fa-file-upload");
                document.getElementById("uploadFile").disabled = true;
            }

            $.ajax({
                url: `/PrivateChat/With/${toUser}/Group/${group}/SendFiles`,
                processData: false,
                contentType: false,
                type: "POST",
                data: data,
                success: function (result) {
                    if (result.haveImages) {
                        document.getElementById("imageSpinner").style.display = "none";
                        document.querySelector("#imageButton i").classList = "";
                        document.querySelector("#imageButton i").classList.add("far", "fa-images");
                        let imageBadge = document.querySelector(".select-image-badge");
                        imageBadge.style.boxShadow = "";
                        imageBadge.style.animation = "";
                        imageBadge.textContent = "0";
                        document.getElementById("uploadImage").disabled = false;
                    }

                    if (result.haveFiles) {
                        document.getElementById("fileSpinner").style.display = "none";
                        document.querySelector("#fileButton i").classList = "";
                        document.querySelector("#fileButton i").classList.add("fas", "fa-paperclip");
                        let fileBadge = document.querySelector(".select-file-badge");
                        fileBadge.style.boxShadow = "";
                        fileBadge.style.animation = "";
                        fileBadge.textContent = "0";
                        document.getElementById("uploadFile").disabled = false;
                    }
                },
                error: function (err) {
                    console.log(err.statusText);
                }
            });

            document.getElementById("uploadImage").value = "";
            document.getElementById("uploadFile").value = "";
        }
    }

    event.preventDefault();
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});