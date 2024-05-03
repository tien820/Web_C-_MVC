﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTAnhNgu
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class HOC_VIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOC_VIEN()
        {
            this.NGAY_DANG_KY = new HashSet<NGAY_DANG_KY>();
        }
    
        public int HV_MAHV { get; set; }

        [Required(ErrorMessage = "Họ tên học viên không được để trống")]
        public string HV_HOTEN { get; set; }

        [RegularExpression(@"^[0-9]{1,15}$", ErrorMessage = "Căn cước phải là số và tối đa 15 số")]
        public string HV_CCCD { get; set; }
        public Nullable<bool> HV_GIOITINH { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> HV_NGAYSINH { get; set; }
        public string HV_DIACHI { get; set; }

        [RegularExpression(@"^[0-9]{1,15}$", ErrorMessage = "Số điện thoại phải là số và không vượt quá 15 số")]
        public string HV_SDT { get; set; }

        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string HV_EMAIL { get; set; }
        public string HV_MATKHAU { get; set; }
        public string HV_HINHANH { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NGAY_DANG_KY> NGAY_DANG_KY { get; set; }
    }
}
