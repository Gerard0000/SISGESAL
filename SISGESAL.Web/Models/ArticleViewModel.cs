using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class ArticleViewModel : Article
    {
        //TIPO DE ARTÍCULO
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Tipo de Artículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int KindofArticleId { get; set; }

        public IEnumerable<SelectListItem>? KindofArticles { get; set; }

        //MARCA DE ARTÍCULO
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Marca de Artículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int TrademarkId { get; set; }

        public IEnumerable<SelectListItem>? Trademarks { get; set; }

        //PROVEEDOR
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Proveedor")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int SupplierId { get; set; }

        public IEnumerable<SelectListItem>? Suppliers { get; set; }
    }
}