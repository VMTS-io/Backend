using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Enum;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Controllers;

public class EnumController : BaseApiController
{
    [HttpGet("maintenance-status")]
    public ActionResult<List<EnumResponseDto>> GetMaintenanceStatus() =>
        Ok(EnumHelper.GetEnumValues<MaintenanceStatus>());

    [HttpGet("category")]
    public ActionResult<List<EnumResponseDto>> GetCategory() =>
        Ok(EnumHelper.GetEnumValues<MaintenanceCategory>());

    [HttpGet("maintenance-type")]
    public ActionResult<List<EnumResponseDto>> GetMaintenanceType() =>
        Ok(EnumHelper.GetEnumValues<MaintenanceType>());

    [HttpGet("fault-component")]
    public ActionResult<List<EnumResponseDto>> GetFaultComponent() =>
        Ok(EnumHelper.GetEnumValues<FaultComponent>());

    [HttpGet("fault-report-status")]
    public ActionResult<List<EnumResponseDto>> GetFaultReportStatus() =>
        Ok(EnumHelper.GetEnumValues<FaultReportStatus>());

    [HttpGet("trip-report-status")]
    public ActionResult<List<EnumResponseDto>> GetTripReportStatus() =>
        Ok(EnumHelper.GetEnumValues<TripReportStatus>());

    [HttpGet("trip-status")]
    public ActionResult<List<EnumResponseDto>> GetTripStatus() =>
        Ok(EnumHelper.GetEnumValues<TripStatus>());

    [HttpGet("trip-type")]
    public ActionResult<List<EnumResponseDto>> GetTripType() =>
        Ok(EnumHelper.GetEnumValues<TripType>());

    [HttpGet("fuel-type")]
    public ActionResult<List<EnumResponseDto>> GetFuelType() =>
        Ok(EnumHelper.GetEnumValues<FuelType>());

    [HttpGet("vehicle-status")]
    public ActionResult<List<EnumResponseDto>> GetVehicleStatus() =>
        Ok(EnumHelper.GetEnumValues<VehicleStatus>());

    [HttpGet("Driving-Condition")]
    public ActionResult<List<EnumResponseDto>> GetDrivingConditions() =>
        Ok(EnumHelper.GetEnumValues<DrivingCondition>());

    [HttpGet("Part-Condtion")]
    public ActionResult<List<EnumResponseDto>> GetPartCondition() =>
        Ok(EnumHelper.GetEnumValues<PartCondition>());

    [HttpGet("Transmission-Type")]
    public ActionResult<List<EnumResponseDto>> GetTransmissiionType() =>
        Ok(EnumHelper.GetEnumValues<TransmissionType>());
}
