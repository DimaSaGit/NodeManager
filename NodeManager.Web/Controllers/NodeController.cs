﻿using NodeManager.Domain;
using Microsoft.AspNetCore.Mvc;
using NodeManager.Web.Abstract;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NodeManager.Web.Models;
using System.Drawing;
using System.IO;
using NodeManager.Web.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NodeManager.Web.Controllers
{
    [Route("")]
    [Route("Node")]

    public class NodeController : Controller
    {
        private INodes repos;
        //private readonly NodeManagerDBEntities dbContext;
        //public int pageSize = 4;

        public NodeController(INodes repo)
        {
            repos = repo;
        }

        [Route("")]
        //[Route("List")]
        //[Route("List/{section:string}")]
        //[Authorize(Policy = "OnlyForInpad")]
        [Route("List/{section?}/{category?}")]
        public async Task<ViewResult> List(string section, string category)
        {

            //using(NodeManagerDBEntities r = new NodeManagerDBEntities())
            if (!repos.Categories.Any(x => x.Name == category))
            {
                category = (string)null;
            }
            if (!repos.Sections.Any(x => x.Name == section))
            {
                section = (string)null;
            }
            Categories cat = repos.Categories.FirstOrDefault(x => x.Name == category);
            Sections sec = repos.Sections.FirstOrDefault(x => x.Name == section);
            NodesViewModel model = new NodesViewModel()
            {
                Symbols = repos.FamilySymbols
                    .Where(x => (category == null || x.CategoryId == cat.Id) && (section == null || x.SectionId == sec.Id))
                    .OrderBy(x => x.Id).ToList(),
                CurrentSec = sec
            };
            model.categorySection = GetCategorySection();

            if (sec == null)
            {
                model.categorySection.SelectedSection = null;
            }
            else
            {
                model.categorySection.SelectedSection = sec.Id;
            }
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();
            return View(model);
        }

        [Route("Symbol/{id:int}")]
        public ViewResult FamSymbol(int id)
        {
            FamSymbolViewModel model = new FamSymbolViewModel()
            {
                _familySymbol = repos.FamilySymbols.FirstOrDefault(x => x.Id == id),
                _revitParameters = repos.RevParameters
                    .Where(c => c.SymbolId == id)
                    .OrderBy(c => c.Id)
            };
            model.UserName = HttpContext.User.Identity.Name;
            model.IsLogin = HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(string[] tags)
        {
            NodesViewModel model = new NodesViewModel();
            //var splittedTags = tags.Split(';');
            HashSet<int> tagsId = new HashSet<int>();
            IEnumerable<int> connections;
            try
            {
                foreach (var tag in tags)
                {
                    tagsId.Add(repos.Tags.FirstOrDefault(x => x.Value.Equals(tag.ToLower())).Id);
                }
                connections = repos.FSTags.Where(x => x.TagId == tagsId.First()).Select(x => x.FSId);

                foreach (var tag in tagsId.Skip(1))
                {
                    connections = connections.Intersect(repos.FSTags.Where(x => x.TagId == tag).Select(x => x.FSId));
                }

                var symbols = repos.FamilySymbols.ToList();
                foreach (var fs in connections)
                {
                    model.Symbols.Add(symbols.FirstOrDefault(x => x.Id == fs));
                }
            }
            catch (Exception ex)
            {
                model.Symbols = repos.FamilySymbols.ToList();
                model.IsTagSearchEmpty = true;
            }

            model.categorySection = GetCategorySection();
            model.categorySection.SelectedSection = null;
            model.tagList = repos.Tags.Select(x => x.Value).ToList();

            return View("List", model);
        }

        [Authorize]
        [Route("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var fs = repos.FamilySymbols.FirstOrDefault(x => x.Id == id);
            repos.dbContext.Remove(fs);
            repos.dbContext.SaveChanges();
            return RedirectToAction("List", "Node");
        }

        [Route("DownloadFile/{id:int}")]
        //public FileResult DownloadFile(int id)
        //{
        //    //// Путь к файлу
        //    ////string file_path="";
        //    ////string file_path = Server.MapPath(repos.FamilySymbols.First(x => x.Id == id).ImagePath);
        //    //string file_path = Server.MapPath("https://mobimg.b-cdn.net/v3/fetch/4d/4df647582c491a06b151a81959eebd73.jpeg");
        //    //// Тип файла - content-type
        //    //string file_type = "application/pdf";
        //    //// Имя файла - необязательно
        //    //string file_name = "PDFIcon.pdf";
        //    //return File(file_path, file_type, file_name);
        //}

        // Отправка потока
        //public FileResult GetStream()
        //{
        //    //System.Web.HttpContext.Current.Server.MapPath(path);
        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("https://mobimg.b-cdn.net/v3/fetch/4d/4df647582c491a06b151a81959eebd73.jpeg");
        //    // Объект Stream
        //    FileStream fs = new FileStream(path, FileMode.Open);
        //    string file_type = "application/pdf";
        //    string file_name = "PDFIcon.pdf";
        //    return File(fs, file_type, file_name);
        //}

        private CategorySection GetCategorySection()
        {
            CategorySection categorySection = new CategorySection();
            if (repos.FamilySymbols.Count() != 0)
            {
                var list = repos.FamilySymbols.ToList();
                foreach (var item in repos.Sections)
                {
                    categorySection.Menu.Add(item, list
                        .Where(x => x.Section == item)
                        .Select(symb => new Categories() { Id = symb.CategoryId.Value, Name = repos.Categories.FirstOrDefault(x => x.Id == symb.CategoryId).Name })
                        .GroupBy(p => p.Id)
                        .Select(g => g.First())
                        .OrderBy(x => x.Id));
                }
            }
            return categorySection;
        }
    }
}