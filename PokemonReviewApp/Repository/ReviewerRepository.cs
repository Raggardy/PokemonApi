﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<Review> GetReviewByReviewer(int reviewerId)
        {
             return _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                .Where(r => r.Id == reviewerId) 
                // This includes the navigation property. Right click and peek definition. In this case.the nav property is an ICollection of Reviews
                .Include(e => e.Reviews)
                .FirstOrDefault();  
                 
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers
                .ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        
    }
}
