let savedNotificationsForHide = [];

function updateStatus(newStatus, id) {
    let colors = {
        'Прочетено': 'green',
        'Непрочетено': 'red',
        'Запазено': 'blue'
    };

    $.ajax({
        type: 'POST',
        url: `/Notification/EditStatus`,
        data: {
            'newStatus': newStatus,
            'id': id
        },
        headers: {
            'X-CSRF-TOKEN':
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data) {
                document.getElementById(`${id}orderStatus`).innerText = newStatus;
                document.getElementById(`${id}orderStatus`).style.color = colors[newStatus];
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function deleteNotification(id) {
    $.ajax({
        type: 'POST',
        url: `/Notification/DeleteNotification`,
        data: {
            'id': id
        },
        headers: {
            'X-CSRF-TOKEN':
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data) {
                let notification = document.getElementById(id);
                document.getElementById(`all-user-notifications`).removeChild(notification);
                loadMoreNotifications(1, true);
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

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

function loadMoreNotifications(take, isForDeleted) {
    let skip = document.getElementById("all-user-notifications").children.length;
    let div = document.getElementById("all-user-notifications");

    $.ajax({
        type: "GET",
        url: `/Notification/GetMoreNotifications`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            "skip": skip,
            "take": take
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            for (var notification of data.newNotifications) {
                let result = createNotification(notification);
                div.appendChild(result);
            }
            if (!data.hasMore) {
                document.getElementById("loadMoreNotifications").disabled = true;
            }
            if (isForDeleted) {
                document.getElementById("loadLessNotifications").style.display = "none";
            } else {
                document.getElementById("loadLessNotifications").style.display = "";
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function hideNotifications(maxCount) {
    let div = document.getElementById("all-user-notifications");

    for (var i = 0; i < Math.min(maxCount, div.children.length % maxCount + div.children.length / maxCount); i++) {
        div.removeChild(div.lastChild);
    }

    if (div.children.length <= maxCount) {
        document.getElementById("loadLessNotifications").style.display = "none";
    }

    document.getElementById("loadMoreNotifications").disabled = false;
}