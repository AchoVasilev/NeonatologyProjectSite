namespace ViewModels.GoogleRecaptcha
{
    using System;

    public class RecaptchaResponse
    {
        public bool Success { get; set; }

        public double Score { get; set; }

        public string Action { get; set; }

        public DateTime ChallengeTimeStamp { get; set; }

        public string Hostname { get; set; }
    }
}
