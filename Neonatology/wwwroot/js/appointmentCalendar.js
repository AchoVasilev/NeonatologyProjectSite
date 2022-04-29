var calendar; document.addEventListener('DOMContentLoaded', async function () {
    let isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    alertify.set('notifier', 'position', 'top-center');

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
            bootstrapFontAwesome: {
                prev: 'fas fa-arrow-circle-left',
                next: 'fas fa-arrow-circle-right'
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
                        }

                        event.target.disabled = true;
                        const plevenBtn = document.querySelector('.fc-plevenButton-button');
                        plevenBtn.disabled = false;
                    }
                }
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

                const form = document.getElementById('form');
                form.addEventListener('submit', ev => onSubmit(ev, info));

                $('#modal').modal();
            }
        });

        return calendar;
    }

    async function onSubmit(ev, info) {
        ev.preventDefault();

        const form = new FormData(ev.target);
        const parentFirstName = form.get('ParentFirstName').trim();
        const parentLastName = form.get('ParentLastName').trim();
        const childFirstName = form.get('ChildFirstName').trim();
        const appointmentCauseId = form.get('AppointmentCauseId');
        const phoneNumber = form.get('PhoneNumber').trim();
        const email = form.get('Email').trim();
        const doctorId = document.getElementById('doctorId').value;

        if (parentFirstName == '' || parentLastName == '' || childFirstName == '' || phoneNumber == '') {
            return alertify.error("Всички полета са задължителни");
        }

        const saveBtn = document.getElementById('saveBtn');
        saveBtn.disabled = true;
        $('#modal').modal('hide');

        const start = info.event.start;
        const month = start.getMonth() + 1;
        const startDate = start.getDate() + '/' + month + '/' + start.getFullYear() + ' ' + start.toLocaleTimeString();
        const end = info.event.end;
        const endDate = end.getDate() + '/' + month + '/' + end.getFullYear() + ' ' + end.toLocaleTimeString();

        const data = {
            doctorId,
            start: startDate,
            end: endDate,
            parentFirstName,
            parentLastName,
            childFirstName,
            appointmentCauseId,
            phoneNumber,
            email,
            addressId: info.event.extendedProps.addressId
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
            ev.target.reset();
            alertify.success(obj.message);
        } catch (err) {
            alertify.error(err.message);
            throw err;
        }

        calendar.refetchEvents();
        saveBtn.disabled = false;
        setTimeout(() => {
            window.location.reload();
        }, 3000)
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