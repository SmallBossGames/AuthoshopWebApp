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
using AutoshopWebApp.Models.ForShow;

namespace AutoshopWebApp.Pages.Workers.WorkerDetails
{
    public class EditModel : PageModel, IWorkerPage
    {
        private readonly AutoshopWebApp.Data.ApplicationDbContext _context;

        public EditModel(AutoshopWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public OutputWorkerModel OutputModel { get; set; }

        public IWorkerCrossPageData WorkerCrossPageData => OutputModel;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OutputModel = await OutputWorkerModel
                .GetQuery(_context)
                .FirstOrDefaultAsync(item => item.WorkerID == id);

            if (OutputModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var street = await _context.Streets
                .FirstOrDefaultAsync(item =>
                item.StreetName.Equals(OutputModel.Street.StreetName, StringComparison.OrdinalIgnoreCase));

            if(street == null)
            {
                street = OutputModel.Street;
                await _context.AddAsync(street);
                await _context.SaveChangesAsync();
            }

            OutputModel.Worker.StreetId = street.StreetId;

            _context.Attach(OutputModel.Worker).State = EntityState.Modified;
            _context.Entry(OutputModel.Worker).Property(x => x.PositionId).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(OutputModel.WorkerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id = OutputModel.WorkerID });
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.WorkerId == id);
        }
    }
}