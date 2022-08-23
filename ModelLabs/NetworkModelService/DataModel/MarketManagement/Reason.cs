using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class Reason : IdentifiedObject
    {
        private string code;
        private string text;
        private List<long> points = new List<long>();
		private List<long> timeSeries = new List<long>();

		public Reason(long globalId) : base(globalId)
        {
        }

        public string Code { get { return code; } set { code = value; } }
        public string Text { get { return text; } set { text = value; } }
        public List<long> Points { get { return points; } set { points = value; } }
        public List<long> TimeSeries { get { return timeSeries; } set { timeSeries = value; } }

		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (Reason)x;
				return base.Equals(x) && this.code == obj.code &&
					this.text == obj.text &&
					CompareHelper.CompareLists(obj.points, this.points) &&
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
				case ModelCode.REASON_CODE:
				case ModelCode.REASON_TEXT:
				case ModelCode.REASON_POINT:
				case ModelCode.REASON_TIMESERIES:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.REASON_CODE:
					property.SetValue(code);
					break;
				case ModelCode.REASON_TEXT:
					property.SetValue(text);
					break;
				case ModelCode.REASON_POINT:
					property.SetValue(points);
					break;
				case ModelCode.REASON_TIMESERIES:
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
				case ModelCode.REASON_CODE:
					code = property.AsString();
					break;
				case ModelCode.REASON_TEXT:
					text = property.AsString();
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
				return (points.Count > 0) || (timeSeries.Count > 0) || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (points != null && points.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.REASON_POINT] = points.GetRange(0, points.Count);
			}
			if (timeSeries != null && timeSeries.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.REASON_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.POINT_REASON:
					points.Add(globalId);
					break;
				case ModelCode.TIMESERIES_REASON:
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
				case ModelCode.POINT_REASON:

					if (points.Contains(globalId))
					{
						points.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;
				case ModelCode.TIMESERIES_REASON:

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
