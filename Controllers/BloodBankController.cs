using BloodBankMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BloodBankMgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodBankController : ControllerBase
    {
        // Valid blood types
        static string[] bloodTypes = { "APOS", "ANEG", "BPOS", "BNEG", "ABPOS", "ABNEG", "OPOS", "ONEG" };

        // Valid blood status
        static string[] bloodStatus = { "AVAILABLE", "EXPIRED", "REQUESTED" };

        // Internal list to hold blood bank entries
        static List<BloodBankEntry> entries = new List<BloodBankEntry> {
            new BloodBankEntry(1, "John Doe", 35, "johndoe@email.com", "APOS", 450.04, "2024-10-10", "2024-11-10", "AVAILABLE"),

            new BloodBankEntry(2, "Jane Smith", 28, "janesmith@email.com", "BNEG", 422.77, "2024-10-19", "2024-11-19", "EXPIRED"),

            new BloodBankEntry(3, "Michael Johnson", 42, "mjohnson@email.com", "ABPOS", 440.13, "2024-11-12", "2024-12-12", "AVAILABLE"),

            new BloodBankEntry(4, "Emily Brown", 31, "ebrown@email.com", "ONEG", 488.13, "2024-11-10", "2024-11-11", "REQUESTED"),

            new BloodBankEntry(5, "David Lee", 39, "dlee@email.com", "BPOS", 399.87, "2024-09-16", "2024-10-16", "AVAILABLE"),

            new BloodBankEntry(6, "Sarah Wilson", 45, "swilson@email.com", "ANEG", 412.91, "2024-10-17", "2024-11-17", "AVAILABLE"),

            new BloodBankEntry(7, "Robert Taylor", 33, "rtaylor@email.com", "OPOS", 449.81, "2024-10-21", "2024-11-21", "EXPIRED"),

            new BloodBankEntry(8, "Lisa Anderson", 29, "landerson@email.com", "ABNEG", 450.42, "2024-09-26", "2024-10-26", "AVAILABLE"),

            new BloodBankEntry(9, "Thomas Martinez", 37, "tmartinez@email.com", "APOS", 441.66, "2024-06-12", "2024-07-12", "AVAILABLE"),

            new BloodBankEntry(10, "Jennifer Garcia", 41, "jgarcia@email.com", "BPOS", 421.18, "2024-10-17", "2024-11-17", "REQUESTED"),

        };


        // Function to check if an email is valid
        static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w{2,3}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        // Function to check if a blood bank entry is valid
        static bool EntryIsValid(BloodBankEntry entry, out string? message)
        {
            if (entry == null)
            {
                message = "No date found";
                return false;
            }

            // Check if all the fields were populated
            if (entry.DonorName == null) {
                message = "Missing field: donorName";
                return false;
            }

            if (entry.DonorAge == null)
            {
                message = "Missing field: donorAge";
                return false;
            }

            if (entry.DonorAge < 18)
            {
                message = "Donor should be at least 18 years old";
                return false;
            }

            if (entry.ContactInfo == null) {
                message = "Missing field: contactInfo";
                return false;
            }

            if (entry.CollectionDate == null)
            {
                message = "Missing field: collectionDate";
                return false;
            }

            if (entry.ExpirationDate == null) {
                message = "Missing field: expirationDate";
                return false;
            }

            if (entry.Status == null)
            {
                message = "Missing field: status";
                return false;
            }

            if (entry.DonorBloodType == null) {
                message = "Missing field: donorBloodType";
                return false;
            }

            if (entry.Quantity == null)
            {
                message = "Missing field: quantity";
                return false;
            }


            // Check if contact info (email) is valid
            if (!IsValidEmail(entry.ContactInfo))
            {
                message = "Invalid contactInfo format (Should be an email address)";
                return false;
            }
            // Check if blood type is valid
            if (!bloodTypes.Contains(entry.DonorBloodType.ToUpper())) {
                message = $"Invalid blood type: {entry.DonorBloodType}";
                return false;
            }

            // Check if status is valid
            if (!bloodStatus.Contains(entry.Status.ToUpper())) {
                message = $"Invalid staus: {entry.Status}";
                return false;
            }

            // Check if quantity is valid
            if (entry.Quantity <= 0)
            {
                message = $"Blood quantity should be greater than 0";
                return false;
            }

            // Check if expiration date is after collection date
            if (entry.CollectionDate > entry.ExpirationDate)
            {
                message = "Expiration date cannot be before collection date";
                return false;
            }
            message = null;
            return true;
        }

        // 1. Get all entries
        [HttpGet]
        public ActionResult<IEnumerable<BloodBankEntry>> GetAll(int pages = 0, int size = 0, string? sortBy = null)
        {
            if (pages != 0 && size != 0)
            {   
                var res = entries.Skip((pages-1)*size).Take(size).ToList();
                return res;
            }

            // Check if sort parameter is provided
            if (sortBy != null)
            {
                if (sortBy.Equals("donorName", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.DonorName).ToList();
                    return res;
                }

                if (sortBy.Equals("age", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.DonorAge).ToList();
                    return res;
                }

                if (sortBy.Equals("quantity", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.Quantity).ToList();
                    return res;
                }

                if (sortBy.Equals("status", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.Status).ToList();
                    return res;
                }

                if (sortBy.Equals("collectionDate", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.CollectionDate).ToList();
                    return res;
                }

                if (sortBy.Equals("expirationDate", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.ExpirationDate).ToList();
                    return res;
                }

                if (sortBy.Equals("donorBloodType", StringComparison.OrdinalIgnoreCase))
                {
                    var res = entries.OrderBy(e => e.DonorBloodType).ToList();
                    return res;
                }

            }
            return entries;
        }

        // 2. Get an entry using ID
        [HttpGet("{id}")]
        public ActionResult<BloodBankEntry> GetById(int id)
        {
            // Check if the entry exists
            var entry = entries.Find(e => e.Id == id);
            if (entry == null)
            {
                return NotFound("Entry not found in blood bank");
            }

            return entry;
        }

        // 2. Add an entry
        [HttpPost]
        public ActionResult<BloodBankEntry> Add(BloodBankEntry entry)
        {
            string? message;
            // Check if the entry is valid
            bool valid = EntryIsValid(entry, out message);
            if (!valid)
            {
                return BadRequest(message);
            }
             

            // If there are no entries in the blood bank, assign 1 as ID
            // Otherwise assign the maximum ID + 1
            entry.Id = entries.Any() ? (entries.Max(e => e.Id) + 1) : 1;
            entries.Add(entry);
            return entry;
        }


        // 3. Update an existing entry
        [HttpPut]
        public ActionResult<BloodBankEntry> Update(BloodBankEntry entry) { 
            var entryInMem = entries.Find(e => e.Id == entry.Id);
            if (entryInMem == null) {
                return NotFound("Entry not found");
            }

            // Update the entry
            if (entry.Status != null && bloodStatus.Contains(entry.Status.ToUpper()))
            {
                entryInMem.Status = entry.Status;
            }

            if (entry.DonorAge != null && entry.DonorAge >= 18)
            {
                entryInMem.DonorAge = entry.DonorAge;
            }

            if (entry.DonorBloodType != null && bloodTypes.Contains(entry.DonorBloodType.ToUpper()))
            {
                entryInMem.DonorBloodType = entry.DonorBloodType;
            }

            if (entry.ContactInfo != null && !IsValidEmail(entry.ContactInfo))
            {
                entryInMem.ContactInfo = entry.ContactInfo;
            }

            if (entry.DonorName != null)
            {
                entry.DonorName = entry.DonorName;
            }

            if (entry.Quantity != null && entry.Quantity > 0)
            {
                entryInMem.Quantity = entry.Quantity;
            }

            if (entry.CollectionDate != null)
            {
                entryInMem.CollectionDate = entry.CollectionDate;
            }

            if (entry.ExpirationDate != null && entry.ExpirationDate > entryInMem.CollectionDate)
            {
                entryInMem.ExpirationDate = entry.ExpirationDate;
            }


            

            return entryInMem;
        }

        // 4. Delete an entry
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<BloodBankEntry>> Delete(int id)
        {
            var entryInMem = entries.Find(e => e.Id == id);
            if (entryInMem == null)
            {
                return NotFound("Entry not found");
            }

            entries.Remove(entryInMem);
            return NoContent();
        }

        // 5. Search
        [HttpGet("search")]
        public ActionResult<IEnumerable<BloodBankEntry>> Filter(string? bloodType = null, string? status = null, string? donorName = null)
        {

            // First filter: blood type
            List<BloodBankEntry> res = new List<BloodBankEntry>(entries);

            if (bloodType != null)
            {
                res = entries.Where(e => e.DonorBloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Second filter: status
            if (status != null)
            {
                res = res.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Third filter: donor name
            if (donorName != null)
            {
                res = res.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return res;
        }

    }
}
