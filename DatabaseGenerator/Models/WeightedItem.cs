using System;


namespace DatabaseGenerator.Models
{
    public abstract class WeightedItem
    {
        public double[] Weights { get; set; }       // from data file
        public double[] DaysWeight { get; set; }    // calculated
    }
}
