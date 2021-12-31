using System.ComponentModel.DataAnnotations.Schema;

namespace Diary.App.Database;

public class ItemType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
}