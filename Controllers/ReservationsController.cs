using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimruBackend.Data;
using SimruBackend.Models;
using SimruBackend.DTO;

namespace SimruBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetActiveReservations([FromQuery] string? search)
        {
            var today = DateTime.Today;

            IQueryable<Reservation> query = _context.Reservations
                .Include(res => res.Room)
                .Where(res => !res.IsDeleted && res.BorrowDate.Date >= today);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(res =>
                    res.BorrowerName.Contains(search) ||
                    (res.Room != null && res.Room.Name.Contains(search))); 
            }

            var result = await query
                .OrderBy(res => res.BorrowDate)
                .Select(res => new ReservationResponseDTO {
                    Id = res.Id,
                    RoomId = res.RoomId,
                    RoomName = res.Room != null ? res.Room.Name : "N/A",
                    BorrowerName = res.BorrowerName,
                    BorrowDate = res.BorrowDate,
                    Purpose = res.Purpose,
                    Status = (int)res.Status
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                    .Include(res => res.Room)
                    .FirstOrDefaultAsync(res => res.Id == id && !res.IsDeleted);

            if (reservation == null) return NotFound();

            return reservation;
        }

        // GET: api/Reservations/history
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetHistoryReservations() 
        {
            var today = DateTime.Today;

            return await _context.Reservations
                .Include(res => res.Room)
                .Where(res => res.IsDeleted || res.BorrowDate.Date < today)
                .OrderByDescending(res => res.BorrowDate)
                .Select(res => new ReservationResponseDTO {
                    Id = res.Id,
                    RoomId = res.RoomId,
                    RoomName = res.Room != null ? res.Room.Name : "N/A",
                    BorrowerName = res.BorrowerName,
                    BorrowDate = res.BorrowDate,
                    Purpose = res.Purpose,
                    Status = (int)res.Status
                })
                .ToListAsync();
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, ReservationRequestDTO dto)
        {
            var existingRes = await _context.Reservations.FindAsync(id);
            if (existingRes == null) return NotFound();

            if (dto.Status == (int)ReservationStatus.Approved)
            {
                var isConflict = await _context.Reservations.AnyAsync(res =>
                    res.RoomId == dto.RoomId &&
                    res.BorrowDate.Date == dto.BorrowDate.Date &&
                    res.Status == ReservationStatus.Approved &&
                    res.Id != id && 
                    !res.IsDeleted);

                if (isConflict) return BadRequest("Ruangan sudah di-book oleh orang lain.");
            }

            existingRes.RoomId = dto.RoomId;
            existingRes.BorrowerName = dto.BorrowerName;
            existingRes.BorrowDate = dto.BorrowDate;
            existingRes.Purpose = dto.Purpose;
            existingRes.Status = (ReservationStatus)dto.Status;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ReservationExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservationResponseDTO>> PostReservation(ReservationRequestDTO dto)
        {
            var reservation = new Reservation {
                RoomId = dto.RoomId,
                BorrowerName = dto.BorrowerName,
                BorrowDate = dto.BorrowDate,
                Purpose = dto.Purpose,
                Status = ReservationStatus.Pending, 
                IsDeleted = false 
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var createdRes = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == reservation.Id);

            var response = new ReservationResponseDTO {
                Id = createdRes!.Id,
                RoomId = createdRes.RoomId,
                RoomName = createdRes.Room?.Name ?? "N/A",
                BorrowerName = createdRes.BorrowerName,
                BorrowDate = createdRes.BorrowDate,
                Purpose = createdRes.Purpose,
                Status = (int)createdRes.Status
            };

            return CreatedAtAction("GetReservation", new { id = response.Id }, response);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            reservation.IsDeleted = true;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}