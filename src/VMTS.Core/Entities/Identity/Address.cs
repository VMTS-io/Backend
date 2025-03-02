namespace VMTS.Core.Entities.Identity;


public class Address
{
        public int Id { get; set; }
        public string Street { get; set; } // Example: "El Tahrir St."
        public string Area { get; set; } // Example: "Nasr City, Maadi, Mohandessin"
        public string Governorate { get; set; } // Example: "Cairo, Giza, Alexandria"
        public string PostalCode { get; set; } // Example: "11765" (Egyptian ZIP code)
        public string Country { get; set; }  // Default value since your system is for Egypt
        
        // Optional Properties
        public string? BuildingNumber { get; set; } // Example: "12A"
        public string? Floor { get; set; } // Example: "5th Floor"
        public string? ApartmentNumber { get; set; } // Example: "Flat 10"
        public string? Landmark { get; set; } // Example: "Near City Stars Mall"
        
        public string AppUserId { get; set; } // foreign Key
}

