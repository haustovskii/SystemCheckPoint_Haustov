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
    
    public partial class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronumic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public System.DateTime Birthday { get; set; }
        public int SeriesPassport { get; set; }
        public int NumberPassport { get; set; }
        public int IDPost { get; set; }
        public Nullable<int> IDAutoTransport { get; set; }
        public int IDPass { get; set; }
        public int IDGender { get; set; }
    
        public virtual AutoTransport AutoTransport { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Pass Pass { get; set; }
        public virtual Post Post { get; set; }
    }
}