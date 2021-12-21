var calendar;
document.addEventListener('DOMContentLoaded', async function () {
    calendar = await generateCalendar();

    calendar.render();

    async function generateCalendar() {
        const calendarEl = document.getElementById('calendar');
        let calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridWeek',
            slotDuration: '00:05:00',
            firstDay: 1,
            allDaySlot: false,
            nowIndicator: true,
            themeSystem: 'bootstrap',
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
            eventSources: [{
                id: 1,
                events: await attachEvents(),
            },
            {
                id: 2,
                events: await getSlots()
            }],
            eventTimeFormat: {
                hour: '2-digit',
                minute: '2-digit',
                meridiem: false,
                hour12: false
            },
            eventColor: '#378006',
            eventDisplay: 'block',
            eventClick: function (info) {
                let startStr = new Date(info.event.startStr).toLocaleTimeString();
                let endStr = new Date(info.event.endStr).toLocaleTimeString();
                let dateStr = new Date(info.event.startStr).toLocaleDateString();

                const headerSpan = document.getElementById('title');
                headerSpan.textContent = info.event.title + ': ' + startStr + '-' + endStr + ' ' + dateStr;

                if (info.event.title == 'Зает') {
                    const smallHeaderSpan = document.getElementById('eventTitle');
                    smallHeaderSpan.textContent = info.event.title;
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

                    $('#smallModal').modal();
                } else {
                    const form = document.getElementById('form');
                    form.addEventListener('submit', ev => onSubmit(ev, info));

                    $('#modal').modal();
                }
            }
        });

        return calendar;
    }

    async function onSubmit(ev, info) {
        ev.preventDefault();

        $('#modal').modal('hide');
        const form = new FormData(ev.target);
        const parentFirstName = form.get('ParentFirstName').trim();
        const parentLastName = form.get('ParentLastName').trim();
        const childFirstName = form.get('ChildFirstName').trim();
        const appointmentCause = form.get('AppointmentCause');
        const phoneNumber = form.get('PhoneNumber').trim();
        const email = form.get('Email').trim();
        const doctorId = document.getElementById('doctorId').value;

        if (parentFirstName == '' || parentLastName == '' || childFirstName == '' || phoneNumber == '') {
            return notify("Всички полета са задължителни");
        }

        const start = info.event.start;
        const end = info.event.end;

        const data = {
            doctorId,
            start,
            end,
            parentFirstName,
            parentLastName,
            childFirstName,
            appointmentCause,
            phoneNumber,
            email
        };

        try {
            const response = await fetch('/calendar/makeAppointment/' + info.event.id, {
                method: 'Post',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': document.getElementById('RequestVerificationToken').value
                },
                body: JSON.stringify(data)
            });

            if (response.ok != true) {
                const error = await response.json();
                throw new Error(error.message);
            }

            const obj = await response.json();
            notify(obj.message);
        } catch (err) {
            notify(err.message);
            throw err;
        }

        calendar.refetchEvents();
        setTimeout(() => location.reload(), 3000);
    }

    async function attachEvents() {
        let events = [];
        const response = await fetch('/calendar/appointments', {
            method: 'Get'
        });

        var eventObjs = await response.json();

        Object.values(eventObjs).forEach(x => {
            events.push({
                id: x.id,
                title: x.status,
                start: x.start,
                end: x.end,
                allDay: false,
            });
        });

        console.log(events);
        return events;
    }

    async function getSlots() {
        const response = await fetch('/calendar/getSlots')

        const slots = await response.json();

        let slotsArr = [];

        Object.values(slots).forEach(x => {
            slotsArr.push({
                id: x.id,
                title: x.status,
                start: x.start,
                end: x.end
            });
        });

        return slotsArr;
    }

    function notify(msg) {
        const element = document.getElementById('errorBox');
        const output = element.querySelector('span');
        output.textContent = msg;
        element.style.display = 'block';

        setTimeout(() => element.style.display = 'none', 3000);
    }
});