namespace ViewModels.Profile
{
    using System;

    public class ProfileViewModel
    {
        public string Id { get; init; }

        public string UserId { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string ImageUrl { get; init; }

        public string Phone { get; init; }

        public string Address { get; init; }

        public string CityName { get; init; }

        public string UserEmail { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
