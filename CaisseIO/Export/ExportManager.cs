using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaisseIO.Export
{
    public class ExportManager
    {
        private Dictionary<string, List<object[]>> SortedObjects;
        private readonly List<object[]> Objects;
        private List<string> Output;
        private string Path { get; }


        public ExportManager(string path, object[] o) : this(path, new List<object[]> { o })
        {
        }

        public ExportManager(string path, List<object[]> objects)
        {
            Output = new List<string>();
            Objects = objects;
            Path = path;
        }

        private object Recursiv(object[] obj)
        {
            if (obj == null || obj.Length <= 1) return null;

            if (!(obj[0] is string str)) return null;

            object returnObject = obj;

            if (SortedObjects.ContainsKey(str))
            {
                var i = obj[1] as int? ?? 0;

                if (SortedObjects[str].All(t => (int) t[1] != i))
                {
                    SortedObjects[str].Add(obj);
                }

                returnObject = i;

            }

            SortedObjects.Add(str, new List<object[]>{obj});

            returnObject = obj[1] as int? ?? 0;

            for (var index = 0; index < obj.Length; index++)
            {
                var o = obj[index];

                if (o is object[] z)
                {
                    obj[index] = Recursiv(z);
                }

            }

            return returnObject;
        }

        public void Analyse()
        {

            foreach (var o in Objects)
            {
                if (o == null || o.Length <= 1) continue;

                if (!(o[0] is string str)) continue;

                if (SortedObjects.ContainsKey(str))
                {
                    SortedObjects[str].Add(Re);
                }
                else
                {
                    SortedObjects.Add(str, new List<object[]> { o });
                }
            }

            foreach (var o in Objects)
            {
                if (o == null || o.Length == 0) continue;

                var builder = new StringBuilder();


                Output.Add(builder.ToString());
            }
        }
    }
}