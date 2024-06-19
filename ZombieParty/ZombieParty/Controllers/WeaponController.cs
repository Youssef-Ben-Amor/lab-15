using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.Utility;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class WeaponController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public WeaponController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public async Task<IActionResult> Index()
        {
            List<Weapon> weapons = await _baseDonnees.Weapons.ToListAsync();
            return View(weapons);
        }

        //GET
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
                // create
                return View(new Weapon());
            else
                //update
                return View(await _baseDonnees.Weapons.FindAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                // Create
                if (weapon.WeaponId == 0)
                {
                    // Ajouter à la BD
                    await _baseDonnees.Weapons.AddAsync(weapon);
                    TempData[AppConstants.Success] = $"{weapon.Name} weapon added";
                }
                else
                {
                    // Update
                    _baseDonnees.Weapons.Update(weapon);
                    TempData[AppConstants.Success] = $"{weapon.Name} weapon updated";
                }
                await _baseDonnees.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            return this.View(weapon);
        }
    }
}
