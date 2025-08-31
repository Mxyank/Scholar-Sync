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
    public class FeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            // _context.Feedbacks.
            return await _context.Feedbacks
                .Include(f => f.User)
                .ToListAsync();
        }

        /* method to get Feedback of a particular user with userId */
        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserId(int userId)
        {

            return await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        /* method to add a Feedback */
        public async Task<bool> AddFeedback(Feedback feedback)
        {
            feedback.User = null;
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteFeedback(int FeedbackId)
        {
            var exs = await _context.Feedbacks.FindAsync(FeedbackId);
            if (exs == null)
            {
                return false;
            }
            _context.Feedbacks.Remove(exs);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
