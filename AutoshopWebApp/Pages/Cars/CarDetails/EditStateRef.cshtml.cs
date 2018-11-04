﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoshopWebApp.Data;
using AutoshopWebApp.Models;

namespace AutoshopWebApp.Pages.Cars.CarDetails
{
    public class EditStateRefModel : PageModel
    {
        private readonly AutoshopWebApp.Data.ApplicationDbContext _context;

        public EditStateRefModel(AutoshopWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CarStateRef CarStateRef { get; set; }

        public MarkAndModel MarkAndModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryData = await
                (from stateRef in _context.CarStateRefId
                where stateRef.CarId == id
                join car in _context.Cars
                on stateRef.CarId equals car.CarId
                join markAndModel in _context.MarkAndModels
                on car.MarkAndModelID equals markAndModel.MarkAndModelId
                select new { stateRef, markAndModel }).FirstOrDefaultAsync();

            if (queryData == null)
            {
                return RedirectToPage("./AddExpertiseRef", new { id = id.Value });
            }

            CarStateRef = queryData.stateRef;
            MarkAndModel = queryData.markAndModel;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CarStateRef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarStateRefExists(CarStateRef.CarStateRefId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarStateRefExists(int id)
        {
            return _context.CarStateRefId.Any(e => e.CarStateRefId == id);
        }
    }
}