//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UP41.Cumponents
{
    using System;
    using System.Collections.Generic;
    
    public partial class Description
    {
        public int Id { get; set; }
        public string Subtitle { get; set; }
        public string MaterialId { get; set; }
    
        public virtual Material Material { get; set; }
    }
}