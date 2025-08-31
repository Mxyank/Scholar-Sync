using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scholarship_Plaatform_Backend.Models
{
    public class ScholarshipApplication
    {
        [Key]
        public int ScholarshipApplicationId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int ScholarshipId { get; set; }

        [ForeignKey("ScholarshipId")]
        public Scholarship? Scholarship { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ApplicationDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string Essay { get; set; }
        public string? Remarks { get; set; }
        public string? SupportingDocuments { get; set; }
    }
}
