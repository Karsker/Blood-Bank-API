namespace BloodBankMgmt.Models
{ 

    public class BloodBankEntry
    {
        static Random random = new Random();

        // FIELDS
        // Unique identifier
        int id;

        // Name of the donor
        string? donorName = null;

        // Age of the donor
        int? donorAge = null;

        // Type of blood
        string? donorBloodType = null;

        // Contact email
        string? contactInfo = null;

        // Quantity of blood in ml
        double? quantity = null;

        // Collection date
        DateTime? collectionDate = null;

        // Expiration date
        DateTime? expirationDate = null;

        // Status
        string? status = null;

        // PROPERTIES
        public int Id { get { return id; } set { id = value; } }
        public string? DonorName { get { return donorName; } set { donorName = value; } }
        public int? DonorAge { get { return donorAge; } set { donorAge = value; } }
        public string? DonorBloodType { get { return donorBloodType; } set { donorBloodType = value?.ToUpper(); } }

        public string? ContactInfo { get { return contactInfo; } set { contactInfo = value; } }

        public double? Quantity { get { return quantity; } set { quantity = value; } }

        public DateTime? CollectionDate { get { return collectionDate; } set { collectionDate = value; } }

        public DateTime? ExpirationDate { get { return expirationDate; } set { expirationDate = value; } }

        public string? Status { get { return status; } set { status = value?.ToUpper(); } }

        // Constructor
        public BloodBankEntry() { }
        public BloodBankEntry(int id, string donorName, int donorAge, string contactInfo, string donorBloodType, double quantity, string collectionDate, string expirationDate, string status)
        {
            this.id = id;
            this.donorName = donorName;
            this.donorAge = donorAge;
            this.donorBloodType = donorBloodType;
            this.contactInfo = contactInfo;
            this.quantity = quantity;
            this.status = status;
            this.collectionDate = DateTime.Parse(collectionDate);
            this.expirationDate = DateTime.Parse(expirationDate);
        }

        // Function to generate a unique ID of 10 characters
        string GenerateID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
