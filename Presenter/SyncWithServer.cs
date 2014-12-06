using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using PVK.Data;
using NLog;
using System.Xml.Linq;
using System.Xml;

namespace PVK.Control.Presenter
{
	class SyncWithServer
	{
		public string _url = "http://87.229.160.69:8080/apex/UPDATE_WEIGHING";
		public string _login;
		public string _pass;
		public int timeOut = 10000000;
        public HttpWebRequest req = null;
        public WebResponse resp = null;
        public Stream stream = null;
		private byte[] byteArray;
		 private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public  SyncWithServer()
		{
			
		}

		public void Synchronize(List<PVK_WEIGHING> _weigings,List<PVK_MODEL_VEHICLE>_models,List<PVK_ATTR_VALUES>_attrValues,List<PVK_PERMISSION_AXIS_LOAD>_permission,List<PVK_DAMAGE_TS_VALUE>_damage)
		{
                // get response    REQ  := UTL_HTTP.BEGIN_REQUEST(VURL || 'GET_SERTIFICATE_UUGADN', 'POST', 'HTTP/1.1');
			string responseFromServer = "";
			try
			{
                _url = _attrValues.FirstOrDefault(t => t.Abbr == "AddressServer").Value;
					WebRequest request = WebRequest.Create (_url );
					// Set the Method property of the request to POST.
					request.Method = "POST";
					request.ContentType = "application/x-www-form-urlencoded";
					//request.ContentType = "application/text";
					byteArray = new byte[4] {1,2,3,4};
					string test = "UPDATE_WEIGHINGS=" +  HttpUtility.UrlEncode(GetXmlWeghings(_weigings,_models));
					request.Timeout = timeOut;
					Stream dataStream ;//= request.GetRequestStream ();
					StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());	
					//dataStream.Write (test);
					requestWriter.Write(test);
					//dataStream.Close ();
					requestWriter.Close();
					 resp = request.GetResponse();
					Stream data = resp.GetResponseStream();
					StreamReader reader = new StreamReader (data);
					responseFromServer = reader.ReadToEnd ();
					reader.Close ();
					resp.Close();
					GetListAttributes(responseFromServer,_attrValues,_permission,_damage);
					
					//dataStream.Close ();
			}
			catch (Exception ex)
			{
				_logger.TraceException (responseFromServer, ex);
				throw ex;
			}  
		}

