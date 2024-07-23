using DatabaseGenerator.DataWriter.Parquet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter.Parquet
{

    // -----------------
    // Required for serialization because JsonSerializerIsReflectionEnabledByDefault = false
    [JsonSerializable(typeof(DeltaSchema))]
    internal partial class DeltaSchemaSerializerContext : JsonSerializerContext
    {
    }
    // -----------------


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
