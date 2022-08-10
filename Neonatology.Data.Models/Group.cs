namespace Neonatology.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models;

public class Group : BaseModel<string>
{
    public Group()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [MaxLength(60)]
    public string Name { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

    public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

    public ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();
}