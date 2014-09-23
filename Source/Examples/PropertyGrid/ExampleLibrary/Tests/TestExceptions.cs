// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestExceptions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    [PropertyGridExample]
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