		public string GetXmlWeghings(List<PVK_WEIGHING> weihings,List<PVK_MODEL_VEHICLE> models)
		{
			try
			{
				   string xml;
					XElement xERoot = new XElement("ROOT");
					XElement xEWeihings = new XElement("WEIGHINGS");
					weihings.ForEach(user => { 
						XElement xERows = new XElement("ROWS");
						xERows.Add(new XElement("ID", user.Id));
						xERows.Add(new XElement("DATE_WEIGHING", user.DateWeighing.ToString("dd.MM.yyyy HH:mm:ss")));
						xERows.Add(new XElement("PVK_ID", user.Pvk == null ? "" : user.Pvk.Nomer));
						xERows.Add(new XElement("OWNER_VEHICLE", user.OwnerVehicle));
						xERows.Add(new XElement("MODEL_VEHICLE", user.ModelVehicle == null ?  null : (string)user.ModelVehicle.Model));
						xERows.Add(new XElement("VEHICLE_REG_NUMBER",  user.VehicleRegNumber));
						xERows.Add(new XElement("VEHICLE_PODVESKA", user.VehiclePodveska));
                        xERows.Add(new XElement("MODEL_TRAILER", user.ModelTrailer == null ? null : (string)user.ModelTrailer.Model));
						xERows.Add(new XElement("TRAILER_PODVESKA", user.TrailerPodveska));
						xERows.Add(new XElement("TRAILER_REG_NUMBER", user.TrailerRegNumber));
						xERows.Add(new XElement("ROUTE_NAME", user.RouteName));
						xERows.Add(new XElement("COUNT_AXIS_VEHICLE", user.CountAxisVehicle));
						xERows.Add(new XElement("COUNT_AXIS_TRAILER", user.CountAxisTrailer));
						xERows.Add(new XElement("PHOTO", user.Photo != null ?  new XCData( Convert.ToBase64String(user.Photo)) : new XCData("")));
						//xERows.Add(new XElement("PHOTO",new XCData( user.Photo )));
						xERows.Add(new XElement("ACT_NOMER", user.ActNomer));
						xERows.Add(new XElement("ACT_NARUSHENIE_ID", user.ActNarushenie == null ? null : (int?) user.ActNarushenie.Id));
						xERows.Add(new XElement("ACT_CARGO_TYPE_ID", user.ActCargo == null ? null : (int?)user.ActCargo.Id));
						xERows.Add(new XElement("ACT_ROUTE_TYPE", user.ActRoute));
						xERows.Add(new XElement("ACT_EXPLANATION_DRIVER", user.ActExplanationDriver));
						xERows.Add(new XElement("ACT_DRIVER", user.ActDriver));
						xERows.Add(new XElement("ACT_ASSECCORY", user.ActAsseccory));
						xERows.Add(new XElement("ACT_PROTOKOL_NUMBER", user.ActProtokolNumber));
						xERows.Add(new XElement("ACT_PROTOCOL_DATE", user.ActProtocol_date.ToString("dd.MM.yyyy HH:mm:ss")));
						xERows.Add(new XElement("ACT_SUBSCRIBE", user.ActSubscribe));
						xERows.Add(new XElement("ACT_DAMADGE", user.ActDamadge));
						xERows.Add(new XElement("ACT_DETAILS_PAY_ID", user.ActDetailsPay == null ? null : (int?)user.ActDetailsPay.Id));	  
						xERows.Add(new XElement("PVK_ADDRESS", user.PvkAdress));
						xERows.Add(new XElement("ACT_INSPECTOR_PVK", user.ActInspectorPvk));
						xERows.Add(new XElement("ACT_SPECIALIST_PVK", user.ActSpecialistPvk));
						xERows.Add(new XElement("ACT_INSPECTOR_GIBDD", user.ActInspectorGIBDD));
						xERows.Add(new XElement("ACT_NOTE", user.ActNote));
  						for (int i = 0; i < 24; i++)
						 {
							 xERows.Add(new XElement("DISTANCE_AXIS_"+(i+1).ToString(), user.DistanceAxis[i]));
						 }
						for (int i = 0; i < 24; i++)
						 {		
							xERows.Add(new XElement("LOAD_AXIS_"+(i+1).ToString(), user.LoadAxis[i]));
						 }
						for (int i = 0; i < 24; i++)
						 {
							 xERows.Add(new XElement("FACT_LOAD_AXIS_"+(i+1).ToString(), user.FactLoadAxis[i]));
						 }
						xEWeihings.Add(xERows);
					});
					xERoot.Add(xEWeihings);
					/*XElement xEModels = new XElement("MODELS");
					models.Where(t => t.Id > 1).ToList().ForEach(user => { 
						XElement xERows = new XElement("ROWS");
						xERows.Add(new XElement("ID", user.Id));
						xERows.Add(new XElement("MARK", user.Mark));
						xERows.Add(new XElement("MODEL", user.Model));
						xERows.Add(new XElement("TYPE", user.Type));
						xERows.Add(new XElement("COUNT_AXIS", user.CountAxis));
						xERows.Add(new XElement("AXIS_LENGTH_1", user.AxisLength1));
						xERows.Add(new XElement("AXIS_LENGTH_2", user.AxisLength2));
						xERows.Add(new XElement("AXIS_LENGTH_3", user.AxisLength3));
						xERows.Add(new XElement("AXIS_LENGTH_4", user.AxisLength4));
						xERows.Add(new XElement("AXIS_LENGTH_5", user.AxisLength5));
						xERows.Add(new XElement("AXIS_LENGTH_6", user.AxisLength6));
						xERows.Add(new XElement("AXIS_LENGTH_7", user.AxisLength7));
						xERows.Add(new XElement("AXIS_LENGTH_8", user.AxisLength8));
						xERows.Add(new XElement("AXIS_LENGTH_9", user.AxisLength9));
						xERows.Add(new XElement("AXIS_LENGTH_10", user.AxisLength10));
						xERows.Add(new XElement("AXIS_LENGTH_11", user.AxisLength11));
						xERows.Add(new XElement("AXIS_LENGTH_12", user.AxisLength12));
						xERows.Add(new XElement("LINK_LENGTH", user.LinkLength));
						xEModels.Add(xERows);
					});
					xERoot.Add(xEModels);*/
					return xERoot.ToString();
			}
			catch (Exception ex)
			{
				_logger.TraceException("Ошибка", ex);
				return "";
			}
		}

