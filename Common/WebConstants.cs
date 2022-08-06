namespace Common;

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
    }

    public static class HubsConstants
    {
        public const string HubReceiveMessage = "ReceiveMessage";
        public const string HubSendMessage = "SendMessage";
    }
}