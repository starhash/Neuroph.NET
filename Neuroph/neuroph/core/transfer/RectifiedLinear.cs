using System;

namespace org.neuroph.core.transfer
{

	public class RectifiedLinear : TransferFunction
	{


		public override double getOutput(double net)
		{
	//        return Math.min(Math.max(0, net), 100);
			return Math.Max(0, net);
		}

		public override double getDerivative(double net)
		{
			if (net > double.Epsilon)
			{
				return 1;
			}
			return 0;
		}

	}

}