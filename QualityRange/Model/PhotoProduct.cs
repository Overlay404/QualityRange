//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QualityRange.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhotoProduct
    {
        public int ID { get; set; }
        public byte[] Photo { get; set; }
        public int ID_Product { get; set; }
    
        public virtual Product Product { get; set; }
    }
}