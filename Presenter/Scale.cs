using System;
using System.Text;
using PVK.Control.View;
using TRLibrary.Commons;
using TRLibrary2;
using System.Data;
using PVK.Data;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using System.ComponentModel;

namespace PVK.Control
{
    [TypeConverter(typeof (EnumDescConverter))]
    public enum EnmProtocol
    {
        [Description("Неопределенное состояние")] Undefined = 0,
        [Description("Фрмат передачи №1")] Protocol1 = 1,
        [Description("Фрмат передачи №2")] Protocol2 = 2
    }

    public class Scale
	{
		SerialPort com;
        SerialPort com2;
		List<byte> bBuffer = new List<byte>();																								  
        List<byte> bBuffer2 = new List<byte>();	
		string sBuffer = String.Empty;
		public event EventHandler<EventArgs> UpdateDisplayValue;
        public event EventHandler<EventArgs> UpdateWeightAxis;
		public WeighingForm WfForm;
		public double currValue = 0.0;
		public double currValue1 = 0.0;
		public double currValue2 = 0.0;
        private EnmProtocol _protocol = EnmProtocol.Undefined;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private bool getCurrValue1;
		private bool getCurrValue2;
        private double lastWeigth = 0.0;
        private bool _bIncreaseWeight = false;

        private const double fluctuation = 5.0; // погрешность 

		public Scale()
		{
			try
			{

				 com = new SerialPort(SerialPort.GetPortNames()[0],9600, Parity.None, 8, StopBits.One);
				 com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
				 com.Open();																								
				Console.WriteLine("Com port {0} opened.",com.PortName);
			}
			catch (Exception ex)
			{
				 _logger.Error(ex.Message); 
			}
								 
		}

		~Scale()
		{
			try
			{
				if (com != null)
				{
					com.Close();
					Console.WriteLine("Com port {0} closed.", com.PortName);
				}
			}
			catch (Exception ex)
			{
				_logger.TraceException(ex.Message,ex);
			}
						 
		}

		public void FreeComPort()
		{
			try
			{
				if (com != null)
				{
					com.Close();
					com = null;
                    com2.Close();
                    com2 = null;
				}
			}
			catch (Exception ex)
			{
				_logger.TraceException(ex.Message,ex);
			}
		}

        public Scale(string portNumber1, string speed1, string dataBit1, string parity1, string stopBit1) 
		{
			try
			{
			    _protocol = EnmProtocol.Protocol1;
				string [] ports = SerialPort.GetPortNames();
				if (ports.Length > 0 && portNumber1 != "0")
				{

                    com = new SerialPort("COM" + portNumber1, Convert.ToInt32(speed1), Parity.None, Convert.ToInt32(dataBit1), (StopBits)Convert.ToInt32(stopBit1));
					com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);	
					if (!com.IsOpen)
						com.Open();																								
					Console.WriteLine("Com port {0} opened.",com.PortName);					
				}
				else
					_logger.Error("Выбранный ком порт для весов не существует");

			}
			catch (Exception ex)
			{
				 _logger.Error(ex.Message); 
			}										 
		}

