using RestaurantInfo.Data;
using RestaurantInfo.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantInfo.Web.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantData _restaurantData;

        public RestaurantController(IRestaurantData restaurantData)
        {
            _restaurantData = restaurantData;
        }

        public ActionResult Index()
        {
            var restaurants = _restaurantData.GetAll();
            return View(restaurants);
        }
        public ActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);

            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _restaurantData.Add(restaurant);
                TempData["Message"] = "You have created the restaurant!";
                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            return View(restaurant);
        }

        public ActionResult Edit(int id)
        {
            var model = _restaurantData.Get(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _restaurantData.Update(restaurant);
                TempData["Message"] = "You have saved the restaurant!";
                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            return View(restaurant);
        }

        public ActionResult Delete(int id)
        {
            var model = _restaurantData.Get(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            _restaurantData.Delete(id);
            return RedirectToAction("Index");
        }

    }

}
