namespace Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Неонат";

        public const string PatientRoleName = "Patient";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "angavasilev@gmail.com";
        public const string AdministratorPassword = "administrator123";

        public const string DoctorId = "9858f28d-b5dd-451b-9015-148704acf2b8";
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

        public static class DateTimeFormats
        {
            public const string DateFormat = "dd/MM/yyyy";

            public const string TimeFormat = "HH:mm";

            public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        }

        public static class Messages
        {
            public const string SuccessfullAppointment = "Успешно си записахте час за {0} от {1} часа.";
            public const string RequiredFieldErrorMsg = "Полето е задължително.";
            public const string LengthErrorMsg = "Полето трябва да е между {0} {1} символа.";
        }
    }
}
