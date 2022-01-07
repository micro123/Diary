using System.ComponentModel.DataAnnotations.Schema;

namespace Diary.Core.Entities;

public class ItemType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }
}