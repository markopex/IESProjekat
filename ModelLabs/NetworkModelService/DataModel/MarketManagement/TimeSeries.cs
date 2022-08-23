using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class TimeSeries : IdentifiedObject
    {
        private string objectAggregation;
        private string product;
        private string version;
        private long auction;
        private List<long> measurementPoints = new List<long>();
		private long reason;
        private long marketParticipant;

        public TimeSeries(long globalId) : base(globalId)
        {
        }
        public string ObjectAggregation { get => objectAggregation; set => objectAggregation = value; }
        public string Product { get => product; set => product = value; }
        public string Version { get => version; set => version = value; }
        public long Auction { get => auction; set => auction = value; }
        public List<long> MeasurementPoints { get => measurementPoints; set => measurementPoints = value; }
        public long Reason { get => reason; set => reason = value; }
        public long MarketParticipant { get => marketParticipant; set => marketParticipant = value; }
		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (TimeSeries)x;
				return base.Equals(x) && this.objectAggregation == obj.objectAggregation &&
					this.product == obj.product &&
                    this.version == obj.version &&
					this.auction == obj.auction &&
					CompareHelper.CompareLists(obj.measurementPoints, this.measurementPoints) &&
					this.reason == obj.reason &&
					this.marketParticipant == obj.marketParticipant
					;
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
				case ModelCode.TIMESERIES_OBJAGGREGATION:
				case ModelCode.TIMESERIES_PRODUCT:
				case ModelCode.TIMESERIES_VERSION:
				case ModelCode.TIMESERIES_AUCTION:
				case ModelCode.TIMESERIES_MPOINT:
				case ModelCode.TIMESERIES_REASON:
				case ModelCode.TIMESERIES_MARKETPARTICIPANT:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.TIMESERIES_OBJAGGREGATION:
					property.SetValue(objectAggregation);
					break;
				case ModelCode.TIMESERIES_PRODUCT:
					property.SetValue(product);
					break;
				case ModelCode.TIMESERIES_VERSION:
					property.SetValue(version);
					break;
				case ModelCode.TIMESERIES_AUCTION:
					property.SetValue(auction);
					break;
				case ModelCode.TIMESERIES_MPOINT:
					property.SetValue(measurementPoints);
					break;
				case ModelCode.TIMESERIES_REASON:
					property.SetValue(reason);
					break;
				case ModelCode.TIMESERIES_MARKETPARTICIPANT:
					property.SetValue(marketParticipant);
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
				case ModelCode.TIMESERIES_OBJAGGREGATION:
					objectAggregation = property.AsString();
					break;
				case ModelCode.TIMESERIES_PRODUCT:
					product = property.AsString();
					break;
				case ModelCode.TIMESERIES_VERSION:
					version = property.AsString();
					break;
				case ModelCode.TIMESERIES_AUCTION:
					auction = property.AsReference();
					break;
				case ModelCode.TIMESERIES_REASON:
					reason = property.AsReference();
					break;
				case ModelCode.TIMESERIES_MARKETPARTICIPANT:
					marketParticipant = property.AsReference();
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
				return (measurementPoints.Count > 0) || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (measurementPoints != null && measurementPoints.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.TIMESERIES_MPOINT] = measurementPoints.GetRange(0, measurementPoints.Count);
			}
			if (reason != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.POINT_REASON] = new List<long> { reason };
			}
			if (auction != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TIMESERIES_AUCTION] = new List<long> { auction };
			}
			if (marketParticipant != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TIMESERIES_MARKETPARTICIPANT] = new List<long> { marketParticipant };
			}
			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.MEASUREMENTPOINT_TIMESERIES:
					measurementPoints.Add(globalId);
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
				case ModelCode.MEASUREMENTPOINT_TIMESERIES:

					if (measurementPoints.Contains(globalId))
					{
						measurementPoints.Remove(globalId);
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
