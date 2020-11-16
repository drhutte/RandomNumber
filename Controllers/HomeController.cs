using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Elmah;
using RandomNumber.Helpers;
using RandomNumber.Models;

namespace RandomNumber.Controllers
{
    public class HomeController : Controller
    {
        //  Static list of classes derived from Base (NumberGenerator) Class
        //  Used to:
        //  1 populate list of buttons to select level of difficulty for number game
        //  2 Cast Base class to derived class selected by user
        private static IEnumerable<NumberGenerator> _derived;

        //  Constructor to populate list of classes derived from NumberGenerator class via reflection
        public HomeController()
        {
            if (_derived == null)
            {
                try
                {
                    _derived = GetDerived();
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        //  Derived list passed to index view and then to htmlHome helper
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View("Index", _derived.ToList());
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Redirect("~/Error");
        }

        //  Cast Base class to derived class selected by user
        //  Use Range property of derived class selected to set max value for random number
        //  Derived class passed to HtmlGuessHelper.Form to create info and input box for user
        [HttpGet]
        public MvcHtmlString NewGame(int Level)
        {
            try
            {
                Random r = new Random();
                NumberGenerator randomNumber = _derived.Where(l => l.Level == Level).FirstOrDefault();
                randomNumber.Random = r.Next(1, randomNumber.Range + 1);
                return HtmlGuessHelper.Form(randomNumber);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return new MvcHtmlString(Redirect("~/Error").ToString());
        }

        //  User guess passed back with other params required to establish other project requirements
        //  Base model populated, cast to derived class and validataed against derived class modelstate
        //  passed to HtmlGuessHelper.Guess so as to report succees/failure etc to user
        [HttpPost]
        public MvcHtmlString Guess(int Level, int Random, int Range, int Guesses, int Guess)
        {
            try
            {
                Guesses = Guesses -1;
                NumberGenerator random = _derived.Where(l => l.Level == Level).FirstOrDefault();
                random.Level = Level;
                random.Random = Random;
                random.Range = Range;
                random.Guesses = Guesses;
                random.Guess = Guess;

                List<string> listErrors = TestModelState(random);

                return HtmlGuessHelper.Guess(random, listErrors);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return new MvcHtmlString(Redirect("~/Error").ToString());
        }
        // Validate derived class validated against derived class modelstate and collect errors to show on user view
        private List<string> TestModelState(NumberGenerator random)
        {
            List<string> modelErrors = null;
            try
            {
                ModelState.Clear();
                TryValidateModel(random);
                if (!ModelState.IsValid)
                {
                    modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return modelErrors;
        }

        // List of derived classes generated via reflected
        private static IEnumerable<NumberGenerator> GetDerived()
        {
            IEnumerable<NumberGenerator> derived = null;
            try
            {
                derived = typeof(NumberGenerator)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(NumberGenerator)) && !t.IsAbstract)
                .Select(t => (NumberGenerator)Activator.CreateInstance(t));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return derived;
        }

        // For testing only
        private static void CreateError()
        {
            int a = 1;
            int b = 0;
            int c = 0;
            c = a / b; //it would cause exception.  
        }
    }
}
