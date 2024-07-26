using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowRecords()
        {
            return Ok(await _context.BorrowRecords.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowRecord(int id)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);

            if (borrowRecord == null)
            {
                return NotFound();
            }

            return Ok(borrowRecord);
        }

        [HttpPost]
        public async Task<IActionResult> AddBorrowRecord([FromBody] BorrowRecord borrowRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                borrowRecord.BorrowedAt = DateTime.UtcNow;
                _context.BorrowRecords.Add(borrowRecord);
                await _context.SaveChangesAsync();
                return Ok(borrowRecord);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "An error occurred while borrowing the book.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowRecord(int id, [FromBody] BorrowRecord borrowRecord)
        {
            if (id != borrowRecord.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(borrowRecord).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(borrowRecord);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowRecordExists(id))
                {
                    return NotFound();
                }

                return StatusCode(500, new { Message = "An error occurred while updating the borrow record." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowRecord(int id)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);

            if (borrowRecord == null)
            {
                return NotFound();
            }

            try
            {
                _context.BorrowRecords.Remove(borrowRecord);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the borrow record.", Details = ex.Message });
            }
        }

        private bool BorrowRecordExists(int id)
        {
            return _context.BorrowRecords.Any(e => e.Id == id);
        }
    }
}
