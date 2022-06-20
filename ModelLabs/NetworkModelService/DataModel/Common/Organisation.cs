using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Common
{
    public class Organisation : IdentifiedObject
    {
        private string electronicAddress;
        private string phone1;
        private string phone2;
        private string postalAddress;
        private string streetAddress;

        public Organisation(long globalId) : base(globalId)
        {
        }

        public string ElectronicAddress { get => electronicAddress; set => electronicAddress = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        public string PostalAddress { get => postalAddress; set => postalAddress = value; }
        public string StreetAddress { get => streetAddress; set => streetAddress = value; }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                var organisation = x as Organisation;
                return organisation.electronicAddress == this.electronicAddress &&
                    organisation.phone1 == this.phone1 &&
                    organisation.phone2 == this.phone2 &&
                    organisation.postalAddress == this.postalAddress &&
                    organisation.streetAddress == this.streetAddress;
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
				case ModelCode.ORGANISATION_ELECTRONICADDRESS:
				case ModelCode.ORGANISATION_PHONE1:
				case ModelCode.ORGANISATION_PHONE2:
				case ModelCode.ORGANISATION_POSTALADDRESS:
				case ModelCode.ORGANISATION_STREETADDRESS:
					return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.ORGANISATION_ELECTRONICADDRESS:
					property.SetValue(electronicAddress);
					break;

				case ModelCode.ORGANISATION_PHONE1:
					property.SetValue(phone1);
					break;

				case ModelCode.ORGANISATION_PHONE2:
					property.SetValue(phone2);
					break;

				case ModelCode.ORGANISATION_POSTALADDRESS:
					property.SetValue(postalAddress);
					break;

				case ModelCode.ORGANISATION_STREETADDRESS:
					property.SetValue(streetAddress);
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
				case ModelCode.ORGANISATION_ELECTRONICADDRESS:
					electronicAddress = property.AsString();
					break;

				case ModelCode.ORGANISATION_PHONE1:
					phone1 = property.AsString();
					break;

				case ModelCode.ORGANISATION_PHONE2:
					phone2 = property.AsString();
					break;

				case ModelCode.ORGANISATION_POSTALADDRESS:
					postalAddress = property.AsString();
					break;

				case ModelCode.ORGANISATION_STREETADDRESS:
					streetAddress = property.AsString();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation
	}
}
