using InovolaTask.Core.BaseEntities;

namespace InovolaTask.Core.Entities;
public class City : BaseEntity
{
    public string Name { get; set; }
    public string Country { get; set; }
    public decimal TemperatureC { get; set; }

}
