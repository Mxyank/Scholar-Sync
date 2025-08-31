using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scholarship_Plaatform_Backend.Models
{
    public class Scholarship
    {
        [Key]
        public int ScholarshipId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EligibilityCriteria { get; set; }
        public decimal Amount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
        public int NumberOfAwards { get; set; }
        public string Sponsor { get; set; }

        // Navigational Property
        [JsonIgnore]
        public ICollection<ScholarshipApplication>? ScholarshipApplications { get; set; }
    }
}
