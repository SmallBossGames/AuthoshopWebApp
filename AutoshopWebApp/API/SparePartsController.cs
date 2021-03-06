﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoshopWebApp.Models;
using AutoshopWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using AutoshopWebApp.Authorization;

namespace AutoshopWebApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SparePartsController : ControllerBase
    {
        private readonly ISparePartService _sparePartService;
        private readonly IAuthorizationService _authorizationService;

        public SparePartsController(ISparePartService sparePartService, 
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _sparePartService = sparePartService;
        }

        // GET: api/SpareParts
        [HttpGet]
        public async Task<IEnumerable<SparePart>> GetSpareParts([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return await _sparePartService.ReadAllAsync();
            }
            else
            {
                return await _sparePartService.ReadAllAsync(search);
            }
        }

        // GET: api/SpareParts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSparePart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sparePart = await _sparePartService.ReadAsync(id);

            var isAuthorized = await _authorizationService
               .AuthorizeAsync(User, sparePart, Operations.Details);

            if(!isAuthorized.Succeeded)
            {
                return Unauthorized();
            }

            if (sparePart == null)
            {
                return NotFound();
            }

            return Ok(sparePart);
        }

        // PUT: api/SpareParts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSparePart([FromRoute] int id, [FromBody] SparePart sparePart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sparePart.SparePartId)
            {
                return BadRequest();
            }

            var isAuthorized = await _authorizationService
               .AuthorizeAsync(User, sparePart, Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                await _sparePartService.UpdateAsync(sparePart);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _sparePartService.IsExistAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SpareParts
        [HttpPost]
        public async Task<IActionResult> PostSparePart([FromBody] SparePart sparePart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAuthorized = await _authorizationService
               .AuthorizeAsync(User, sparePart, Operations.Create);

            if (!isAuthorized.Succeeded)
            {
                return Unauthorized();
            }

            await _sparePartService.CreateAsync(sparePart);

            return CreatedAtAction("GetSparePart", new { id = sparePart.SparePartId }, sparePart);
        }

        // DELETE: api/SpareParts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSparePart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAuthorized = await _authorizationService
               .AuthorizeAsync(User, new SparePart() { SparePartId = id }, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Unauthorized();
            }

            var sparePart = await _sparePartService.DeleteAsync(id);

            if (sparePart == null)
            {
                return NotFound();
            }

            return Ok(sparePart);
        }
    }
}