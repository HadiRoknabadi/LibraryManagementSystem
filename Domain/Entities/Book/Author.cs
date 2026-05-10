using Domain.Entities.Common;

namespace Domain.Entities.Book
{
    public class Author:BaseEntity
    {
        #region Properties

        public string Name { get; set; }
        public string Family { get; set; }
        public string Biography { get; set; }
        public string FullName { get { return $"{Name} {Family}"; } }

        #endregion

    }
}