		public string GetListAttributes(string xml, List<PVK_ATTR_VALUES> _attr,List<PVK_PERMISSION_AXIS_LOAD> _permission,List<PVK_DAMAGE_TS_VALUE> _damage)
		{
			try
			{
				xml = xml.Replace("&lt;", "<").Replace("&gt;", ">");
				var permisssion = from c in XElement.Parse(xml).Elements("PERMISSION")
							select c;
				foreach (var item in permisssion)
				{
					var _permissionNew = new PVK_PERMISSION_AXIS_LOAD() { Id = Convert.ToInt32(item.Element("ID").Value), IdAdd = item.Element("ID_ADD").Value, DistanceMin = Convert.ToDouble(item.Element("DISTANCE_MIN").Value == ""?"0": item.Element("DISTANCE_MIN").Value.Replace(".",",")), DistanceMax = Convert.ToDouble(item.Element("DISTANCE_MAX").Value == "" ? "0" : item.Element("DISTANCE_MAX").Value.Replace(".",",")), PermissionValue1 = Convert.ToDouble(item.Element("PERMISSION_VALUE_1").Value == "" ? "0" :item.Element("PERMISSION_VALUE_1").Value.Replace(".",",")), PermissionValue2 = Convert.ToDouble(item.Element("PERMISSION_VALUE_2").Value == "" ? "0" :item.Element("PERMISSION_VALUE_2").Value.Replace(".",",")), PermissionValue3 = Convert.ToDouble(item.Element("PERMISSION_VALUE_3").Value == "" ? "0" : item.Element("PERMISSION_VALUE_3").Value.Replace(".",",")) };
					if (_permission.Where(t => t.Id == _permissionNew.Id).FirstOrDefault() == null)
					{
						_permissionNew.Id = 0;
						_permission.Add(_permissionNew);
					}
					else
					{
						var d = _permission.Where(t => t.Id == _permissionNew.Id).First();
						d.IdAdd = _permissionNew.IdAdd;
						d.DistanceMin = _permissionNew.DistanceMin;
						d.DistanceMax = _permissionNew.DistanceMax;
						d.PermissionValue1 = _permissionNew.PermissionValue1;
						d.PermissionValue2 = _permissionNew.PermissionValue2;
						d.PermissionValue3 = _permissionNew.PermissionValue3;
					}
				}
				var damage = from c in XElement.Parse(xml).Elements("DAMAGE")
						        select c;
				foreach (var item in damage)
				{
                    var _damageNew = new PVK_DAMAGE_TS_VALUE() { Id = Convert.ToInt32(item.Element("ID").Value), IdReCalc = new PVK_PERIOD_CALCULATE_DAMAGE_TS() { Id = Convert.ToInt32(item.Element("ID_RECALC").Value) }, OverMin = Convert.ToDouble(item.Element("OVER_MIN").Value.Replace(".", ",")), OverMax = Convert.ToDouble(item.Element("OVER_MAX").Value.Replace(".", ",")), Type = item.Element("TYPE").Value, Factor = item.Element("FACTOR").Value, Formula = item.Element("FORMULA").Value };
					if (_damage.Where(t => t.Id == _damageNew.Id).FirstOrDefault() == null)
					{
						_damageNew.Id = 0;
						_damage.Add(_damageNew);
					}
					else
					{
						var d = _damage.Where(t => t.Id == _damageNew.Id).First();
						d.IdReCalc = _damageNew.IdReCalc;
						d.OverMin = _damageNew.OverMin;
						d.OverMax = _damageNew.OverMax;
						d.Type = _damageNew.Type;
						d.Factor = _damageNew.Factor;
						d.Formula = _damageNew.Formula;
					}
				}
				return "";
			}
			catch (Exception ex)
			{
				_logger.TraceException(xml,ex);
				throw ex;
				return "";
			}

		}
	}
}



