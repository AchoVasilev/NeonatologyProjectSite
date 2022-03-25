var wsconn = new signalR.HubConnectionBuilder()
    .withUrl('/connectionHub')
    .configureLogging(signalR.LogLevel.None).build();

var peerConnectionConfig = { "iceServers": [{ "url": "stun:stun.l.google.com:19302" }] };

var closeBtn = document.getElementById('close');
closeBtn.style.display = 'none';
var callBtn = document.getElementById('call');
callBtn.style.display = 'block';
var group = document.getElementById('group-name').textContent;

alertify.set('notifier', 'position', 'top-center');

$(document).ready(function () {

    wsconn.start()
        .then(() => { console.log("SignalR: Connected"); setUsername(document.getElementById('sender').textContent); })
        .catch(err => console.log(err));

    // Add click handler to users in the "Users" pane
    $(document).on('click', '#call', function () {
        // Find the target user's SignalR client id
        const sender = document.getElementById('senderEmail');
        const receiver = document.getElementById('receiverEmail');

        // Make sure we are in a state where we can make a call
        if ($('#users').attr("data-mode") !== "idle") {
            alertify.error('Вече провеждате разговор. Конферентните разговори не се поддържат.');
            return;
        }

        if (sender == null || receiver == null) {
            alertify.error('Другият потребител не е на линия');
        }

        // Then make sure we aren't calling ourselves.
        if (sender.textContent != receiver.textContent) {
            // Initiate a call
            wsconn.invoke('callUser', { "connectionId": receiver.dataset.cid }, group);

            // UI in calling mode
            $('#users').attr('data-mode', 'calling');
            $("#callstatus").text('Позвъняване...');

            closeBtn.style.display = 'block';
            callBtn.style.display = 'none';
        } else {
            alertify.error("Не може да набереш себе си.");
        }
    });

    // Add handler for the hangup button
    $('#close').click(function () {

        if ($('#users').attr("data-mode") !== "idle") {
            wsconn.invoke('hangUp');
            closeAllConnections();
            $('#users').attr('data-mode', 'idle');
            $("#callstatus").text('Idle');

            closeBtn.style.display = 'none';
            callBtn.style.display = 'block';
        }
    });
});

var webrtcConstraints = { audio: true, video: false };
var streamInfo = { applicationName: WOWZA_APPLICATION_NAME, streamName: WOWZA_STREAM_NAME, sessionId: WOWZA_SESSION_ID_EMPTY };

var WOWZA_STREAM_NAME = null, connections = {}, localStream = null;

attachMediaStream = (e) => {
    var partnerAudio = document.querySelector('.audio.partner');
    if (partnerAudio.srcObject !== e.stream) {
        partnerAudio.srcObject = e.stream;
        console.log("OnPage: Attached remote stream");
    }
};

const receivedCandidateSignal = (connection, partnerClientId, candidate) => {
    connection.addIceCandidate(new RTCIceCandidate(candidate),
        () => console.log("WebRTC: added candidate successfully"),
        () => console.log("WebRTC: cannot add candidate"));
}

// Process a newly received SDP signal
const receivedSdpSignal = (connection, partnerClientId, sdp) => {
    connection.setRemoteDescription(new RTCSessionDescription(sdp), () => {
        if (connection.remoteDescription.type == "offer") {
            connection.addStream(localStream);
            connection.createAnswer().then((desc) => {
                connection.setLocalDescription(desc, () => {
                    sendHubSignal(JSON.stringify({ "sdp": connection.localDescription }), partnerClientId);
                }, errorHandler);
            }, errorHandler);
        } else if (connection.remoteDescription.type == "answer") {
            console.log('WebRTC: remote Description type answer');
        }
    }, errorHandler);
}

// Hand off a new signal from the signaler to the connection
const newSignal = (partnerClientId, data) => {
    var signal = JSON.parse(data);
    var connection = getConnection(partnerClientId);

    if (signal.sdp) {
        receivedSdpSignal(connection, partnerClientId, signal.sdp);
    } else if (signal.candidate) {
        receivedCandidateSignal(connection, partnerClientId, signal.candidate);
    } else {
        connection.addIceCandidate(null,
            () => console.log("WebRTC: added null candidate successfully"),
            () => console.log("WebRTC: cannot add null candidate"));
    }
}

const onReadyForStream = (connection) => {
    connection.addStream(localStream);
}

const onStreamRemoved = (connection, streamId) => {
    console.log("WebRTC: onStreamRemoved -> Removing stream: ");
}
// Close the connection between myself and the given partner
const closeConnection = (partnerClientId) => {
    var connection = connections[partnerClientId];

    if (connection) {
        onStreamRemoved(null, null);

        // Close the connection
        connection.close();
        delete connections[partnerClientId]; // Remove the property
    }
}
// Close all of our connections
const closeAllConnections = () => {
    for (var connectionId in connections) {
        closeConnection(connectionId);
    }
}

const getConnection = (partnerClientId) => {
    if (connections[partnerClientId]) {
        return connections[partnerClientId];
    }
    else {
        console.log("WebRTC: initialize new connection");
        return initializeConnection(partnerClientId)
    }
}

const initiateOffer = (partnerClientId, stream) => {
    var connection = getConnection(partnerClientId);
    connection.addStream(stream);

    connection.createOffer().then(offer => {
        connection.setLocalDescription(offer).then(() => {
            setTimeout(() => {
                sendHubSignal(JSON.stringify({ "sdp": connection.localDescription }), partnerClientId);
            }, 1000);
        }).catch(err => console.error('WebRTC: Error while setting local description', err));
    }).catch(err => console.error('WebRTC: Error while creating offer', err));
}

