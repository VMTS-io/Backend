namespace VMTS.API.Dtos;

public class UpdateFaultReportRequest
{
    public string Details { get; set; }

    public string FaultAddress { get; set; }
    public decimal Cost { get; set; }
    public int FuelRefile { get; set; }
}