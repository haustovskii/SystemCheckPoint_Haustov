//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SystemCheckPoint.AppData
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExternalPerson
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronumic { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> SeriesPassport { get; set; }
        public Nullable<int> NumberPassport { get; set; }
        public Nullable<int> IDAutoTransport { get; set; }
        public Nullable<int> IDPass { get; set; }
        public Nullable<int> IDAccountingMaterialValue { get; set; }
        public Nullable<int> IDGender { get; set; }
    
        public virtual AccountingMaterialValue AccountingMaterialValue { get; set; }
        public virtual AutoTransport AutoTransport { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Pass Pass { get; set; }
    }
}