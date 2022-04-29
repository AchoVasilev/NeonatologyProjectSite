document.addEventListener('DOMContentLoaded', async function () {
    let isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    alertify.set('notifier', 'position', 'top-center');

    let calendar = generateCalendar();
    calendar.render();

    function generateCalendar() {
        const calendarEl = document.getElementById('calendar');
        let newCalendar = new FullCalendar.Calendar(calendarEl, {
            initialView: isMobile ? 'dayGridDay' : 'dayGridWeek',
            slotDuration: '00:05:00',
            firstDay: 1,
            allDaySlot: false,
            nowIndicator: true,
            themeSystem: 'bootstrap',
            navLinks: true,
            navLinkDayClick: function (date, jsEvent) {
                const dateText = jsEvent.target.textContent;
                const title = document.getElementById('doctorTitle');
                title.textContent = dateText;

                const newDate = new Date(date);

                $('#doctorModal').modal();
                const slotForm = document.getElementById('slotsForm');
                slotForm.addEventListener('submit', e => saveChanges(e, newDate, slotForm), false);
            },
            headerToolbar: {
                center: 'title',
                end: 'prev,next today',
                start: 'plevenButton gabrovoButton',
            },
            buttonText: {
                today: 'днес',
            },
            customButtons: {
                plevenButton: {
                    text: 'Плевен',
                    click: async function (event) {
                        let sourceToRemove = calendar.getEventSourceById(2);
                        if (sourceToRemove) {
                            sourceToRemove.remove();
                        }

                        let sourceToAdd = calendar.getEventSourceById(1);
                        if (!sourceToAdd) {
                            calendar.addEventSource({
                                id: 1, events: async function (info, successCallback, failureCallback) {
                                    return await loadEvents('/pleven');
                                }
                            });

                        }

                        event.target.disabled = true;
                        const gabrovoBtn = document.querySelector('.fc-gabrovoButton-button');
                        gabrovoBtn.disabled = false;
                    }
                },
                gabrovoButton: {
                    text: 'Габрово',
                    click: async function (event) {
                        let sourceToRemove = calendar.getEventSourceById(1);
                        if (sourceToRemove) {
                            sourceToRemove.remove();
                        }

                        let sourceToAdd = calendar.getEventSourceById(2);
                        if (!sourceToAdd) {
                            calendar.addEventSource({
                                id: 2, events: async function (info, successCallback, failureCallback) {
                                    return await loadEvents('/gabrovo');
                                }
                            });

                        }

                        event.target.disabled = true;
                        const plevenBtn = document.querySelector('.fc-plevenButton-button');
                        plevenBtn.disabled = false;
                    }
                }
            },
            bootstrapFontAwesome: {
                prev: 'fas fa-arrow-circle-left',
                next: 'fas fa-arrow-circle-right'
            },
            locale: 'bg',
            slotMinTime: '08:00',
            slotMaxTime: '20:00',
            businessHours: {
                daysOfWeek: [1, 5],
                startTime: '09:00',
                endTime: '14:00'
            },
            eventSources: [
                {
                    id: 1,
                    events: async function (info, successCallback, failureCallback) {
                        return await loadEvents('/pleven');
                    }
                }
            ],
            //events: async function (info, successCallback, failureCallback) {
            //   return await attachEvents();
            //},
            eventTimeFormat: {
                hour: '2-digit',
                minute: '2-digit',
                meridiem: false,
                hour12: false
            },
            eventColor: '#378006',
            eventDisplay: 'block',
            eventClick: function (info) {
                const headerSpan = document.getElementById('eventTitle');
                headerSpan.textContent = info.event.title;
                const details = document.getElementById('pDetails');

                const description = document.createElement('div');
                const pStartElement = document.createElement('p');
                const strongStartEl = document.createElement('strong');

                let startStr = new Date(info.event.startStr).toLocaleTimeString('bg-BG');
                let endStr = new Date(info.event.endStr).toLocaleTimeString('bg-BG');
                let date = new Date(info.event.startStr).toLocaleDateString('bg-BG');

                const h4Element = document.createElement('h4');
                h4Element.textContent = 'Дата:' + ' ' + date;

                strongStartEl.textContent = 'Начален час:' + ' ' + startStr;

                const pEndElement = document.createElement('p');
                const strongEndEl = document.createElement('strong');
                strongEndEl.textContent = 'Краен час:' + ' ' + endStr;

                pStartElement.appendChild(strongStartEl);
                pEndElement.appendChild(strongEndEl);

                description.appendChild(h4Element);
                description.appendChild(pStartElement);
                description.appendChild(pEndElement);

                $(details).empty().html(description);

                const saveBtn = document.getElementById('saveBtn');
                saveBtn.addEventListener('click', ev => editSlot(ev, info));

                $('#modal').modal();
            }
        });

        return newCalendar;
    }

    async function editSlot(ev, info) {
        const hourType = document.getElementById('hourType').value;
        const text = document.getElementById('textId').value;

        if (hourType == 'Свободен') {
            $('#modal').modal('hide');
            return;
        }

        const eventId = info.event.id;
        const params = {
            id: eventId,
            text: text,
            status: hourType
        };

        try {
            const response = await fetch('/doctorCalendar/' + eventId, {
                method: 'Put',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': document.getElementById('RequestVerificationToken').value
                },
                body: JSON.stringify(params)
            });

            if (response.ok != true) {

                const error = await response.json();

                if (response.status == 400) {
                    alertify.error(err.message);
                }

                throw new Error(error.response);
            }
        } catch (err) {
            throw err;
        }

        $('#modal').modal('hide');

        calendar.refetchEvents();
    }

    async function saveChanges(ev, date, slotForm) {
        ev.preventDefault();
        const form = new FormData(slotForm);
        const slotDurationMinutes = form.get('interval');
        const addressId = form.get('address-id');
        const endHour = form.get('endHour');
        const startHour = form.get('startHour');

        const month = date.getMonth() + 1;
        const startStr = date.getDate() + '/' + month + '/' + date.getFullYear() + ' ' + startHour;
        const endStr = date.getDate() + '/' + month + '/' + date.getFullYear() + ' ' + endHour;

        const params = {
            startDate: startStr,
            endDate: endStr,
            slotDurationMinutes: Number(slotDurationMinutes),
            addressId
        };

        try {
            const response = await fetch('/doctorCalendar/generate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': document.getElementById('RequestVerificationToken').value
                },
                body: JSON.stringify(params)
            });

            if (response.ok == false) {
                const error = await response.json();
                throw new Error(error.response);
            }
        } catch (err) {
            $('#doctorModal').modal('hide');

            if (err.status == 400) {
                alertify.error(err.message);
            }

            throw err;
        }

        $('#doctorModal').modal('hide');

        calendar.refetchEvents();
    }

    async function attachEvents(url) {
        let events = [];
        const response = await fetch('/doctorCalendar' + url, {
            method: 'Get'
        });

        var eventObjs = await response.json();

        Object.values(eventObjs).forEach(x => {
            events.push({
                id: x.id,
                title: x.childFirstName + ' ' + x.appointmentCauseName,
                start: x.dateTime,
                end: x.end,
                allDay: false,
                addressCityName: x.addressCityName
            });
        });

        let slots = await getSlots(url);

        Object.values(slots).forEach(x => {
            events.push({
                id: x.id,
                title: `${x.status} ${x.text ? x.text : ''}`,
                start: x.start,
                end: x.end,
                addressCityName: x.addressCityName
            });
        });

        return events;
    }

    async function getSlots(url) {
        const response = await fetch('/doctorCalendar/getSlots' + url)

        const slots = await response.json();

        return slots;
    }

    async function loadEvents(url) {
        let allEvents = await attachEvents(url);

        return allEvents;
    }
});