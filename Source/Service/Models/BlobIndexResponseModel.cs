namespace Christmas2016.Service.Models
{
    using Newtonsoft.Json;
    using System.Diagnostics;

    [DebuggerStepThrough]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class BlobIndexResponseModel
    {
        internal BlobIndexResponseModel(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public int Y { get; }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public int X { get; }
    }
}