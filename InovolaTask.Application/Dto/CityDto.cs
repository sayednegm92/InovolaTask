using InovolaTask.Core.BaseEntities;

namespace InovolaTask.Application.Dto;

public class CityDto
{
    public string Name { get; set; }
    public string Country { get; set; }
    public decimal TemperatureC { get; set; }
}
public class EditCityDto : BaseId
{
    public string Name { get; set; }
    public string Country { get; set; }
    public decimal TemperatureC { get; set; }
}
