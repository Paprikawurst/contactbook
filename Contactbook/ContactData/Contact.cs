namespace ContactData
{
    public abstract class Contact
    {
        public int ContactIndexNumber
        {
            get;
            set;
        }

        public string ContactName
        {
            get;
            set;

        }

        public Location Location
        {
            get;
            set;
        }
        public long PhoneNumber
        {
            get;
            set;
        }

        public string MailAdress
        {
            get;
            set;
        }

        public abstract string Gender
        {
            get;
        }              
    }
}
