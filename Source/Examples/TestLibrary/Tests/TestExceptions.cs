namespace TestLibrary
{
    using System;

    public class TestExceptions : TestBase
    {
        public string Impossibilium
        {
            get
            {
                return "Corrrect me!";
            }
            set
            {
                throw new InvalidOperationException("No way!");
            }
        }

        public bool IsOK
        {
            get
            {
                return false;
            }
            set
            {
                throw new InvalidOperationException("No!");
            }
        }
    
        public override string ToString()
        {
            return "Exceptions";
        }
    }
}