﻿using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.API.Dtos;

public class FaultReportRequest
{ 
    public MaintenanceType FaultType { get; set; }
    public string Address { get; set; }
    public string Details { get; set; } = string.Empty;
}