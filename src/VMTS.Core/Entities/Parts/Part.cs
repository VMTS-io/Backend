﻿namespace VMTS.Core.Entities.Parts;

public class Part : BaseEntity
{
    public string Name { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Cost { get; set; }
}