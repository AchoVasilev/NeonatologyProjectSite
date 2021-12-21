document.addEventListener('DOMContentLoaded', async function () {
    let events = await attachEvents();

    let calendar = generateCalendar(events);

    calendar.render();

    function generateCalendar(events) {
        const calendarEl = document.getElementById('calendar');
        let calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridWeek',
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
                const saveBtn = document.getElementById('save');
                saveBtn.addEventListener('click', e => saveChanges(e, newDate));

                $('#doctorModal').modal();
            },
            headerToolbar: {
                center: 'title',
                end: 'prev,next today',
                start: '',
            },
            buttonText: {
                today: 'днес',
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
            events: events,
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

                let startStr = new Date(info.event.startStr).toLocaleTimeString();
                let endStr = new Date(info.event.endStr).toLocaleTimeString();
                let date = new Date(info.event.startStr).toLocaleDateString();

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

        return calendar;
    }

    async function editSlot(ev, info) {
        const hourType = document.getElementById('hourType').value;

        if (hourType == 'Свободен') {
            $('#modal').modal('hide');
            return;
        }

        const eventId = info.event.id;
        const params = {
            id: eventId,
            status: hourType
        };

        try {
            const response = await fetch('/calendar/' + eventId, {
                method: 'Put',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': document.getElementById('RequestVerificationToken').value
                },
                body: JSON.stringify(params)
            });

            if (response.ok != true) {
                const error = await response.json();
                throw new Error(error.response);
            }
        } catch (err) {
            notify(err.message);
            throw err;
        }

        $('#modal').modal('hide');

        calendar.refetchEvents();
        setTimeout(() => location.reload(), 3000);
    }

    async function saveChanges(ev, date) {
        const startDate = new Date(date);
        const endDate = new Date(date);

        const startTime = document.getElementById('start').value;
        const startHour = startTime.split(':')[0];
        const startMinutes = startTime.split(':')[1];

        const endTime = document.getElementById('end').value;
        const endHour = endTime.split(':')[0];
        const endMinutes = endTime.split(':')[1];

        startDate.setHours(startDate.getHours() + startHour);
        startDate.setMinutes(startDate.getMinutes() + startMinutes);

        endDate.setHours(endDate.getHours() + endHour);
        endDate.setMinutes(endDate.getMinutes() + endMinutes);

        const scale = document.getElementById('minutes').value;

        const params = {
            start: startDate,
            end: endDate,
            slotDurationMinutes: Number(scale)
        };

        try {
            const response = await fetch('/calendar/generate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': document.getElementById("RequestVerificationToken").value
                },
                body: JSON.stringify(params)
            });

            if (response.ok == false) {
                const error = await response.json();
                throw new Error(error.response);
            }
        } catch (err) {
            $('#doctorModal').modal('hide');
            notify(err.message);
            throw err;
        }

        $('#doctorModal').modal('hide');

        calendar.refetchEvents();

        setTimeout(() => location.reload(), 3000);
    }

    async function attachEvents() {
        let events = [];
        const response = await fetch('/calendar', {
            method: 'Get'
        });

        var eventObjs = await response.json();

        Object.values(eventObjs).forEach(x => {
            events.push({
                id: x.id,
                title: x.childFirstName + ' ' + x.appointmentCause,
                start: x.dateTime,
                end: x.end,
                allDay: false,
            });
        });

        let slots = await getSlots();

        Object.values(slots).forEach(x => {
            events.push({
                id: x.id,
                title: x.status,
                start: x.start,
                end: x.end
            });
        });

        return events;
    }

    async function getSlots() {
        const response = await fetch('/calendar/getSlots')

        const slots = await response.json();

        return slots;
    }

    function notify(msg) {
        const element = document.getElementById('errorBox');
        const output = element.querySelector('span');
        output.textContent = msg;
        element.style.display = 'block';

        setTimeout(() => element.style.display = 'none', 3000);
    }
});