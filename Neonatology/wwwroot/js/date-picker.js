$(document).ready(function () {
    const date = document.getElementById('dp1');
    const myDatePicker = MCDatepicker.create({
        el: '#dp1',
        dateFormat: 'dd/mm/yyyy',
        disableWeekends: true,
        firstWeekday: 1,
        autoClose:true,
        minDate: new Date(),
        selectedDate: new Date(),
        disableWeekDays: [0, 1, 2, 3, 4, 5, 6],
        customWeekDays: ['Неделя', 'Понеделник', 'Вторник', 'Сряда', 'Четвъртък', 'Петък', 'Събота'],
        customMonths: ['Януари', 'Февруари', 'Март', 'Април', 'Май', 'Юни', 'Юли', 'Август', 'Септември', 'Октомври', 'Ноември', 'Декември']
    });

    date.onclick = () => myDatePicker.open();

    myDatePicker.onClose((date, formatedDate) => changeHandler());

    for (let i = 1; i <= 5; i++) {
        const day = document.getElementById(i);
        day.addEventListener('click', clickHandler);
    }

    function clickHandler(e) {
        console.log(e.target.id);
        myDatePicker.options.disableWeekDays = myDatePicker.options.disableWeekDays.filter(x => x != e.target.id);
    }

    function changeHandler() {
        let dateField = document.getElementById('selectedDate');
        let dateDoc = document.getElementById('Date');
        dateField.textContent = date.value;
        dateDoc.value = date.value;
    }

    $('.cell').click(function () {
        $('.cell').removeClass('select');
        $(this).addClass('select');
        $("#selectedTime").text(this.innerHTML);
        $("#Time").val(this.innerHTML);
    });
});