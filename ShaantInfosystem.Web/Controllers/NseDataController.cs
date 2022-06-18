using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShaantInfosystem.Web.Data;
using ShaantInfosystem.Web.Models;
using ShaantInfosystems.Web.Models;

namespace ShaantInfosystem.Web.Controllers
{
    public class NseDataController : Controller
    {
        private readonly ShaantInfosystemWebContext _context;

        public NseDataController(ShaantInfosystemWebContext context)
        {
            _context = context;
            DbInitializer.Initialize(_context);
        }

        // GET: NseData
        public async Task<IActionResult> Index()
        {
              return _context.NseModel != null ? 
                          View(await _context.NseModel.ToListAsync()) :
                          Problem("Entity set 'ShaantInfosystemWebContext.NseModel'  is null.");
        }

        public async Task<IActionResult> DailyHighestReturns()
        {
            var dailyHighestReturnViewModels = await GetHighestReturnModel();

            return View(dailyHighestReturnViewModels);
        }


        public async Task<IActionResult> MaxHighestReturnsCount()
        {
            List<HighestReturnCount> highestReturnCounts = new List<HighestReturnCount>();
            var dailyHighestReturnViewModels = await GetHighestReturnModel();
            highestReturnCounts = dailyHighestReturnViewModels.GroupBy(a => a.IndexType)
                .Select(a => new HighestReturnCount() 
                {
                    Count  = a.Count(),
                    IndexType = a.Key
                }).ToList();
            return View(highestReturnCounts);
        }

        // GET: NseData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NseModel == null)
            {
                return NotFound();
            }

            var nseModel = await _context.NseModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nseModel == null)
            {
                return NotFound();
            }

