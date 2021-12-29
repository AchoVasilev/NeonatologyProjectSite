$('#messagesList')[0].scrollTop = $('#messagesList')[0].scrollHeight;
$('#messageInput').on('keypress',
    function (e) {
        if (e.which === 13 && !e.shiftKey) {
            e.preventDefault();
            $('#sendButton').click();
        }
    });
 //$("#messageInput").on('change keyup copy paste cut',
 //   function () {
 //       if (!this.value) {
 //           connection.invoke("WhoIsTyping", '');
 //       } else {
 //           connection.invoke("WhoIsTyping", '@User.Identity?.Name');
 //       }
 //   });
        var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();
//connection.on("SayWhoIsTyping",
//    function(name) {
//        if (name === null || name === "") {
//            $("#whoIsTyping").html('');
//        } else {
//            var whoIsTypingInfo = `<em>&nbsp;&nbsp;${name} is typing...</em>`;
//            $("#whoIsTyping").html(whoIsTypingInfo);
//        }
//    });
connection.on("ReceiveMessage",
    function (message) {
        location.reload(); // this is the easier way because otherwise i would have to write new js methods with group logic
    });
//            function (message) {
//                var date = new Date();
//                date.setMonth(date.getMonth() + 1);
//                var timeOfDay = date.getHours() < 12 ? 'AM' : 'PM';
//                var hours = date.getHours() > 12 ? date.getHours()-12 : date.getHours();
//                var chatInfo = `
//<div class="card">
//    <h4>${message.senderUserName}</h4>
//        <div class="container ">
//            <p class="card-text">${escapeHtml(message.content)}</p>
//        <span class="time-date">${date.getDate() + '/' + (date.getMonth() < 10 ? `0${date.getMonth()}` : date.getMonth()) + '/' + date.getFullYear() + ' ' + hours + ':' + (date.getMinutes() < 10 ? `0${date.getMinutes()}` :date.getMinutes())  + timeOfDay}</span>
//        </div>
//</div>`;
//                $("#messagesList").append(chatInfo);
//                $('#messagesList')[0].scrollTop = $('#messagesList')[0].scrollHeight;
//            });
$("#sendButton").click(function () {
    var message = $.trim($('#messageInput').val());
    $('#messageInput').val('');
    connection.invoke("SendMessage", escapeHtml(message), '@Model.User.Id');
});
connection.start().catch(function (err) {
    return console.error(err.toString());
});
function escapeHtml(unsafe) {
    return unsafe
        .replace(':)', "🙂")
        .replace(':(', "🙁")
        .replace(';)', "😉")
        .replace(':*', "😘")
        .replace(/<3/g, "❤")
        .replace(/:D/g, "😀")
        .replace(/:P/g, "😜")
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}