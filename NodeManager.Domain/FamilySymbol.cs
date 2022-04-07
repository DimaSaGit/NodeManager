//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NodeManager.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FamilySymbol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FamilySymbol()
        {
            this.RevitParameters = new HashSet<RevitParameter>();
            Tags1 = new List<FSTags>();
        }
    
        public int Id { get; set; }

        
        public Nullable<int> AuthorId { get; set; }
        public string Name { get; set; }
        public Nullable<int> FileId { get; set; }
        public Nullable<int> Scale { get; set; }
        public byte[] Image { get; set; }
        //public string Tags { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SectionId { get; set; }




        

        [ForeignKey("CategoryId")]
        public virtual Categories Category { get; set; }

        [ForeignKey("SectionId")]
        public virtual Sections Section { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Users User { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<RevitParameter> RevitParameters { get; set; }

        public virtual ICollection<FSTags> Tags1 { get; set; }
    }
}
