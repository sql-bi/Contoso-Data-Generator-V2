using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter.Parquet
{

    // ---------------------------------------
    // Delta schema json serialization classes
    // ---------------------------------------

    public class DeltaSchema
    {
        public string type { get; set; }
        public List<DeltaField> fields { get; set; }
    }

    public class DeltaField
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool nullable { get; set; }
        public DeltaMetadata metadata { get; set; }

        public static DeltaField GetInstance(string nameValue, string typeValue, bool nullableValue)
        {
            return new DeltaField() { name = nameValue, type = typeValue, nullable = nullableValue, metadata = new DeltaMetadata() };
        }
    }

    public class DeltaMetadata
    {
    }

}
