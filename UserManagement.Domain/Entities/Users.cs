using SharedKernel;

namespace UserManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string SurnameName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public int AuthId { get; private set; }
        public Address Address { get; private set; }

        // Konstruktor pa parametra
        public User() { }
        public User(string name, string surnameName, DateTime brithDate, int authId, Address address)
        {
            Name = name;
            SurnameName = surnameName;
            BirthDate = brithDate;
            AuthId = authId;
            Address = address;
        }
    }

}
