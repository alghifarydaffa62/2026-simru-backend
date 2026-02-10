using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimruBackend.Data;
using SimruBackend.Models;

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
        public async Task<ActionResult<IEnumerable<Reservation>>> GetActiveReservations()
        {
            var today = DateTime.Today;

            return await _context.Reservations
                .Include(res => res.Room) 
                .Where(res => !res.IsDeleted && res.BorrowDate.Date >= today) 
                .OrderByDescending(res => res.BorrowDate) 
                .ToListAsync();
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
        public async Task<ActionResult<IEnumerable<Reservation>>> GetHistoryReservations() 
        {
            var today = DateTime.Today;

            return await _context.Reservations
                .Include(res => res.Room)
                .Where(res => res.IsDeleted || res.BorrowDate.Date < today)
                .OrderByDescending(res => res.BorrowDate)
                .ToListAsync();
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id) return BadRequest();

            var existingReservation = await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingReservation == null) return NotFound();

            if (reservation.Status == ReservationStatus.Approved)
            {
                var isConflict = await _context.Reservations.AnyAsync(res =>
                    res.RoomId == reservation.RoomId &&
                    res.BorrowDate.Date == reservation.BorrowDate.Date &&
                    res.Status == ReservationStatus.Approved &&
                    res.Id != id && 
                    !res.IsDeleted);

                if (isConflict) 
                {
                    return BadRequest("Gagal menyimpan. Ruangan tersebut sudah memiliki jadwal Approved lain di tanggal yang dipilih.");
                }
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Pending; 
    
            reservation.IsDeleted = false;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var result = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == reservation.Id);

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, result);
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
