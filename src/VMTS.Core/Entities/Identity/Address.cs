namespace VMTS.Core.Entities.Identity;


public class Address
{
        public int Id { get; set; }
        public string Street { get; set; } // Example: "El Tahrir St."
        public string Area { get; set; } // Example: "Nasr City, Maadi, Mohandessin"
        public string Governorate { get; set; } // Example: "Cairo, Giza, Alexandria"
        public string Country { get; set; }  // Default value since your system is for Egypt
        
        
        public string AppUserId { get; set; } // foreign Key
}

