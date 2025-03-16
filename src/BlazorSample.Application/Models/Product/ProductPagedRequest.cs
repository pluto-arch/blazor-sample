using System.ComponentModel.DataAnnotations;
using BlazorSample.Application.Models.Generics;

namespace BlazorSample.Application.Models.Product;

public class ProductPagedRequest : PageRequest
{
    [MaxLength(3, ErrorMessage = "MaxLengthValidate")]
    public string Keyword { get; set; }
}