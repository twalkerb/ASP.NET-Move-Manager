﻿using Microsoft.AspNetCore.Mvc;
using MovieManager.Models;
using MovieManager.Services;
using System.Diagnostics;
using System.Text;

namespace MovieManager.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }


        //[Authorize]
        public IActionResult Main()
        {
            Console.WriteLine("Hit controller: Movie , hit view: Main");
            //add logic for getting movie for each column for each user
            //this requires Authentication to be fully working, 

            //var watchedMovies = GetUserMovieList(this.User.Id, "watched")
            //var currentMovies = GetUserMovieList(this.User.Id, "current")
            //var futureMovies = GetUserMovieList(this.User.Id, "future")

            return View();
        }


        //[Authorize]
        public IActionResult MovieList()
        {
            Console.WriteLine("Hit controller: Movie , hit view: MovieList");
            //takes a PlaylistId and returns a PlaylistViewModel (List<Movie>) with all movies in it to be displayed
            //can edit the playlist?

            return View();
        }



        [Route("Movie/MovieCard/{id}")]
        public IActionResult MovieCard(int id) //takes moviecard viewmodel
        {
            Console.WriteLine($"Hit controller: Movie , hit view: MovieCard, ID = {id}");

            var movieIdResult = SearchMethods.SearchApiWithID(id);
            //takes a movie ID and returns a Movie Db Model

            var model = new MovieCardViewModel()
            {
                Movie = movieIdResult,
            };

            return View(model);
        }



        //Initial Search Get Page
        public IActionResult Search()
        {
            Console.WriteLine("Hit controller: Movie , hit view: Search WITHOUT PARAM");

            return View();
        }

        //Search Post Request on Search Bar Submit
        [HttpPost]
        public IActionResult Search(SearchResultViewModel model)
        {
            Console.WriteLine("Hit controller: Movie , hit view: Search WITH TITLE PARAM");

            var movieResults = SearchMethods.SearchMovieTitleToList(model.SearchTerm);
            var showResults = SearchMethods.SearchShowTitleToList(model.SearchTerm);

            var results = new SearchResultViewModel()
            {
                ResultMovieList = movieResults,
                ResultShowList = showResults,
                SearchTerm = model.SearchTerm
            };

            Console.WriteLine($"Searching for {model.SearchTerm}");

            var viewWithViewModel = SearchResult(results);

            return View("SearchResult", results);
        }

        //Search Results Page - gets the View with The SearchResultViewModel (Show and Movie Results Lists)
        public IActionResult SearchResult(SearchResultViewModel results)
        {
            Console.WriteLine("Hit controller: Movie , hit view: SearchResult");

            Console.WriteLine($"search term - {results.SearchTerm}");

            return View();
        }



        [Route("Movie/ActorCard/{id}")]
        public IActionResult ActorCard(int id)
        {
            var actor = SearchMethods.GetActorWithID(id);
            StringBuilder sb = new StringBuilder();
            //foreach (var item in actor.MovieCredits.Cast)
            //{
            //    sb.Append(item.Title);
            //}

            var model = new ActorViewModel()
            {
                ActorId = actor.Id,
                FullName = actor.Name,
                Description = actor.Biography,
                CountryCode = actor.PlaceOfBirth,
                Images = SaveMovieToDbObject.BuildImageURL(actor.ProfilePath),
            //    MovieCredits = sb.ToString(),
            };

            return View(model);
        }

        //TODO, hardcoded basics work
        public IActionResult Discover() => View();

        public IActionResult Releases() => View();

        public IActionResult Review() => View();


       



        //This is default from ASP.NET, not sure if its needed
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}