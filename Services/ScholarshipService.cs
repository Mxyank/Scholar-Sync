using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Plaatform_Backend.Data;
using Scholarship_Plaatform_Backend.Exceptions;
using Scholarship_Plaatform_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scholarship_Plaatform_Backend.Services
{
    public class ScholarshipService
    {
        private readonly ApplicationDbContext _context;

        public ScholarshipService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Get all scholarships
        public async Task<IEnumerable<Scholarship>> GetAllScholarships()
        {
            return await _context.Scholarships.ToListAsync();
        }

        // 2. Get scholarship by ID
        public async Task<Scholarship> GetScholarshipById(int scholarshipId)
        {
            var scholarship = await _context.Scholarships.FindAsync(scholarshipId);
            return scholarship;
        }

        // 3. Add a new scholarship
        public async Task<bool> AddScholarship(Scholarship scholarship)
        {
            // Check if a scholarship with the same name exists
            bool exists = await _context.Scholarships.AnyAsync(s => s.Name == scholarship.Name);
            if (exists)
            {
                throw new ScholarshipException("Scholarship with the same name already exists");
            }
            _context.Scholarships.Add(scholarship);
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Update an existing scholarship
        public async Task<bool> UpdateScholarship(int scholarshipId, Scholarship scholarship)
        {
            var existingScholarship = await _context.Scholarships.FindAsync(scholarshipId);
            if (existingScholarship == null)
            {
                return false;
            }
            // Check if another scholarship with the same name exists
            bool exists = await _context.Scholarships.AnyAsync(s => s.Name == scholarship.Name && s.ScholarshipId != scholarshipId);
            if (exists)
            {
                throw new ScholarshipException("Scholarship with the same name already exists");
            }
            // Update scholarship details
            existingScholarship.Name = scholarship.Name;
            existingScholarship.Description = scholarship.Description;
            existingScholarship.EligibilityCriteria = scholarship.EligibilityCriteria;
            existingScholarship.Amount = scholarship.Amount;
            existingScholarship.Deadline = scholarship.Deadline;
            existingScholarship.Category = scholarship.Category;
            existingScholarship.NumberOfAwards = scholarship.NumberOfAwards;
            existingScholarship.Sponsor = scholarship.Sponsor;

            await _context.SaveChangesAsync();
            return true;
        }

        // 5. Delete a scholarship
        public async Task<bool> DeleteScholarship(int scholarshipId)
        {
            var scholarship = await _context.Scholarships.FindAsync(scholarshipId);
            if (scholarship == null)
            {
                return false;
            }
            // Check if the scholarship is referenced in ScholarshipApplication
            bool isReferenced = await _context.ScholarshipApplications.AnyAsync(sa => sa.ScholarshipId == scholarshipId);
            if (isReferenced)
            {
                throw new ScholarshipException("Scholarship cannot be deleted, it is referenced in ScholarshipApplication");
            }
            _context.Scholarships.Remove(scholarship);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
