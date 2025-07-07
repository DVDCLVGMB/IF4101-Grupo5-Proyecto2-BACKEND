public class OrderRequestDto
{
    public int OrderId { get; set; }      // 0 al crear
    public int ClientId { get; set; }
    public int EmployeeId { get; set; }
    public int CityId { get; set; }
    public DateTime OrderDate { get; set; }
}