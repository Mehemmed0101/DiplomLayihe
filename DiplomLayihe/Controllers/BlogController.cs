using DiplomLayihe.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomLayihe.Models.Entities;

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        readonly DiplomDbContext db;
        public BlogController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var data = db.BlogPosts
               .Where(bp => bp.DeletedById == null)
               .ToList();
            return View(data);
        }

        public IActionResult Details(int id)
        {
            var post = db.BlogPosts
                .Include(bp => bp.TagCloud)
                .ThenInclude(bp => bp.PostTag)
                .FirstOrDefault(bp => bp.DeletedById == null && bp.Id == id);


            var comments = db.BlogPostComments.Where(bpc => bpc.DeletedById == null && bpc.ParentId == id).ToList();
            ViewBag.Comments = comments;

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> CreatingComment(int id, string name, string email, string message, int replyid)
        {
            if (replyid == 0)
            {
                BlogPostComments newComment = new BlogPostComments
                {
                    ParentId = id,
                    Name = name,
                    Email = email,
                    Comment = message,
                    ReplyId = null

                };
                await db.BlogPostComments.AddAsync(newComment);
                await db.SaveChangesAsync();
            }
            else
            {

                BlogPostComments newComment = new BlogPostComments
                {
                    ParentId = id,
                    Name = name,
                    Email = email,
                    Comment = message,
                    ReplyId = replyid

                };
                await db.BlogPostComments.AddAsync(newComment);
                await db.SaveChangesAsync();
            }


            return Json(new { status = 200 });
        }

    }
}
    