using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CsvDemo
{
    /// <summary>
    /// A class for comma-separated value files
    /// http://en.wikipedia.org/wiki/Comma-separated_values
    /// Default is "USA/UK CSV" where the separator is ',' and decimal separator is '.'.
    /// Todo: Support quoted values...
    /// </summary>
    public class CsvDocument
    {
        public string[] Headers { get; private set; }
        public Collection<string[]> Items { get; private set; }

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="separator">The separator (auto-detect if not specified).</param>
        public void Load(string fileName, char separator = '\0')
        {
            using (var r = new StreamReader(fileName))
            {
                var header = r.ReadLine();


                if (separator == '\0')
                {
                    // Auto detect
                    int commaCount = Count(header, ',');
                    int semicolonCount = Count(header, ';');
                    separator = commaCount > semicolonCount ? ',' : ';';
                }

                Headers = header.Split(separator);
                Items = new Collection<string[]>();

                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    if (line == null || line.StartsWith("%") || line.StartsWith("//"))
                        continue;
                    Items.Add(line.Split(separator));
                }
            }
        }

        public static Collection<T> Load<T>(string path, char separator = '\0') where T : class
        {
            var result = new Collection<T>();
            var doc = new CsvDocument();
            doc.Load(path, separator);
            var properties = new PropertyInfo[doc.Headers.Length];
            var itemType = typeof(T);
            for (int i = 0; i < doc.Headers.Length; i++)
                properties[i] = itemType.GetProperty(doc.Headers[i]);

            foreach (var item in doc.Items)
            {
                var o = Activator.CreateInstance(itemType) as T;
                for (int i = 0; i < properties.Length; i++)
                    if (properties[i] != null)
                    {
                        // todo: convert value
                        var converter=TypeDescriptor.GetConverter(properties[i].PropertyType);
                        object value = item[i];
                        if (value!=null && converter!=null && converter.CanConvertFrom(value.GetType()))
                        {
                            value = converter.ConvertFrom(value);
                        }
                        properties[i].SetValue(o, value, null);
                    }
                result.Add(o);
            }
            return result;
        }


        private int Count(string s, char c)
        {
            return s.Count(ch => ch == c);
        }
    }
}