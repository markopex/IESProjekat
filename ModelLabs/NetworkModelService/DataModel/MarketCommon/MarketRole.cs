using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.MarketCommon
{
    public class MarketRole : IdentifiedObject
    {
        private MarketRoleKind marketRoleKind;
        private string status;
        private string type;
        private List<long> marketParticipant = new List<long>();

		public MarketRole(long globalId) : base(globalId)
        {
        }
        public MarketRoleKind MarketRoleKind { get { return marketRoleKind; } set { marketRoleKind = value; } }
        public string Status { get { return status; } set { status = value; } }
        public string Type { get { return type; } set { type = value; } }
        public List<long> MarketParticipant { get { return marketParticipant; } set { marketParticipant = value; } }

		public override bool Equals(object x)
		{
			if (base.Equals(x))
			{
				var obj = (MarketRole)x;
				return base.Equals(x) && this.marketRoleKind == obj.marketRoleKind &&
					this.status == obj.status &&
					this.type == obj.type &&
					CompareHelper.CompareLists(obj.marketParticipant, this.marketParticipant);
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
				case ModelCode.MARKETROLE_ROLE_TYPE:
				case ModelCode.MARKETROLE_STATUS:
				case ModelCode.MARKETROLE_TYPE:
				case ModelCode.MARKETROLE_MARKETPARTICIPANT:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.MARKETROLE_ROLE_TYPE:
					property.SetValue((short)marketRoleKind);
					break;
				case ModelCode.MARKETROLE_STATUS:
					property.SetValue(status);
					break;
				case ModelCode.MARKETROLE_TYPE:
					property.SetValue(type);
					break;
				case ModelCode.MARKETROLE_MARKETPARTICIPANT:
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
				case ModelCode.MARKETROLE_ROLE_TYPE:
					marketRoleKind = (MarketRoleKind)property.AsEnum();
					break;
				case ModelCode.MARKETROLE_STATUS:
					status = property.AsString();
					break;
				case ModelCode.MARKETROLE_TYPE:
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
				return (marketParticipant.Count > 0) || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (marketParticipant != null && marketParticipant.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.MARKETROLE_MARKETPARTICIPANT] = marketParticipant.GetRange(0, marketParticipant.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.MARKETPARTICIPANT_MARKETROLE:
					marketParticipant.Add(globalId);
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
				case ModelCode.MARKETPARTICIPANT_MARKETROLE:

					if (marketParticipant.Contains(globalId))
					{
						marketParticipant.Remove(globalId);
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
