namespace Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Data.Common.Models;

public class ChatImage : BaseModel<string>
{
    public ChatImage()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [MaxLength(60)]
    public string Name { get; set; }

    public string Url { get; set; }

    [ForeignKey(nameof(Group))]
    public string GroupId { get; set; }

    public Group Group { get; set; }

    [ForeignKey(nameof(Message))]
    public int MessageId { get; set; }

    public Message Message { get; set; }
}