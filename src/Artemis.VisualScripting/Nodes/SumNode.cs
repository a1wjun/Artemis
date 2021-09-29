﻿using System.Linq;
using Artemis.Core;

namespace Artemis.VisualScripting.Nodes
{
    [Node("Sum (Integer)", "Sums the connected integer values.", "Mathematics", InputType = typeof(int), OutputType = typeof(int))]
    public class SumIntegersNode : Node
    {
        #region Properties & Fields

        public InputPinCollection<int> Values { get; }

        public OutputPin<int> Sum { get; }

        #endregion

        #region Constructors

        public SumIntegersNode()
            : base("Sum", "Sums the connected integer values.")
        {
            Values = CreateInputPinCollection<int>("Values", 2);
            Sum = CreateOutputPin<int>("Sum");
        }

        #endregion

        #region Methods

        public override void Evaluate()
        {
            Sum.Value = Values.Values.Sum();
        }

        #endregion
    }

    [Node("Sum (Float)", "Sums the connected float values.", "Mathematics", InputType = typeof(float), OutputType = typeof(float))]
    public class SumFloatsNode : Node
    {
        #region Properties & Fields

        public InputPinCollection<float> Values { get; }

        public OutputPin<float> Sum { get; }

        #endregion

        #region Constructors

        public SumFloatsNode()
            : base("Sum", "Sums the connected float values.")
        {
            Values = CreateInputPinCollection<float>("Values", 2);
            Sum = CreateOutputPin<float>("Sum");
        }

        #endregion

        #region Methods

        public override void Evaluate()
        {
            Sum.Value = Values.Values.Sum();
        }

        #endregion
    }
}
