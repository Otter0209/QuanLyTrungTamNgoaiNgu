    using System;
    using System.ComponentModel.DataAnnotations;

    namespace QuanLyTrungTamNN.Models
    {
        public partial class STUDENT
        {

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Ngày sinh")]
            public DateTime DateOfBirthRequired
            {
                get { return DateOfBirth ?? DateTime.MinValue; }
                set { DateOfBirth = value; }
            }

            public int CalculateAge()
            {
                if (!DateOfBirth.HasValue)
                    return 0;

                int age = DateTime.Now.Year - DateOfBirth.Value.Year;
                if (DateTime.Now < DateOfBirth.Value.AddYears(age)) age--; // Điều chỉnh nếu chưa đến ngày sinh nhật
                return age;
            }
        }
    }
