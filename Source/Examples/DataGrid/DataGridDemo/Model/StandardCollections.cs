namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StandardCollections
    {
        private static readonly Random r = new Random(0);
        static StandardCollections()
        {
            Names = GenerateNames().ToArray();
        }

        public static string[] Cities { get; } = { "Oslo", "Stockholm", "Copenhagen" };
        public static string[] FirstNames { get; } = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };
        public static string[] LastNames { get; } = { "Smoth", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };
        public static string[] Names { get; }

        public static IEnumerable<string> GenerateNames(int n = 100)
        {
            for (var i = 0; i < n; i++)
            {
                yield return GenerateName();
            }
        }

        public static string GenerateName()
        {
            return FirstNames[r.Next(FirstNames.Length)] + " " + LastNames[r.Next(LastNames.Length)];
        }
    }
}