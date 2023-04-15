using System;
using System.ComponentModel.DataAnnotations;

namespace testAppIrkit
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Performer { get; set; }
        public string FirstName { get; set; }
        public DateTime RegDate { get; set; }
        public string Content { get; set; }
        public string KindName { get; set; }
        public string ReferenceList { get; set; }
    }
}
