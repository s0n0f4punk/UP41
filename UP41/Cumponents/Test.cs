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
    
    public partial class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> isPassed { get; set; }
        public string Description { get; set; }
        public Nullable<int> IdProduct { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
