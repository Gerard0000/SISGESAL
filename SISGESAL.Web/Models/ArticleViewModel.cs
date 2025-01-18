using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class ArticleViewModel : Article
    {
        public int KindofArticleId { get; set; }
    }
}
