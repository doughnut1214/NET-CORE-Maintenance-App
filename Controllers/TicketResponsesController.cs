#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketApplication.Data;
using TicketApplication.Models;

namespace TicketApplication.Controllers
{
    public class TicketResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketResponsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketResponses
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketResponse.ToListAsync());
        }

        // GET: TicketResponses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketResponse = await _context.TicketResponse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketResponse == null)
            {
                return NotFound();
            }

            return View(ticketResponse);
        }

        // GET: TicketResponses/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Ticket, "TicketId", "TicketId");
            return View();
        }

        // POST: TicketResponses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedDate,TicketId")] TicketResponse ticketResponse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketResponse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TicketResponses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketResponse = await _context.TicketResponse.FindAsync(id);
            if (ticketResponse == null)
            {
                return NotFound();
            }
            return View(ticketResponse);
        }

        // POST: TicketResponses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedDate,TicketId")] TicketResponse ticketResponse)
        {
            if (id != ticketResponse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketResponse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketResponseExists(ticketResponse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticketResponse);
        }

        // GET: TicketResponses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketResponse = await _context.TicketResponse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketResponse == null)
            {
                return NotFound();
            }

            return View(ticketResponse);
        }
        public async Task<IActionResult> Tickets()
        {

            IQueryable<Ticket> tickets = from m in _context.Ticket.AsQueryable()
                                         select m;
            foreach (var ticket in tickets)
            {
                IQueryable<TicketResponse> responses = from m in _context.TicketResponse.AsQueryable().Where(t => t.TicketId == ticket.TicketId)
                                                       select m;
                if (responses.Any())
                {
                    ticket.TicketResponses = responses.ToList();

                }
            }


            return View(tickets);
        }

        // POST: TicketResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketResponse = await _context.TicketResponse.FindAsync(id);
            _context.TicketResponse.Remove(ticketResponse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketResponseExists(int id)
        {
            return _context.TicketResponse.Any(e => e.Id == id);
        }
    }
}
