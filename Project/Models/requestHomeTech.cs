//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class requestHomeTech
    {
        public int requestID { get; set; }
        public int homeTechID { get; set; }
    
        public virtual homeTech homeTech { get; set; }
        public virtual request request { get; set; }
    }
}