const callbackUserMediaSuccess = (stream) => {
    localStream = stream;

    const audioTracks = localStream.getAudioTracks();
    if (audioTracks.length > 0) {
        console.log(`Using Audio device: ${audioTracks[0].label}`);
    }
};

const initializeUserMedia = () => {
    navigator.getUserMedia(webrtcConstraints, callbackUserMediaSuccess, errorHandler);
};
// stream removed
const callbackRemoveStream = (connection, evt) => {
    // Clear out the partner window
    var otherAudio = document.querySelector('.audio.partner');
    otherAudio.src = '';
}

const callbackAddStream = (connection, evt) => {
      attachMediaStream(evt);
}

const callbackNegotiationNeeded = (connection, evt) => {
    console.log("WebRTC: Negotiation needed...");
    //console.log("Event: ", evt);
}

const callbackIceCandidate = (evt, connection, partnerClientId) => {   
    if (evt.candidate) {
        sendHubSignal(JSON.stringify({ "candidate": evt.candidate }), partnerClientId);
    } else {
        // Null candidate means we are done collecting candidates.
        console.log('WebRTC: ICE candidate gathering complete');
        sendHubSignal(JSON.stringify({ "candidate": null }), partnerClientId);
    }
}

const initializeConnection = (partnerClientId) => {
    var connection = new RTCPeerConnection(peerConnectionConfig);

    connection.onicecandidate = evt => callbackIceCandidate(evt, connection, partnerClientId); // ICE Candidate Callback
    //connection.onnegotiationneeded = evt => callbackNegotiationNeeded(connection, evt); // Negotiation Needed Callback
    connection.onaddstream = evt => callbackAddStream(connection, evt); // Add stream handler callback
    connection.onremovestream = evt => callbackRemoveStream(connection, evt); // Remove stream handler callback

    connections[partnerClientId] = connection; // Store away the connection based on username

    return connection;
}

sendHubSignal = (candidate, partnerClientId) => {
    wsconn.invoke('sendSignal', candidate, partnerClientId).catch(errorHandler);
};

wsconn.onclose(e => {
    if (e) {
        console.log("SignalR: closed with error.");
        console.log(e);
    }
    else {
        console.log("Disconnected");
    }
});

// Hub Callback: Update User List
wsconn.on('updateUserList', (userList) => {
    $.each(userList, function (index) {
        var userIcon = '', status = '';

        $('#users li.user').remove();

        if (userList[index].username === $('#sender').text() || userList[index].username === $('#receiver').text()) {
            myConnectionId = userList[index].connectionId;
        }

        status = userList[index].inCall ? 'In Call' : 'Available';

        const isSender = userList[index].username === $('#sender').text();
        var listString = `<li id=${isSender ? 'senderEmail' : 'receiverEmail'} data-cid=${userList[index].connectionId} data-username=${userList[index].username} hidden>`;
        listString += '<a href="#"><div class="username"> ' + userList[index].username + ' hidden</div>';
        listString += '<span class="helper ' + userIcon + '" data-callstatus=' + userList[index].inCall + ' hidden></span></a></li>';
        $('#users').append(listString);
    });
});

// Hub Callback: Call Accepted
wsconn.on('callAccepted', (acceptingUser) => {
    // Callee accepted our call, let's send them an offer with our video stream
    initiateOffer(acceptingUser.connectionId, localStream); // Will use driver email in production
    // Set UI into call mode
    $('#users').attr('data-mode', 'incall');
    $("#callstatus").text('In Call');
    closeBtn.style.display = 'block';
    callBtn.style.display = 'none';
    alertify.success('Вие сте в разговор.');
});

// Hub Callback: Call Declined
wsconn.on('callDeclined', (decliningUser, reason) => {
    alertify.error(reason);

    // Back to an idle UI
    $('#users').attr('data-mode', 'idle');
    closeBtn.style.display = 'none';
    callBtn.style.display = 'block';
});

// Hub Callback: Incoming Call
wsconn.on('incomingCall', (callingUser) => {
    // Ask if we want to talk
    alertify.confirm(callingUser.username + ' Ви звъни.  Желаете ли да проведете разговор?', function (e) {
        if (e) {
            // I want to chat
            wsconn.invoke('AnswerCall', true, callingUser, group).catch(err => console.log(err));

            // So lets go into call mode on the UI
            $('#users').attr('data-mode', 'incall');
            $("#callstatus").text('В обаждане');

            closeBtn.style.display = 'block';
            callBtn.style.display = 'none';
        } else {
            // Go away, I don't want to chat with you
            wsconn.invoke('AnswerCall', false, callingUser, group).catch(err => console.log(err));
            closeBtn.style.display = 'none';
            callBtn.style.display = 'block';
        }
    });
});

// Hub Callback: WebRTC Signal Received
wsconn.on('receiveSignal', (signalingUser, signal) => {
    newSignal(signalingUser.connectionId, signal);
});

// Hub Callback: Call Ended
wsconn.on('callEnded', (signalingUser, signal) => {
    // Let the user know why the server says the call is over
    alertify.error(signal);

    // Close the WebRTC connection
    closeConnection(signalingUser.connectionId);

    // Set the UI back into idle mode
    $('#users').attr('data-mode', 'idle');
    $("#callstatus").text('Idle');
    closeBtn.style.display = 'none';
    callBtn.style.display = 'block';
});

const setUsername = (username) => {
    wsconn.invoke('Join', username)
        .catch((err) => {
            alertify.alert('<h4>Проблем с връзката</h4> Не успяхме да ви свържем със сървъра.<br/><br/>');
        });

    initializeUserMedia();
};

const errorHandler = (error) => {
    if (error.message)
        alertify.error('<h4>Възникна грешка</h4></br>Не сте включили микрофона си');
    else
        alertify.error('<h4>Възникна грешка</h4></br>Не сте включили микрофона си');
};