            return View(nseModel);
        }

        // GET: NseData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NseData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Open,High,Low,Close,SharesTraded,TurnOverInRsCore")] NseModel nseModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nseModel);
        }

        // GET: NseData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NseModel == null)
            {
                return NotFound();
            }

            var nseModel = await _context.NseModel.FindAsync(id);
            if (nseModel == null)
            {
                return NotFound();
            }
            return View(nseModel);
        }

        // POST: NseData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Open,High,Low,Close,SharesTraded,TurnOverInRsCore")] NseModel nseModel)
        {
            if (id != nseModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NseModelExists(nseModel.Id))
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
            return View(nseModel);
        }

        // GET: NseData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NseModel == null)
            {
                return NotFound();
            }

            var nseModel = await _context.NseModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nseModel == null)
            {
                return NotFound();
            }

            return View(nseModel);
        }

        // POST: NseData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NseModel == null)
            {
                return Problem("Entity set 'ShaantInfosystemWebContext.NseModel'  is null.");
            }
            var nseModel = await _context.NseModel.FindAsync(id);
            if (nseModel != null)
            {
                _context.NseModel.Remove(nseModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NseModelExists(int id)
        {
          return (_context.NseModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<List<DailyHighestReturnViewModel>> GetHighestReturnModel()
        {

            var nseModelList = (await _context.NseModel.ToListAsync());
            var nifty50 = nseModelList.Where(a => a.DataType == 1).OrderBy(a => a.Date).ToList();
            var nifty100 = nseModelList.Where(a => a.DataType == 2).OrderBy(a => a.Date).ToList();
            var nifty200 = nseModelList.Where(a => a.DataType == 3).OrderBy(a => a.Date).ToList();
            var nifty500 = nseModelList.Where(a => a.DataType == 4).OrderBy(a => a.Date).ToList();
            List<DailyHighestReturnViewModel> dailyHighestReturnViewModels = new List<DailyHighestReturnViewModel>();
            List<ReturnPercentageOnEachDay> returnPercentageOnEachDaysForNifty50 = new List<ReturnPercentageOnEachDay>();
            List<ReturnPercentageOnEachDay> returnPercentageOnEachDaysForNifty100 = new List<ReturnPercentageOnEachDay>();
            List<ReturnPercentageOnEachDay> returnPercentageOnEachDaysForNifty200 = new List<ReturnPercentageOnEachDay>();
            List<ReturnPercentageOnEachDay> returnPercentageOnEachDaysForNifty500 = new List<ReturnPercentageOnEachDay>();
            for (int i = 0; i < nifty50.Count(); i++)
            {
                if (i != 0)
                {
                    returnPercentageOnEachDaysForNifty50.Add(new ReturnPercentageOnEachDay()
                    {
                        IndexType = 1,
                        Date = nifty50[i].Date,
                        ReturnPercentage = ((nifty50[i].Close - nifty50[i - 1].Close) / nifty50[i - 1].Close) * 100
                    });
                }
            }

            for (int i = 0; i < nifty100.Count(); i++)
            {
                if (i != 0)
                {
                    returnPercentageOnEachDaysForNifty100.Add(new ReturnPercentageOnEachDay()
                    {
                        IndexType = 2,
                        Date = nifty100[i].Date,
                        ReturnPercentage = ((nifty100[i].Close - nifty100[i - 1].Close) / nifty100[i - 1].Close) * 100
                    });
                }
            }

            for (int i = 0; i < nifty200.Count(); i++)
            {
                if (i != 0)
                {
                    returnPercentageOnEachDaysForNifty200.Add(new ReturnPercentageOnEachDay()
                    {
                        IndexType = 3,
                        Date = nifty200[i].Date,
                        ReturnPercentage = ((nifty200[i].Close - nifty200[i - 1].Close) / nifty200[i - 1].Close) * 100
                    });
                }
            }

            for (int i = 0; i < nifty500.Count(); i++)
            {
                if (i != 0)
                {
                    returnPercentageOnEachDaysForNifty500.Add(new ReturnPercentageOnEachDay()
                    {
                        IndexType = 4,
                        Date = nifty500[i].Date,
                        ReturnPercentage = ((nifty500[i].Close - nifty500[i - 1].Close) / nifty500[i - 1].Close) * 100
                    });
                }
            }

            dailyHighestReturnViewModels = returnPercentageOnEachDaysForNifty50
                .Join(returnPercentageOnEachDaysForNifty100, a => a.Date, b => b.Date, (a, b) => new { a, b })
                .Join(returnPercentageOnEachDaysForNifty200, i => i.a.Date, j => j.Date, (ab, c) => new { a = ab.a, b = ab.b, c = c })
                .Join(returnPercentageOnEachDaysForNifty500, i => i.a.Date, j => j.Date, (abc, d) => new { a = abc.a, b = abc.b, c = abc.c, d = d })
                .Select(x => new DailyHighestReturnViewModel()
                {
                    CurrentDate = x.a.Date,
                    IndexType = x.a.ReturnPercentage > x.b.ReturnPercentage && x.a.ReturnPercentage > x.c.ReturnPercentage && x.a.ReturnPercentage > x.d.ReturnPercentage ?
                    x.a.IndexType : (x.b.ReturnPercentage > x.c.ReturnPercentage && x.b.ReturnPercentage > x.d.ReturnPercentage ? x.b.IndexType : (x.c.ReturnPercentage > x.d.ReturnPercentage ? x.c.IndexType : x.d.IndexType)),
                    Percentage = x.a.ReturnPercentage > x.b.ReturnPercentage && x.a.ReturnPercentage > x.c.ReturnPercentage && x.a.ReturnPercentage > x.d.ReturnPercentage ?
                    x.a.ReturnPercentage : (x.b.ReturnPercentage > x.c.ReturnPercentage && x.b.ReturnPercentage > x.d.ReturnPercentage ? x.b.ReturnPercentage : (x.c.ReturnPercentage > x.d.ReturnPercentage ? x.c.ReturnPercentage : x.d.ReturnPercentage))
                }).ToList();
            return dailyHighestReturnViewModels;
        }
    }
}
