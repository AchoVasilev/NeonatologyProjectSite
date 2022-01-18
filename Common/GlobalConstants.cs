namespace Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Педиамед";
        public const string SystemAdministrationName = "Педиамед Администрация";

        public const string PatientRoleName = "Patient";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "pediamedbg@gmail.com";
        public const string AdministratorPassword = "administrator123";

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
            public const string ErrorApprovingRating = "Възникна грешка с одобряването, опитайте пак";
            public const string SuccessfullyApprovedRating = "Одобрихте успешно оценката";
            public const string ErrorDeleting = "Възникна грешка с изтриването, опитайте пак";
            public const string SuccessfullyDeletedRating = "Успешно изтрихте оценката";

            public const string RequiredFieldErrorMsg = "Полето е задължително.";
            public const string LengthErrorMsg = "Полето трябва да е между {0} и {1} символа.";
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

            public const string UnsuccessfulEditMsg = "Нещо се обърка, опитайте пак";
            public const string AppointmentCauseWrongId = "Такъв преглед не съществува!";
            public const string PatientIsRegistered = "Моля, използвайте платформата, за да си запишете час.";

            public const string SuccessfulFeedbackSent = "Успешно изпратихте вашето съобщение. Ще получите отговор на и-мейла си.";
            public const string SuccessfulDelete = "Изтриването беше успешно.";
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

        public static class NotificationConstants
        {
            public const int MaxChatNotificationsPerUser = 5;
        }

        public static class FileConstants
        {
            public const string DefaultFolderName = "pediamed";
            public const string ProfileFolderName = $"{DefaultFolderName}/userProfiles";
            public const string ChatFolderName = $"{DefaultFolderName}/pediamedChat";
            public const string ChatFileName = "{0}-ChatFile";
            public const string NoProfilePicUrl = "/images/NoAvatarProfileImage.png";
        }

        public static class ChatConstants
        {
            public const int SavedChatMessagesCount = 200;
            public const string ChatGroupNameSeparator = "->";
            public const int MessagesCountPerScroll = 10;
        }
    }
}
