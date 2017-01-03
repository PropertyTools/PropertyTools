// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    [PropertyGridExample]
    public class ExceptionExample : Example
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
    }
}