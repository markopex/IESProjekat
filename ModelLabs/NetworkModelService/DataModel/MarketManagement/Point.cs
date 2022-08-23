using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class Point : IdentifiedObject
    {
        private float bidQuantity;
        private int position;
        private float quantity;
        private long reason;
        public Point(long globalId) : base(globalId)
        {
        }
        public float BidQuantity { get { return bidQuantity; } set { bidQuantity = value; } }
        public int Position { get { return position; } set { position = value; } }
        public float Quantity { get { return quantity; } set { quantity = value; } }
        public long Reason { get { return reason; } set { reason = value; } }

		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (Point)x;
				return base.Equals(x) && this.bidQuantity == obj.bidQuantity &&
					this.position == obj.position &&
					this.quantity == obj.quantity &&
					this.reason == obj.reason;
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
				case ModelCode.POINT_BIDQTY:
				case ModelCode.POINT_POSITION:
				case ModelCode.POINT_QUANTITY:
				case ModelCode.REASON:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.POINT_BIDQTY:
					property.SetValue(bidQuantity);
					break;
				case ModelCode.POINT_POSITION:
					property.SetValue(position);
					break;
				case ModelCode.POINT_QUANTITY:
					property.SetValue(quantity);
					break;
				case ModelCode.REASON:
					property.SetValue(reason);
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
				case ModelCode.POINT_BIDQTY:
					bidQuantity = property.AsFloat();
					break;
				case ModelCode.POINT_POSITION:
					position = property.AsInt();
					break;
				case ModelCode.POINT_QUANTITY:
					quantity = property.AsFloat();
					break;
				case ModelCode.REASON:
					reason = property.AsReference();
					break;
				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (reason != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.POINT_REASON] = new List<long> { reason };
			}

			base.GetReferences(references, refType);
		}
		#endregion IReference implementation
	}
}