        public Scale(string portNumber1, string speed1, string dataBit1, string parity1, string stopBit1, string portNumber2, string speed2, string dataBit2, string parity2, string stopBit2)
        {
            try
            {
                _protocol = EnmProtocol.Protocol2;
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length > 0 && (portNumber1 != "0" || portNumber2 != null))
                {
                    try
                    {
                        com = new SerialPort("COM" + portNumber1, Convert.ToInt32(speed1), Parity.None, Convert.ToInt32(dataBit1), (StopBits)Convert.ToInt32(stopBit1));
                        com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
                        if (!com.IsOpen)
                            com.Open();
                        Console.WriteLine("Com port {0} opened.", com.PortName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Trace(ex.Message);
                    }}
                if (ports.Length > 0 && (portNumber2 !="0" || portNumber2!=null)){
                    try
                    {
                        com2 = new SerialPort("COM" + portNumber2, Convert.ToInt32(speed2), Parity.None, Convert.ToInt32(dataBit2), (StopBits)Convert.ToInt32(stopBit2));
                        com2.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived2);
                        if (!com2.IsOpen)com2.Open();
                        Console.WriteLine("com2 port {0} opened.", com2.PortName);
                    }catch (Exception ex)
                    {
                        _logger.Trace(ex.Message);
                    }
                }
                else
                    _logger.Error("Выбранный ком порт для весов не существует");

            }
            catch (Exception ex)
            {
                _logger.Trace(ex.Message);
            }
        }
		 
		void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
		    try
		    {
                // Use either the binary OR the string technique (but not both)	   
                // Buffer and process binary data	 
                while (com.BytesToRead > 0)
                {
                    bBuffer.Add((byte)com.ReadByte());

                }
                //sBuffer = com.ReadExisting();
                //_logger.Trace("Приняты данные: " +sBuffer);
                ProcessBuffer(bBuffer, 1);
                // Buffer string data					  
                //sBuffer += com.ReadExisting();			 
                //ProcessBuffer(sBuffer);
		    }
		    catch (Exception ex)
		    {
                _logger.TraceException(" " , ex);
		    }			  
		}

        void com_DataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Use either the binary OR the string technique (but not both)	   
                // Buffer and process binary data	 
                while (com2.BytesToRead > 0)
                {
                    bBuffer2.Add((byte)com2.ReadByte());

                }
                //sBuffer = com.ReadExisting();
                //_logger.Trace("Приняты данные: " +sBuffer);
                ProcessBuffer(bBuffer2, 2);
                // Buffer string data					  
                //sBuffer += com.ReadExisting();			 
                //ProcessBuffer(sBuffer);	
            }
            catch (Exception ex)
            {
                _logger.TraceException(" " , ex);
            }
 		  
        }

		private void ProcessBuffer(string sBuffer)		   
		{
			try
			{
                if (_protocol == EnmProtocol.Protocol1)
			    {
                    			    // Look in the byte array for useful information 
				// then remove the useful data from the buffer
				//var str = Encoding.ASCII.GetString(bBuffer.ToArray());
				var  str = sBuffer;
				if (!str.Contains("kg"))
					return;
				string[] strings = str.Split(new string[] { "kg" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var st in strings)
				{
					string[] weigth = st.Split(new string[] { "," }, StringSplitOptions.None);
					if (weigth.Length != 3)
						continue;
					if (weigth[1].Replace(" ", "") == "1")
					{
						currValue1 = Convert.ToDouble(weigth[2].Replace("kg", "").Replace(" ", "").Replace(".", ",")) / 1000;
						getCurrValue1 = true;
					}
					if (weigth[1].Replace(" ", "") == "2")
					{
						currValue2 = Convert.ToDouble(weigth[2].Replace("kg", "").Replace(" ", "").Replace(".", ",")) / 1000;
						getCurrValue2 = true;
					}
					if (getCurrValue1 && getCurrValue2)
					{
						getCurrValue1 = getCurrValue2 = false;
						currValue = currValue1 + currValue2;
						if (UpdateDisplayValue != null)
							UpdateDisplayValue(this, EventArgs.Empty);
                        //if (WfForm != null && WfForm.digitalGauge1 != null)
                        //    WfForm.digitalGauge1.Text = currValue.ToString();
					}
				}
				//bBuffer.Clear();
				sBuffer = "";
			    }
                else if (_protocol == EnmProtocol.Protocol2)
                {

                }
			}
		catch (Exception ex)
			{
				bBuffer.Clear();
				_logger.TraceException(ex.Message, ex);
			}


		}

		private void ProcessBuffer(List<byte> bBuffer, int numberBuffer = 1 )

		{

			try
			{
				// Look in the byte array for useful information 
					// then remove the useful data from the buffer
                if (_protocol == EnmProtocol.Protocol1)
                {
				    var str = Encoding.ASCII.GetString(bBuffer.ToArray());
				    if ( !str.Contains("kg") )
					    return;
				    string[] strings = str.Split(new string[] {"kg"}, StringSplitOptions.RemoveEmptyEntries);
				    foreach ( var st in strings )
				    {
					    string[] weigth = st.Split(new string[] { "," }, StringSplitOptions.None);
					    if ( weigth.Length != 3 )
						    continue;
					    if ( weigth[1].Replace(" ", "") == "1" )
					    {
						    currValue1 = Convert.ToDouble(weigth[2].Replace("kg", "").Replace(" ", "").Replace(".", ",")) / 1000;
						    getCurrValue1 = true;
					    }
					    if ( weigth[1].Replace(" ", "") == "2" )
					    {
						    currValue2 = Convert.ToDouble(weigth[2].Replace("kg", "").Replace(" ", "").Replace(".", ",")) / 1000;
						    getCurrValue2 = true;
					    }
					    if ( getCurrValue1 && getCurrValue2 )
					    {
						    getCurrValue1 = getCurrValue2 = false;
						    currValue = currValue1 + currValue2;
						    if ( UpdateDisplayValue != null )
							    UpdateDisplayValue(this, EventArgs.Empty);
                            //if (WfForm != null && WfForm.digitalGauge1 != null)
                            //    WfForm.digitalGauge1.Text = currValue.ToString();
					    }
				    }
				    bBuffer.Clear();
                }
                else if (_protocol == EnmProtocol.Protocol2)
                {
                    var str = Encoding.ASCII.GetString(bBuffer.ToArray());
				    string[] strings = str.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var st in strings)
                    {
                        try
                        {
                            if (st.Trim() == "")
                                continue;
                            if (numberBuffer == 1)
                            {
                                getCurrValue1 = Double.TryParse(st.Trim().Replace(" ", "").Replace(".", ","), out currValue1);
                                bBuffer.Clear();
                            }
                            if (numberBuffer == 2)
                            {
                                getCurrValue2 = Double.TryParse(st.Trim().Replace(" ", "").Replace(".", ","), out currValue2);
                                bBuffer.Clear();
                            }
                            if (getCurrValue1 && getCurrValue2)
                            {
                                getCurrValue1 = getCurrValue2 = false;
                                currValue = (currValue1 + currValue2)/1000;
                                if (UpdateDisplayValue != null)
                                    UpdateDisplayValue(this, EventArgs.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.TraceException(ex.Message + " !!!!!!!!!!!!!!!!!! !!!!!!!!!!!!!!! ST = " + st, ex);
                        }
                            //if (WfForm != null && WfForm.digitalGauge1 != null)
                            //    WfForm.digitalGauge1.Text = currValue.ToString();
                            //Если увеличивается то все хорошо, если пошел на спад значит уже пора записывать заначение в таблицу
                            //if (currValue < lastWeigth && _bIncreaseWeight)
                            //{
                            //    if (UpdateWeightAxis != null)
                            //        UpdateWeightAxis(this, EventArgs.Empty);
                            //    _bIncreaseWeight = false;
                            //}
                            //else if (currValue < lastWeigth)
                            //    lastWeigth = currValue;
                            //else
                            //{
                            //    _bIncreaseWeight = true;
                            //    lastWeigth = currValue;
                            //}
                    }
                }
			}
			catch (Exception ex)
			{
				bBuffer.Clear();
                _logger.TraceException(ex.Message, ex); 
			}

		}

		public void TestValue(double value)
		{
			// Look in the byte array for useful information 
			// then remove the useful data from the buffer
			currValue1 = value;
			if 	(UpdateDisplayValue!=null)
				UpdateDisplayValue(this,EventArgs.Empty);	
		}
	}
}
