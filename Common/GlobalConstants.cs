namespace Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Педиамед";

        public const string PatientRoleName = "Patient";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "angavasilev@gmail.com";
        public const string AdministratorPassword = "administrator123";

        public const string DoctorId = "9858f28d-b5dd-451b-9015-148704acf2b8";
        public const string UserDoctorId = "b530acf5-8ae7-46ed-af15-6ec8534051aa";
        public const string DoctorRoleName = "Doctor";
        public const string DoctorEmail = "dr.p.petrova@abv.bg";
        public const string DoctorPassword = "ASDASD";
        public const string DoctorFirstName = "Павлина";
        public const string DoctorLastName = "Петрова";
        public const string DoctorPhone = "0878787342";
        public const int DoctorAge = 48;
        public const int YearsOfExperience = 25;
        public const string Address = "ул. Стефан Караджа 49";
        public const string Biography = "Д-р Павлина Петрова Михова е специалист педиатър и неонатолог в Плевен и Габрово с над 20 години опит. " +
            "Квалифициран специалист е в областта на трансфонтанелната и абдоминалната ехография, води детска консултация, провежда имунизации спрямо имунизациония календар на България, както и препоръчителни ваксинации." +
            "От 1998 г. до 2003 г. д-р Петрова работи като педиатър в Център за спешна медицинска помощ Габрово. " +
            "От 2003 г. до 2005 г. е ординатор в Отделение по Неонатология към УМБАЛ Георги Странски - Плевен. " +
            "От 2005 г. до 2012 г. е ординатор в Отделение по Неонатология към МБАЛ Д-р Тота Венкова - Габрово. " +
            "От 2016 г. до 2017 г. практикува в ДКЦ Авис Медика Плевен и Отделение по Неонатология към МБАЛ Авис Медика. " +
            "От 2017 г. работи в Отделение по неонатология УМБАЛ Георги Странски ЕАД - Гр. Плевен. " +
            "Към момента има и самостоятелни практики като педиатър в Плевен и Габрово.";
        public const int CityId = 582;

        public const string SuccessfulApointmentEmailMsgSubject = "Записване на час при д-р Петрова";

        public static class DateTimeFormats
        {
            public const string DateFormat = "dd/MM/yyyy";

            public const string TimeFormat = "HH:mm";

            public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        }

        public static class Messages
        {
            public const string SuccessfullAppointment = "Успешно си записахте час за {0} от {1} часа. Може да проверите Вашия и-мейл";
            public const string RatedAppointment = "Вече сте дали своята оценка";
            public const string SuccessfulRating = "Вие дадохте оценка {0}";

            public const string RequiredFieldErrorMsg = "Полето е задължително.";
            public const string LengthErrorMsg = "Полето трябва да е между {0} {1} символа.";
            public const string PasswordLengthErrorMsg = "Паролата трябва да е между {0} {1} символа.";
            public const string PasswordsNotMatchErrorMsg = "Паролите не съвпадат";

            public const string DateBeforeNowErrorMsg = "Не можете да създавате събития за минала дата";
            public const string StartDateIsAfterEndDateMsg = "Подали сте грешни часове";
            public const string FailedSlotEditMsg = "Грешка при редакция, опитайте пак";

            public const string TakenDateMsg = "Съжаляваме, но някой Ви изпревари. Опитайте да запишете друг час.";
            public const string AppointmentBeforeNowErrorMsg = "Не можете да си запишете час за минала дата";

            public const string IvalidEmailErrorMsg = "Моля въведете валиден и-мейл адрес";
            public const string AppointmentMakeEmailMsg = "Успешно си записахте час. Вашият час е от {0} часа на {1}";

            public const string PatientProfileIsNotFinishedMsg = "За да си запазите час, трябва да си довършите профила";

            public const string UnsuccessfulDoctorEditMsg = "Нещо се обърка, опитайте пак";
            public const string AppointmentCauseWrongId = "Такъв преглед не съществува!";
        }

        public static class AccountConstants
        {
            public const string Name = "Име";
            public const string FamilyName = "Фамилия";
            public const string Phone = "Тел. номер";
            public const string ChildName = "Име на дете";
            public const string AgeName = "Възраст";
            public const string EmailName = "Имейл";
            public const string AddressName = "Служебен адрес";
            public const string CityName = "Град";
            public const string SpecializationsName = "Специализации";
            public const string PictureName = "Снимка";
            public const string PictureLinkName = "Линк на снимка";
            public const string YearsExpierence = "Стаж";
            public const string BiographyName = "Биография";
            public const string PasswordName = "Парола";
            public const string RepeatPasswordName = "Повтори парола";
        }
    }
}
