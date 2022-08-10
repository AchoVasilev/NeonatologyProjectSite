namespace Neonatology.Common.Constants;

public static class WebConstants
{
    public static class RouteTemplates
    {
        public const string ChatWithUser = "Chat/With/{username?}/Group/{group?}";
        public const string ChatSendFiles = "Chat/With/{toUsername?}/Group/{group?}/SendFiles";
        public const string ChatLoadMoreMessages =
            "[controller]/With/{username?}/Group/{group?}/LoadMoreMessages/{messagesSkipCount?}";

        public const string CheckoutConfig = "config";
        public const string CheckoutGetCheckoutSession = "checkout-session";
        public const string CheckoutWebhook = "stripe";

        public const string HomeIndex = "/Index";
        public const string HomeIndexSlash = "/";
        public const string HomeError404 = "/Home/Error/404";
        public const string HomeError400 = "/Home/Error/400";

        public const string NotificationEditStatus = "/Notification/EditStatus";
        public const string NotificationDelete = "/Notification/DeleteNotification";
        public const string NotificationGetMore = "/Notification/GetMoreNotifications";

        public const string CalendarGetGabrovoSlots = "getSlots/gabrovo";
        public const string CalendarGetPlevenSlots = "getSlots/pleven";
        public const string CalendarMakeAnAppointment = "makeAppointment/{id}";
        public const string CalendarMakePatientAppointment = "makePatientAppointment/{id}";

        public const string DoctorCalendarGetGabrovoAppointments = "gabrovo";
        public const string DoctorCalendarGetGabrovoSlots = "getSlots/gabrovo";
        public const string DoctorCalendarGetPlevenAppointments = "pleven";
        public const string DoctorCalendarGetPlevenSlots = "getSlots/pleven";
        public const string DoctorCalendarGenerate = "generate";
        public const string DoctorCalendarEditSlot = "{id}";
    }

    public static class CheckoutConstants
    {
        public const string SuccessUrl =
            "https://pediamedbg.com/checkout/successfulpayment?session_id={{CHECKOUT_SESSION_ID}}";

        public const string CancelUrl = "https://pediamedbg.com/checkout/canceledpayment";
        public const string SessionOptionsMode = "payment";
        public const string Currency = "bgn";

        public const string OnlineConsultationUrl =
            "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1641585449/pediamed/onlineConsultation_vyvebl.jpg";

        public const string CheckoutSessionCompleted = "checkout.session.completed";
        public const string LocationHeader = "Location";
        public const string StripeSignatureHeader = "Stripe-Signature";
        public const int Quantity = 1;
        public const int Multiplier = 100;
        public const int HangFireBackgroundJobDays = 1;
    }

    public static class HubsConstants
    {
        public const string HubReceiveMessage = "ReceiveMessage";
        public const string HubSendMessage = "SendMessage";
    }

    public static class ServiceCollectionExtensionsConstants
    {
        public const string LoginPath = "/Identity/Account/Login";
        public const string LogoutPath = "/Identity/Account/Logout";
        public const string AccessDeniedPath = "/Identity/Account/AccessDenied";
        public const string XCSRFHeader = "X-CSRF-TOKEN";
        public const int IdentityPasswordUniqueChars = 0;
        public const int MaxFailedAccessAttempts = 5;
        public const int DefaultLockoutTimeSpan = 15;
        public const int HangFireTimeSpan = 5;
    }

    public static class ApplicationBuilderExtensionsConstants
    {
        public const string AreasName = "areas";
        public const string AreasPattern = "{area:exists}/{controller=Home}/{action=Index}/{id?}";

        public const string DefaultName = "default";
        public const string DefaultPattern = "{controller=Home}/{action=Index}/{id?}";

        public const string ChatHubPattern = "/chatHub";
        public const string NotificationHubPattern = "/notificationHub";
        public const string ConnectionHubPattern = "/connectionHub";
    }
}