namespace MVC5Course.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product //: IValidatableObject
    {
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
            ////在新增資料時this.ProductId不會有值，所以這邊檢查時this.ProductId會是預設值或0
            ////如果this.ProductId有值代表是在更新
            //if (this.ProductId != default(int))
            //{
            //    //更新
            //}

            //if (this.Price < 0)
            //{

            //    //return IEnumerable時，都要用yield return來return錯誤
            //    yield return new ValidationResult("價錢不可小於0", new string[] { "Price"});
            //}

            //if (this.Stock < 10 )
            //{
            //    yield return new ValidationResult("庫存不可小於10", new string[] { "Stock"});

            //}
        //}
    }
    //[DataContract]
    public partial class ProductMetaData
    {
        //[DataMember]
        [Required]
        public int ProductId { get; set; }
        //[DataMember]
        [StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        //[產品名稱必須至少包含兩個空白字元(ErrorMessage = "產品名稱必須至少包含兩個空白字元")]
        public string ProductName { get; set; }
        //[DataMember]
        [Required]
        public Nullable<decimal> Price { get; set; }
        //[DataMember]
        [Required]
        public Nullable<bool> Active { get; set; }
        //[DataMember]
        [Required]
        public Nullable<decimal> Stock { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
