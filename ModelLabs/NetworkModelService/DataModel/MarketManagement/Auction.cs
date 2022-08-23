using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class Auction : IdentifiedObject
    {
        private string type;
        private List<long> timeSeries = new List<long>();
        public Auction(long globalId) : base(globalId)
        {
        }
        public string Type { get => type; set => type = value; }
        public List<long> TimeSeries { get { return timeSeries; } set { timeSeries = value; } }
		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (Auction)x;
				return base.Equals(x) && this.type == obj.type &&
					CompareHelper.CompareLists(obj.timeSeries, this.timeSeries);
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
				case ModelCode.AUCTION_TYPE:
				case ModelCode.AUCTION_TIMESERIES:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.AUCTION_TYPE:
					property.SetValue(type);
					break;
				case ModelCode.AUCTION_TIMESERIES:
					property.SetValue(timeSeries);
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
				case ModelCode.AUCTION_TYPE:
					type = property.AsString();
					break;
				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override bool IsReferenced
		{
			get
			{
				return (timeSeries.Count > 0) || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (timeSeries != null && timeSeries.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.AUCTION_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.TIMESERIES_AUCTION:
					timeSeries.Add(globalId);
					break;

				default:
					base.AddReference(referenceId, globalId);
					break;
			}
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.TIMESERIES_AUCTION:

					if (timeSeries.Contains(globalId))
					{
						timeSeries.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;
				default:
					base.RemoveReference(referenceId, globalId);
					break;
			}
		}

		#endregion IReference implementation
	}
}
