using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketCommon
{
    public class MarketParticipant : IdentifiedObject
    {
		private long marketRole;
		private List<long> timeSeries;
        public MarketParticipant(long globalId) : base(globalId)
        {
        }

		public long MarketRole { get { return marketRole; } set { marketRole = value; } }
		public List<long> TimeSeries { get { return timeSeries; } set { timeSeries = value; } }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
				var obj = (MarketParticipant)x;
				return base.Equals(x) && this.marketRole == obj.marketRole && 
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
				case ModelCode.MARKETPARTICIPANT_MARKETROLE:
				case ModelCode.MARKETPARTICIPANT_TIMESERIES:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.MARKETPARTICIPANT_MARKETROLE:
					property.SetValue(marketRole);
					break;
				case ModelCode.MARKETPARTICIPANT_TIMESERIES:
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
				case ModelCode.MARKETPARTICIPANT_MARKETROLE:
					marketRole = property.AsReference();
					break;
				default:
					base.SetProperty(property);
					break;
			}
		}

        #endregion IAccess implementation


        public override bool IsReferenced
        {
			get
            {
				return timeSeries.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
			if(marketRole != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MARKETPARTICIPANT_MARKETROLE] = new List<long> { marketRole };
            }
			if(timeSeries != null && timeSeries.Count != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
				references[ModelCode.MARKETPARTICIPANT_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
				case ModelCode.TIMESERIES_MARKETPARTICIPANT:
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
				case ModelCode.TIMESERIES_MARKETPARTICIPANT:

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

    }
}
