namespace InovolaTask.Core.BaseEntities;

public class BaseEntity : BaseId
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
