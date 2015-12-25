using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DisplayController;

namespace Displays
{
    class LEDDisplay
    {
        public struct TNetParam
        {
            public string Password;
            public string IPAddr;
            public short UDPPort;                       
        }

        //public struct TComParam
        //{
        //    public int ComPort;
        //    public int Baudrate;
        //    public int LedNum;
        //}

        private bool _bUse;
        private bool _SCL2008;
        private short _DevID;
        private bool _Net;
        private TNetParam _NetParam;
      //  private TComParam _ComParam;
        private int _Timeout;
        private int _Retry;
        private short _Width;
        private short _Height;

        public LEDDisplay()
        {       	
        	_NetParam = new TNetParam();
      //  	_ComParam = new TComParam();
        	this._bUse				= false;
            this._SCL2008 			= true;
            this._DevID 			= 13;
            this._Net 				= true;
            this._NetParam.Password = "scl2008";
            this._NetParam.IPAddr 	= "192.168.100.220";
            this._NetParam.UDPPort 	= 27694;
            //this._ComParam.ComPort 	= 7;
            //this._ComParam.Baudrate = 38400;
            //this._ComParam.LedNum 	= 0;
            this._Timeout 			= 2;
            this._Retry 			= 4;
            this._Width				= 128;
            this._Height			= 32;
        }
    
        public bool bUse
        {
        	get
        	{
        		return this._bUse;
        	}
        	
        	set
        	{
        		this._bUse = value;
        	}
        }
        
        public bool SCL2008
        {
        	get
        	{
        		return this._SCL2008;
        	}
        	
        	set
        	{
        		this._SCL2008 = value;
        	}
        }
        
        public short DevID
        {
        	get
        	{
        		return this._DevID;
        	}
        	
        	set
        	{
        		this._DevID = value;
        	}
        }

        public bool Net
        {
        	get
        	{
        		return this._Net;
        	}
        	
        	set
        	{
        		this._Net = value;
        	}
        }

        
        public TNetParam NetParam
        {
        	get
        	{
        		return this._NetParam;
        	}
        	
        	set
        	{
        		this._NetParam = value;
        	}
        }     


        //public TComParam ComParam
        //{
        //    get
        //    {
        //        return this._ComParam;
        //    }
        	
        //    set
        //    {
        //        this._ComParam = value;
        //    }
        //}
        
        public int Timeout
        {
        	get
        	{
        		return this._Timeout;
        	}
        	
        	set
        	{
        		this._Timeout = value;
        	}
        }

        public int Retry
        {
        	get
        	{
        		return this._Retry;
        	}
        	
        	set
        	{
        		this._Retry = value;
        	}
        }   

        public short Width
        {
        	get
        	{
        		return this._Width;
        	}
        	
        	set
        	{
        		this._Width = value;
        	}
        }

        public short Height
        {
        	get
        	{
        		return this._Height;
        	}
        	
        	set
        	{
        		this._Height = value;
        	}
        }          
      
        
        public bool Open()
        {
            bool bOK = false;

            if (!this._bUse)
            {
                //if (Net)
                //{
	                bOK = DisplayController.SCL2008.SCL_NetInitial(this._DevID,
	                                                               this._NetParam.Password,
	                                                               this._NetParam.IPAddr,
	                                                               this._Timeout,
	                                                               this._Retry,
	                                                               this._NetParam.UDPPort,
                                                                   this._SCL2008);
                //}
                //else
                //{
                //    bOK = DisplayController.SCL2008.SCL_ComInitial(this._DevID,
                //                                                   this._ComParam.ComPort,
                //                                                   this._ComParam.Baudrate,
                //                                                   this._ComParam.LedNum,
                //                                                   this._Timeout,
                //                                                   this._Retry,
                //                                                   this._SCL2008);
                //}
	
	            this._bUse = bOK;
            }
            else
            {
            	bOK = true;
            }
            
            return bOK;
        }

        public bool Close()
        {     
			bool bOK = false;
			
        	if (this._bUse)
        	{
            	bOK = DisplayController.SCL2008.SCL_Close(this._DevID);
            	if (bOK)
            	{
            		this._bUse = false;
            	}
        	}

            return bOK;
        }

        public bool Reset()
        {
            return DisplayController.SCL2008.SCL_Reset(this._DevID);
        }     
        
        public bool Replay(short DevID, int Drv, int Index)
        {
        	return DisplayController.SCL2008.SCL_Replay(DevID, Drv, Index);
        }          

        public bool ShowString(short DevID, DisplayController.SCL2008.TextInfoType TextInfo, string Str)
        {
        	return DisplayController.SCL2008.SCL_ShowString(DevID, ref TextInfo.Left_, Str);
        }
        
        public bool SendFile(short DevID, int DrvNo, string Path, string FileName)
        {
        	return DisplayController.SCL2008.SCL_SendFile(DevID, DrvNo, Path, FileName);
        }
        
        public bool RemoveFile(short DevID, int DrvNo, string FileName)
        {
        	return DisplayController.SCL2008.SCL_RemoveFile(DevID, DrvNo, FileName);
        }        
    }
}