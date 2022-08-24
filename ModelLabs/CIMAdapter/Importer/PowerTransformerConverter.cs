namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN.Common;
    using System;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}
		public static void PopulateOrganisationProperties(FTN.Organisation cimOrganisation, ResourceDescription rd)
		{
			if ((cimOrganisation != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimOrganisation, rd);

				if (cimOrganisation.ElectronicAddressHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ORGANISATION_ELECTRONICADDRESS, cimOrganisation.ElectronicAddress.ID));
				}
				if (cimOrganisation.Phone1HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ORGANISATION_PHONE1, cimOrganisation.Phone1));
				}
				if (cimOrganisation.Phone2HasValue)
				{
					rd.AddProperty(new Property(ModelCode.ORGANISATION_PHONE2, cimOrganisation.Phone2.ID));
				}
				if (cimOrganisation.PostalAddressHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ORGANISATION_POSTALADDRESS, cimOrganisation.PostalAddress.ID));
				}
				if (cimOrganisation.StreetAddressHasValue)
				{
					rd.AddProperty(new Property(ModelCode.ORGANISATION_STREETADDRESS, cimOrganisation.StreetAddress.ID));
				}
			}
		}
		public static void PopulateTimeSeriesProperties(FTN.TimeSeries cimTimeSeries, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTimeSeries != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTimeSeries, rd);

				if (cimTimeSeries.ObjectAggregationHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_OBJAGGREGATION, cimTimeSeries.ObjectAggregation));
				}
				if (cimTimeSeries.ProductHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_PRODUCT, cimTimeSeries.Product));
				}
				if (cimTimeSeries.VersionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_VERSION, cimTimeSeries.Version));
				}
				if (cimTimeSeries.MarketParticipantHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTimeSeries.MarketParticipant.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
						report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimTimeSeries.MarketParticipant.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TIMESERIES_MARKETPARTICIPANT, gid));
				}
				if (cimTimeSeries.ReasonHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTimeSeries.Reason.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
						report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimTimeSeries.Reason.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TIMESERIES_REASON, gid));
				}
				if (cimTimeSeries.AuctionHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTimeSeries.Auction.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
						report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimTimeSeries.Auction.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TIMESERIES_AUCTION, gid));
				}
			}
		}
		public static void PopulateMarketRoleProperties(FTN.MarketRole cimMarketRole, ResourceDescription rd)
		{
			if ((cimMarketRole != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimMarketRole, rd);

				if (cimMarketRole.StatusHasValue)
				{
					rd.AddProperty(new Property(ModelCode.MARKETROLE_STATUS, cimMarketRole.Status.ID));
				}
				if (cimMarketRole.TypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.MARKETROLE_TYPE, cimMarketRole.Type));
				}
				if (cimMarketRole.RoleTypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.MARKETROLE_ROLE_TYPE, (short)GetDMSMarketRoleKind(cimMarketRole.RoleType)));
				}
			}
		}
        public static void PopulateMarketParticipantProperties(FTN.MarketParticipant cimMarketParticipant, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimMarketParticipant != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateOrganisationProperties(cimMarketParticipant, rd);

				if (cimMarketParticipant.MarketRoleHasValue)
				{
					long gid = importHelper.GetMappedGID(cimMarketParticipant.MarketRole.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimMarketParticipant.GetType().ToString()).Append(" rdfID = \"").Append(cimMarketParticipant.ID);
						report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(cimMarketParticipant.MarketRole.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.MARKETPARTICIPANT_MARKETROLE, gid));
				}
			}
		}
		public static void PopulateAuctionProperties(FTN.Auction cimAuction, ResourceDescription rd)
		{
			if ((cimAuction != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimAuction, rd);

				if (cimAuction.TypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.AUCTION_TYPE, cimAuction.Type));
				}
			}
		}
		public static void PopulateReasonProperties(FTN.Reason cimReason, ResourceDescription rd)
		{
			if ((cimReason != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimReason, rd);

				if (cimReason.CodeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REASON_CODE, cimReason.Code));
				}
				if (cimReason.TextHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REASON_TEXT, cimReason.Text));
				}
			}
		}
		public static void PopulatePointProperties(FTN.Point cimPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPoint != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPoint, rd);

				if (cimPoint.PositionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POINT_POSITION, cimPoint.Position));
				}
				if (cimPoint.ReasonHasValue)
				{
					long gid = importHelper.GetMappedGID(cimPoint.Reason.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimPoint.ID);
						report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimPoint.Reason.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.POINT_REASON, gid));
				}
			}
		}
		public static void PopulateBidTimeSeiesProperties(FTN.BidTimeSeries cimBidTimeSeries, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimBidTimeSeries != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateTimeSeriesProperties(cimBidTimeSeries, rd, importHelper, report);

				if (cimBidTimeSeries.BlockBidHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BIDTIMESERIES_BLOCKBID, cimBidTimeSeries.BlockBid));
				}
				if (cimBidTimeSeries.DirectionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BIDTIMESERIES_DIRECTION, cimBidTimeSeries.Direction));
				}
				if (cimBidTimeSeries.DivisibleHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BIDTIMESERIES_DIVISIBLE, cimBidTimeSeries.Divisible));
				}
				if (cimBidTimeSeries.LinkedBidsIdentificationHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BIDTIMESERIES_LBI, cimBidTimeSeries.LinkedBidsIdentification));
				}
			}
		}
		public static void PopulateMeasurementPointProperties(FTN.MeasurementPoint cimMeasurementPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimMeasurementPoint != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimMeasurementPoint, rd);

				
				if (cimMeasurementPoint.TimeSeriesHasValue)
				{
					long gid = importHelper.GetMappedGID(cimMeasurementPoint.TimeSeries.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimMeasurementPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimMeasurementPoint.ID);
						report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimMeasurementPoint.TimeSeries.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.MEASUREMENTPOINT_TIMESERIES, gid));
				}
			}
		}

		#endregion Populate ResourceDescription

		#region Enums convert
		private static MarketRoleKind GetDMSMarketRoleKind(FTN.MarketRoleKind roleType)
		{
			switch (roleType)
			{
				case FTN.MarketRoleKind.BalanceResponsibleParty:
					return MarketRoleKind.BalanceResponsibleParty;
				case FTN.MarketRoleKind.BalanceSupplier:
					return MarketRoleKind.BalanceSupplier;
				case FTN.MarketRoleKind.balancingAuthority:
					return MarketRoleKind.balancingAuthority;
				case FTN.MarketRoleKind.BillingAgent:
					return MarketRoleKind.BillingAgent;
				case FTN.MarketRoleKind.BlockEnergyTrader:
					return MarketRoleKind.BlockEnergyTrader;
				case FTN.MarketRoleKind.CapacityCoordinator:
					return MarketRoleKind.CapacityCoordinator;
				case FTN.MarketRoleKind.CapacityTrader:
					return MarketRoleKind.CapacityTrader;
				case FTN.MarketRoleKind.competitiveRetailer:
					return MarketRoleKind.competitiveRetailer;
				case FTN.MarketRoleKind.complianceMonitor:
					return MarketRoleKind.complianceMonitor;
				case FTN.MarketRoleKind.Consumer:
					return MarketRoleKind.Consumer;
				case FTN.MarketRoleKind.ConsumptionResponsibleParty:
					return MarketRoleKind.ConsumptionResponsibleParty;
				case FTN.MarketRoleKind.ControlAreaOperator:
					return MarketRoleKind.ControlAreaOperator;
				case FTN.MarketRoleKind.ControlBlockOperator:
					return MarketRoleKind.ControlBlockOperator;
				case FTN.MarketRoleKind.CoordinationCenterOperator:
					return MarketRoleKind.CoordinationCenterOperator;
				case FTN.MarketRoleKind.distributionProvider:
					return MarketRoleKind.distributionProvider;
				case FTN.MarketRoleKind.energyServiceConsumer:
					return MarketRoleKind.energyServiceConsumer;
				case FTN.MarketRoleKind.generatorOperator:
					return MarketRoleKind.generatorOperator;
				case FTN.MarketRoleKind.generatorOwner:
					return MarketRoleKind.generatorOwner;
				case FTN.MarketRoleKind.GridAccessProvider:
					return MarketRoleKind.GridAccessProvider;
				case FTN.MarketRoleKind.GridOperator:
					return MarketRoleKind.GridOperator;
				case FTN.MarketRoleKind.ImbalanceSettlementResponsible:
					return MarketRoleKind.ImbalanceSettlementResponsible;
				case FTN.MarketRoleKind.interchangeAuthority:
					return MarketRoleKind.interchangeAuthority;
				case FTN.MarketRoleKind.InterconnectionTradeResponsible:
					return MarketRoleKind.InterconnectionTradeResponsible;
				case FTN.MarketRoleKind.loadServingEntity:
					return MarketRoleKind.loadServingEntity;
				case FTN.MarketRoleKind.MarketInformationAggregator:
					return MarketRoleKind.MarketInformationAggregator;
				case FTN.MarketRoleKind.MarketOperator:
					return MarketRoleKind.MarketOperator;
				case FTN.MarketRoleKind.MeterAdministrator:
					return MarketRoleKind.MeterAdministrator;
				case FTN.MarketRoleKind.MeteredDataAggregator:
					return MarketRoleKind.MeteredDataAggregator;
				case FTN.MarketRoleKind.MeteredDataCollector:
					return MarketRoleKind.MeteredDataCollector;
				case FTN.MarketRoleKind.MeteredDataResponsible:
					return MarketRoleKind.MeteredDataResponsible;
				case FTN.MarketRoleKind.MeteringPointAdministrator:
					return MarketRoleKind.MeteringPointAdministrator;
				case FTN.MarketRoleKind.MeterOperator:
					return MarketRoleKind.MeterOperator;
				case FTN.MarketRoleKind.MOLResponsible:
					return MarketRoleKind.MOLResponsible;
				case FTN.MarketRoleKind.NominationValidator:
					return MarketRoleKind.NominationValidator;
				case FTN.MarketRoleKind.PartyConnectedToTheGrid:
					return MarketRoleKind.PartyConnectedToTheGrid;
				case FTN.MarketRoleKind.planningAuthority:
					return MarketRoleKind.planningAuthority;
				case FTN.MarketRoleKind.Producer:
					return MarketRoleKind.Producer;
				case FTN.MarketRoleKind.ProductionResponsibleParty:
					return MarketRoleKind.ProductionResponsibleParty;
				case FTN.MarketRoleKind.purchasingSellingEntity:
					return MarketRoleKind.purchasingSellingEntity;
				case FTN.MarketRoleKind.ReconciliationAccountable:
					return MarketRoleKind.ReconciliationAccountable;
				case FTN.MarketRoleKind.ReconciliationResponsible:
					return MarketRoleKind.ReconciliationResponsible;
				case FTN.MarketRoleKind.reliabilityAuthority:
					return MarketRoleKind.reliabilityAuthority;
				case FTN.MarketRoleKind.ReserveAllocator:
					return MarketRoleKind.ReserveAllocator;
				case FTN.MarketRoleKind.resourcePlanner:
					return MarketRoleKind.resourcePlanner;
				case FTN.MarketRoleKind.ResourceProvider:
					return MarketRoleKind.ResourceProvider;
				case FTN.MarketRoleKind.SchedulingCoordinator:
					return MarketRoleKind.SchedulingCoordinator;
				case FTN.MarketRoleKind.standardsDeveloper:
					return MarketRoleKind.standardsDeveloper;
				case FTN.MarketRoleKind.SystemOperator:
					return MarketRoleKind.SystemOperator;
				case FTN.MarketRoleKind.TradeResponsibleParty:
					return MarketRoleKind.TradeResponsibleParty;
				case FTN.MarketRoleKind.TransmissionCapacityAllocator:
					return MarketRoleKind.TransmissionCapacityAllocator;
				case FTN.MarketRoleKind.transmissionOperator:
					return MarketRoleKind.transmissionOperator;
				case FTN.MarketRoleKind.transmissionOwner:
					return MarketRoleKind.transmissionOwner;
				case FTN.MarketRoleKind.transmissionPlanner:
					return MarketRoleKind.transmissionPlanner;
				case FTN.MarketRoleKind.transmissionServiceProvider:
					return MarketRoleKind.transmissionServiceProvider;
				default: return MarketRoleKind.transmissionServiceProvider;
			}
		}
		#endregion Enums convert
	}
}
