document.addEventListener('DOMContentLoaded', async function () {
    let isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    let calendar = await generateCalendar();

    calendar.render();

    async function generateCalendar() {
        const calendarEl = document.getElementById('calendar');
        let calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: isMobile ? 'dayGridDay' : 'dayGridWeek',
            slotDuration: '00:05:00',
            firstDay: 1,
            allDaySlot: false,
            nowIndicator: true,
            themeSystem: 'bootstrap',
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
                                    return await getSlots('/pleven');
                                }
                            });

                            calendar.refetchEvents();
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
                                    return await getSlots('/gabrovo');
                                }
                            });

                            calendar.refetchEvents()
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
            eventSources: [{
                id: 1,
                events: await getSlots('/pleven'),
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
                let startStr = new Date(info.event.startStr).toLocaleTimeString('bg-BG', { hour: '2-digit', minute: '2-digit' });
                let endStr = new Date(info.event.endStr).toLocaleTimeString('bg-BG', { hour: '2-digit', minute: '2-digit' });
                let dateStr = new Date(info.event.startStr).toLocaleDateString('bg-BG');

                const headerSpan = document.getElementById('title');
                headerSpan.textContent = `${info.event.extendedProps.addressCityName} ${info.event.title}: ${startStr} - ${endStr} ${dateStr}`;

                console.log(info.event.extendedProps.addressCityName);
                const form = document.getElementById('form');
                form.addEventListener('submit', ev => onSubmit(ev, info));

                $('#modal').modal();
            }
        });

        return calendar;
    }

    async function onSubmit(ev, info) {
        ev.preventDefault();

        $('#modal').modal('hide');
        const form = new FormData(ev.target);
        const childFirstName = form.get('ChildFirstName').trim();
        const appointmentCauseId = form.get('AppointmentCauseId');
        const doctorId = document.getElementById('doctorId').value;

        if (childFirstName == '') {
            return notify("Всички полета са задължителни");
        }

        const start = info.event.start;
        const end = info.event.end;

        const data = {
            doctorId,
            start,
            end,
            childFirstName,
            appointmentCauseId,
            addressId: info.event.extendedProps.addressId
        };

        try {
            const response = await fetch('/calendar/makePatientAppointment/' + info.event.id, {
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

            calendar.render();
            setTimeout(() => window.location = '/appointment/myappointments', 2000);
        } catch (err) {
            notify(err.message);
            throw err;
        }
    }

    async function getSlots(url) {
        const response = await fetch('/calendar/getSlots' + url)

        const slots = await response.json();

        let slotsArr = [];

        Object.values(slots).forEach(x => {
            slotsArr.push({
                id: x.id,
                title: x.status,
                start: x.start,
                end: x.end,
                addressCityName: x.addressCityName,
                addressId: x.addressId
            });
        });

        return slotsArr;
    }
});