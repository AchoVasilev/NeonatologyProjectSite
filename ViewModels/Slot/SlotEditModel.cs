namespace ViewModels.Slot
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Constants;

    public class SlotEditModel
    {
        public int Id { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string Text { get; set; }

        public string Status { get; set; }
    }
}
