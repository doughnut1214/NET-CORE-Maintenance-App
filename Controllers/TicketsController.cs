#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketApplication.Data;
using TicketApplication.Models;

namespace TicketApplication.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        //convert query to ticket object, that way we can set ticketresponses to make it appear green in index 
        public async Task<IActionResult> Index()
        {
            //var tickets = from m in _context.Ticket
            //              select m;
            //tickets = tickets.Where(t => t.OwnerID == User.FindFirstValue(ClaimTypes.NameIdentifier));
            /* 
             The block below handles retrieving the models as an object, retrieving any possible responses, and then passing the ticket to 
             the view. If it has a response, it will be green in the index.
             
             
             */

            IQueryable<Ticket> tickets = from m in _context.Ticket.AsQueryable().Where(m => m.OwnerID == User.Identity.Name)
                                         select m;
            foreach(var ticket in tickets)
            {
                IQueryable<TicketResponse> responses = from m in _context.TicketResponse.AsQueryable().Where(t => t.TicketId == ticket.TicketId)
                                                       select m;
                if(responses.Any())
                {
                    ticket.TicketResponses = responses.ToList();

                }
            }

            /* 
             Handles gathereing annonuncements and tickets, and placing them within a view model object to pass as a model to index 
             
             */
            IQueryable<Announcements> Announcements = from m in _context.Announcements.AsQueryable().OrderByDescending(m => m.CreatedDate)
                                                      select m;
            var announcementAndTicketViewModel = new AnnouncementAndTicketViewModel();
            {
                announcementAndTicketViewModel.Title = "Most Recent Announcement";
                announcementAndTicketViewModel.Announcements = Announcements.ToList();
                announcementAndTicketViewModel.Ticket = tickets.ToList();
            }
            //TODO: fix the exception regarding a difference in Ienumerable<AnnouncementAndTicketViewModel> and the object passed into view
            return View(announcementAndTicketViewModel);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.TicketId == id);

            /*
            var ticketResponse = from m in _context.TicketResponse.AsQueryable().Cast<TicketResponse>()
                                 select m;
            ticketResponse = ticketResponse.Where(t => t.TicketId == ticket.TicketId);
            
            ticket.TicketResponses.Add((TicketResponse)ticketResponse);
            Generates an invalid cast exception, cannot get model TicketResponse this way 
            */

            //This is how you get another model from the controller: query then cast as an object 
            IQueryable<TicketResponse> responses = from m in _context.TicketResponse.AsQueryable().Where(t => t.TicketId == ticket.TicketId) 
                                                   select m;
            
            
            if (responses.Any())
            {
                ticket.TicketResponses = responses.ToList();
            }

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
            


            
           
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,OwnerID,Title,Description,CreatedDate")] Ticket ticket)
        {
            //Forces Owner ID to be equal to the user Identity, preventing changing hidden field through inspect 
            if(ticket.OwnerID != User.Identity?.Name)
            {
                ticket.OwnerID = User.Identity?.Name;

            }


            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,OwnerID,Title,Description,CreatedDate")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            //OwnerId was being set to null for some reason, so this hard codes Owner ID to the ID in the user table
            //*shrug*
            ticket.OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
