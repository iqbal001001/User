using System;

namespace User.Domain
{
    public interface IData
    {
        int Id { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
    }

    public class Data : IData
    {
        public int Id { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
