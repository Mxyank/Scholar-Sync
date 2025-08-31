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
    public class ScholarshipApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ScholarshipApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /* Retrieves all scholarship applications from the database. */
        public async Task<IEnumerable<ScholarshipApplication>> GetAllScholarshipApplications()
        {
            return await _context.ScholarshipApplications.Include(i => i.User).Include(i => i.Scholarship).ToListAsync();
        }

        /* Retrieves a scholarship application by user ID. */
        public async Task<IEnumerable<ScholarshipApplication>> GetScholarshipApplicationsByUserId(int userId)
        {
            var scholarships = await _context.ScholarshipApplications
                                     .Where(sa => sa.UserId == userId)
                                     .ToListAsync();
            return scholarships;
        }

        /* Adds a new scholarship application to the database. */
        public async Task<bool> AddScholarshipApplication(ScholarshipApplication scholarshipApplication)
        {
            // Checks if the user has already applied for this scholarship
            bool exists = await _context.ScholarshipApplications.AnyAsync(sa =>
                sa.UserId == scholarshipApplication.UserId &&
                sa.ScholarshipId == scholarshipApplication.ScholarshipId);

            if (exists)
                throw new ScholarshipException("User already applied for this scholarship");

            await _context.ScholarshipApplications.AddAsync(scholarshipApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        /* Updates an existing scholarship application in the database. */
        public async Task<bool> UpdateScholarshipApplication(int scholarshipApplicationId, ScholarshipApplication scholarshipApplication)
        {
            var existingApplication = await _context.ScholarshipApplications.FindAsync(scholarshipApplicationId);

            if (existingApplication == null)
                return false;

            // Update all fields
            existingApplication.UserId = scholarshipApplication.UserId;
            existingApplication.ScholarshipId = scholarshipApplication.ScholarshipId;
            existingApplication.ApplicationDate = scholarshipApplication.ApplicationDate;
            existingApplication.ApplicationStatus = scholarshipApplication.ApplicationStatus;
            existingApplication.Remarks = scholarshipApplication.Remarks;
            existingApplication.Essay = scholarshipApplication.Essay; // Ensure all fields are updated
            existingApplication.SupportingDocuments = scholarshipApplication.SupportingDocuments;

            // _context.ScholarshipApplications.Update(existingApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        /* Deletes a scholarship application from the database. */
        public async Task<bool> DeleteScholarshipApplication(int scholarshipApplicationId)
        {
            var application = await _context.ScholarshipApplications.FindAsync(scholarshipApplicationId);
            if (application == null)
                return false;

            _context.ScholarshipApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
        public int GetApplicationCountForScholarship(int scholarshipId)
        {
            return _context.ScholarshipApplications.Count(sa => sa.ScholarshipId == scholarshipId);
        }
    }
}
