using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyTrungTamNN.ViewModels
{
    public class StudentParentViewModel
    {
        // Thuộc tính cho STUDENT
        public int StudentID { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }

        // Thuộc tính cho PARENT
        // (Nếu bạn chỉ cần họ tên, điện thoại, email, v.v., tùy ý thêm bớt)
        public int? ParentID { get; set; } // Nếu muốn cho phép chọn ID cũ, 
                                           // bạn có thể dùng dropdown, hoặc bỏ qua
        [Required]
        public string ParentFullName { get; set; }
        public string ParentPhone { get; set; }
        public string ParentEmail { get; set; }
        public string ParentAddress { get; set; }
    }
}
