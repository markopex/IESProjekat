using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportMarketRole();
			ImportMarketParticipant();
			ImportAuctions();
			ImportReasons();
			ImportPoints();
			ImportBidTimeSeriess();
			ImportMeasurementPoints();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		private void ImportMarketRole()
		{
			SortedDictionary<string, object> cimMarketRoles = concreteModel.GetAllObjectsOfType("FTN.MarketRole");
			if (cimMarketRoles != null)
			{
				foreach (KeyValuePair<string, object> cimMarketRolePair in cimMarketRoles)
				{
					FTN.MarketRole cimMarketRole = cimMarketRolePair.Value as FTN.MarketRole;

					ResourceDescription rd = CreateMarketRoleResourceDescription(cimMarketRole);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("MarketRole ID = ").Append(cimMarketRole.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("MarketRole ID = ").Append(cimMarketRole.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateMarketRoleResourceDescription(FTN.MarketRole cimMarketRole)
		{
			ResourceDescription rd = null;
			if (cimMarketRole != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MARKETROLE, importHelper.CheckOutIndexForDMSType(DMSType.MARKETROLE));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimMarketRole.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateMarketRoleProperties(cimMarketRole, rd);
			}
			return rd;
		}
		
		private void ImportMarketParticipant()
		{
			SortedDictionary<string, object> cimMarketParticipants = concreteModel.GetAllObjectsOfType("FTN.Location");
			if (cimMarketParticipants != null)
			{
				foreach (KeyValuePair<string, object> cimMarketParticipantPair in cimMarketParticipants)
				{
					FTN.MarketParticipant cimMarketParticipant = cimMarketParticipantPair.Value as FTN.MarketParticipant;

					ResourceDescription rd = CreateLocationResourceDescription(cimMarketParticipant);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("MarketParticipant ID = ").Append(cimMarketParticipant.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("MarketParticipant ID = ").Append(cimMarketParticipant.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateLocationResourceDescription(FTN.MarketParticipant cimLocation)
		{
			ResourceDescription rd = null;
			if (cimLocation != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MARKETPRINCIPANT, importHelper.CheckOutIndexForDMSType(DMSType.MARKETPRINCIPANT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimLocation.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateMarketParticipantProperties(cimLocation, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportAuctions()
		{
			SortedDictionary<string, object> cimAuctions = concreteModel.GetAllObjectsOfType("FTN.Auction");
			if (cimAuctions != null)
			{
				foreach (KeyValuePair<string, object> cimAuctionPair in cimAuctions)
				{
					FTN.Auction cimAuction = cimAuctionPair.Value as FTN.Auction;

					ResourceDescription rd = CreateAuctionResourceDescription(cimAuction);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Auction ID = ").Append(cimAuction.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Auction ID = ").Append(cimAuction.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateAuctionResourceDescription(FTN.Auction cimAuction)
		{
			ResourceDescription rd = null;
			if (cimAuction != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.AUCTION, importHelper.CheckOutIndexForDMSType(DMSType.AUCTION));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimAuction.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateAuctionProperties(cimAuction, rd);
			}
			return rd;
		}

		private void ImportReasons()
		{
			SortedDictionary<string, object> cimReasons = concreteModel.GetAllObjectsOfType("FTN.Reason");
			if (cimReasons != null)
			{
				foreach (KeyValuePair<string, object> cimReasonPair in cimReasons)
				{
					FTN.Reason cimReason = cimReasonPair.Value as FTN.Reason;

					ResourceDescription rd = CreateReasonResourceDescription(cimReason);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Reason ID = ").Append(cimReason.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Reason ID = ").Append(cimReason.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateReasonResourceDescription(FTN.Reason cimReason)
		{
			ResourceDescription rd = null;
			if (cimReason != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REASON, importHelper.CheckOutIndexForDMSType(DMSType.REASON));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimReason.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateReasonProperties(cimReason, rd);
			}
			return rd;
		}

		private void ImportPoints()
		{
			SortedDictionary<string, object> cimPoints = concreteModel.GetAllObjectsOfType("FTN.Point");
			if (cimPoints != null)
			{
				foreach (KeyValuePair<string, object> cimPointPair in cimPoints)
				{
					FTN.Point cimPoint = cimPointPair.Value as FTN.Point;

					ResourceDescription rd = CreatePointResourceDescription(cimPoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Point ID = ").Append(cimPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Point ID = ").Append(cimPoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePointResourceDescription(FTN.Point cimPoint)
		{
			ResourceDescription rd = null;
			if (cimPoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POINT, importHelper.CheckOutIndexForDMSType(DMSType.POINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPoint.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulatePointProperties(cimPoint, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportBidTimeSeriess()
		{
			SortedDictionary<string, object> cimBidTimeSeriess = concreteModel.GetAllObjectsOfType("FTN.BidTimeSeries");
			if (cimBidTimeSeriess != null)
			{
				foreach (KeyValuePair<string, object> cimBidTimeSeriesPair in cimBidTimeSeriess)
				{
					FTN.BidTimeSeries cimBidTimeSeries = cimBidTimeSeriesPair.Value as FTN.BidTimeSeries;

					ResourceDescription rd = CreateBidTimeSeriesResourceDescription(cimBidTimeSeries);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("BidTimeSeries ID = ").Append(cimBidTimeSeries.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("BidTimeSeries ID = ").Append(cimBidTimeSeries.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateBidTimeSeriesResourceDescription(FTN.BidTimeSeries cimBidTimeSeries)
		{
			ResourceDescription rd = null;
			if (cimBidTimeSeries != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BIDTIMESERIES, importHelper.CheckOutIndexForDMSType(DMSType.BIDTIMESERIES));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimBidTimeSeries.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateBidTimeSeiesProperties(cimBidTimeSeries, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportMeasurementPoints()
		{
			SortedDictionary<string, object> cimMeasurementPoints = concreteModel.GetAllObjectsOfType("FTN.MeasurementPoint");
			if (cimMeasurementPoints != null)
			{
				foreach (KeyValuePair<string, object> cimMeasurementPointPair in cimMeasurementPoints)
				{
					FTN.MeasurementPoint cimMeasurementPoint = cimMeasurementPointPair.Value as FTN.MeasurementPoint;

					ResourceDescription rd = CreateMeasurementPointResourceDescription(cimMeasurementPoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("MeasurementPoint ID = ").Append(cimMeasurementPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("MeasurementPoint ID = ").Append(cimMeasurementPoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateMeasurementPointResourceDescription(FTN.MeasurementPoint cimMeasurementPoint)
		{
			ResourceDescription rd = null;
			if (cimMeasurementPoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MEASUREMENTPOINT, importHelper.CheckOutIndexForDMSType(DMSType.MEASUREMENTPOINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimMeasurementPoint.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateMeasurementPointProperties(cimMeasurementPoint, rd, importHelper, report);
			}
			return rd;
		}
		#endregion Import
	}
}

