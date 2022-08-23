using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class BidTimeSeries : TimeSeries
    {
        private bool blockBid;
        private string direction;
        private bool divisible;
        private string linkedBidsIdentification;
        private float minimumActivationQuantity;
        private float stepIncrementQuantity;

        public BidTimeSeries(long globalId) : base(globalId)
        {
        }
        public bool BlockBid { get => blockBid; set => blockBid = value; }
        public string Direction { get => direction; set => direction = value; }
        public bool Divisible { get => divisible; set => divisible = value; }
        public string LinkedBidsIdentification { get => linkedBidsIdentification; set => linkedBidsIdentification = value; }
        public float MinimumActivationQuantity { get => minimumActivationQuantity; set => minimumActivationQuantity = value; }
        public float StepIncrementQuantity { get => stepIncrementQuantity; set => stepIncrementQuantity = value; }

		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (BidTimeSeries)x;
				return base.Equals(x) && this.blockBid == obj.blockBid &&
					this.direction == obj.direction &&
					this.divisible == obj.divisible &&
					this.linkedBidsIdentification == obj.linkedBidsIdentification &&
					this.minimumActivationQuantity == obj.minimumActivationQuantity &&
					this.stepIncrementQuantity == obj.stepIncrementQuantity;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode property)
		{
			switch (property)
			{
				case ModelCode.BIDTIMESERIES_BLOCKBID:
				case ModelCode.BIDTIMESERIES_DIRECTION:
				case ModelCode.BIDTIMESERIES_DIVISIBLE:
				case ModelCode.BIDTIMESERIES_LBI:
				case ModelCode.BIDTIMESERIES_MAQ:
				case ModelCode.BIDTIMESERIES_SIQ:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.BIDTIMESERIES_BLOCKBID:
					property.SetValue(blockBid);
					break;
				case ModelCode.BIDTIMESERIES_DIRECTION:
					property.SetValue(direction);
					break;
				case ModelCode.BIDTIMESERIES_DIVISIBLE:
					property.SetValue(divisible);
					break;
				case ModelCode.BIDTIMESERIES_LBI:
					property.SetValue(linkedBidsIdentification);
					break;
				case ModelCode.BIDTIMESERIES_MAQ:
					property.SetValue(minimumActivationQuantity);
					break;
				case ModelCode.BIDTIMESERIES_SIQ:
					property.SetValue(stepIncrementQuantity);
					break;
				default:
					base.GetProperty(property);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.BIDTIMESERIES_BLOCKBID:
					blockBid = property.AsBool();
					break;
				case ModelCode.BIDTIMESERIES_DIRECTION:
					direction = property.AsString();
					break;
				case ModelCode.BIDTIMESERIES_DIVISIBLE:
					divisible = property.AsBool();
					break;
				case ModelCode.BIDTIMESERIES_LBI:
					linkedBidsIdentification = property.AsString();
					break;
				case ModelCode.BIDTIMESERIES_MAQ:
					minimumActivationQuantity = property.AsFloat();
					break;
				case ModelCode.BIDTIMESERIES_SIQ:
					stepIncrementQuantity = property.AsFloat();
					break;
				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation
	}
}
