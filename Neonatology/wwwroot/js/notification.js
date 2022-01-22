"use strict"

let notificationConnection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

notificationConnection.start().then(function () {
    if (!sessionStorage.getItem("isFirstNotificationSound")) {
        sessionStorage.setItem("isFirstNotificationSound", true);
    } else {
        sessionStorage.setItem("isFirstNotificationSound", false);
    }

    let isFirstNotificationSound = sessionStorage.getItem("isFirstNotificationSound") == "true" ? true : false;
    notificationConnection.invoke("GetUserNotificationCount", isFirstNotificationSound).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

notificationConnection.on("ReceiveNotification", function (count, isFirstNotificationSound) {
    const notificationCountElement = document.getElementById("notificationCount");
    notificationCountElement.dataset.count = count;

    if (count > 0) {
        if (isFirstNotificationSound) {
            document.querySelector("audio").load();
            document.querySelector("audio").play();
        }
    }
});

notificationConnection.on("VisualizeNotification", function (notification) {
    let div = document.getElementById("all-user-notifications");

    if (div) {
        let newNotification = createNotification(notification);
        if (div.children.length % 5 == 0 && div.children.length > 0) {
            let lastNotification = div.lastElementChild;
            div.removeChild(lastNotification);
            document.getElementById("loadMoreNotifications").disabled = false;
        }
        if (div.children.length == 0) {
            div.appendChild(newNotification);
        } else {
            div.insertBefore(newNotification, div.childNodes[0]);
        }
    }
});

function createNotification(notification) {
    let newNotification = document.createElement('div');
    newNotification.classList.add('p-3', 'd-flex', 'align-items-center', 'bg-light', 'border-bottom', 'osahan-post-header');
    newNotification.id = notification.id;

    let allStatuses = '';

    for (var status of notification.allStatuses) {
        allStatuses += `<a onclick="updateStatus('${status}', '${notification.id}')">${status}</a>`;
    }

    newNotification.innerHTML =
        `<div class="dropdown-list-image mr-3">
                                            <img class="rounded-circle" src="${notification.ImageUrl}" alt="avatar" />
                                        </div>
                                        <div class="font-weight-bold mr-3">
                                            <span>
                                                <a class="mdi mdi-delete" onclick="deleteNotification('${notification.Id}')">
                                                    <i class="fas fa-trash-alt"></i>
                                                </a>
                                            </span>
                                            <div class="text-truncate">${notification.Heading}</div>
                                            <div class="small">${notification.Text}</div>
                                        </div>
                                        <span class="ml-auto mb-auto">
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-light btn-sm rounded" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                    <i class="fas fa-ellipsis-v"></i>
                                                </button>
                                                <div class="dropdown-menu dropdown-menu-right">
                                                    ${allStatuses}
                                                    <span class="dropdown-item">Статус: </span>
                                                    <b>
                                                        <span id="${notification.Id}orderStatus" style="color: red; text-transform: uppercase; text-align: center">
                                                            ${notification.allStatuses[notification.status - 1]}
                                                        </span>
                                                    </b>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="text-right text-muted pt-1">${notification.CreatedOn}</div>
                                        </span>`;

    return newNotification;
}