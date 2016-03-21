namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //在新增資料時this.ProductId不會有值，所以這邊檢查時this.ProductId會是預設值或0
            //如果this.ProductId有值代表是在更新
            if (this.ProductId != default(int))
            {
                //更新
            }

            if (this.Price < 100)
            {

                //return IEnumerable時，都要用yield return來return錯誤
                yield return new ValidationResult("價錢不可小於50", new string[] { "Price"});
            }

            if (this.Stock < 10 )
            {
                yield return new ValidationResult("庫存不可小於10", new string[] { "Stock"});

            }
        }
    }

    public partial class ProductMetaData
    {
        [Required]
        public int ProductId { get; set; }
        
        [StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        [產品名稱必須至少包含兩個空白字元(ErrorMessage = "產品名稱必須至少包含兩個空白字元")]
        public string ProductName { get; set; }
        [Required]
        public Nullable<decimal> Price { get; set; }
        [Required]
        public Nullable<bool> Active { get; set; }
        [Required]
        public Nullable<decimal> Stock { get; set; }
    
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
