using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Common
{
    public enum FilterDataOrder
    {
        [Display(Name = "صعودی")]
        CreateDate_ASC,

        [Display(Name = "نزولی")]
        CreateDate_DES
    }
}
