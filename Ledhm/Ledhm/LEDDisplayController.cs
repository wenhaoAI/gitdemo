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

namespace DisplayController
{
    //******************************************************************************
    // 名称: SCL_API
    // 简介: 本模块包括所有SCL_API_Stdcall.dll调用的声明
  public  class SCL2008
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DirItemType
        {
            // 用空格扩展填充的文件名
            public char[] Name;
            // 用空格扩展填充的扩展名
            public char[] Ext;
            // 属性,$00表示普通文件, $10表示子目录
            public byte Attr;
            // 保留
            public byte[] Reserved;
            // 文件创建的时间, bit15-11 时, bit10-5 分, bit4-0 秒/10
            public short CTTime;
            // 文件创建的日期, bit15-9 年-1980, bit8-5 月, bit4-0 日
            public short CTDate;
            // 文件存储位置的簇号
            public short Cluster;
            // 以字节为单位的文件长度
            public int Length;

            public void Initialize()
            {
                Name = new char[8];
                Ext = new char[3];
                Reserved = new byte[10];
            }
        }

        //控制卡运行状态信息
        [StructLayout(LayoutKind.Sequential)]
        public struct RunTimeInfo
        {
            //无关数据
            public byte[] Start;
            //当前节目表中的节目总数
            public short TotalProgCount;
            //正在播放的节目序号
            public short CurrentProg;
            //无关数据
            public short NotUsed1;
            //当前运行的节目表所在的驱动器
            public short DrvProg;
            //SD 卡就绪标记，0：未就绪，1：就绪
            public short SD_OK;
            //无关数据
            public short NotUsed2;
            //湿度采样结果
            public short Humid;
            //温度采样结果，有符号
            public short Temperature;
            //屏体电源开关状态
            public short PowerSwitch;
            //无关数据
            public int NotUsed3;
            //当前播放节目表的索引
            public byte ProgramIndex;
            //当前播放节目表的驱动器
            public byte ProgramDrv;
            //无关数据
            public short[] NotUsed4;
            //每点颜色深度，2：16位色，3：24位色
            public short ColorBytes;
            //控制卡时钟之秒
            public short Second;
            //控制卡时钟之分
            public short Minute;
            //控制卡时钟之时
            public short Hour;
            //控制卡时钟之日
            public short Day;
            //控制卡时钟之月
            public short Month;
            //控制卡时钟之星期
            public short Week;
            //控制卡时钟之年
            public short Year;
            //当前亮度等级
            public short Brightness;
            //无关数据
            public short NotUsed5;
            //串口1收到的8组数据
            public char[,] Com1Data;
            //串口2收到的8组数据
            public char[,] Com2Data;
            //串口3收到的8组数据
            public char[,] Com3Data;
            //无关数据
            public int[] NotUsed6;
            //0:日历时钟芯片读出失败, 1:日历时钟数据有效
            public short TimerOK;
            //0:屏体电源被强行关闭,1:屏体电源被强行打开,2:正在执行自动控制
            public short PowerMode;
            public char[] ProgPath;
            public short[] NotUsed7;
            //SW1的实际状态
            public short SW1;
            //SW2的实际状态
            public short SW2;

            public short[] NotUsed8;
            public void Initialize()
            {
                Start = new byte[30];
                NotUsed4 = new short[7];
                Com1Data = new char[8, 8];
                Com2Data = new char[8, 8];
                Com3Data = new char[8, 8];
                NotUsed6 = new int[24];
                ProgPath = new char[8];
                NotUsed7 = new short[3];
                NotUsed8 = new short[57];
            }
        }

        //文本输出信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct TextInfoType
        { 
            /// <summary>
            /// 区域左上角的水平坐标
            /// </summary>
            public short Left_;
            /// <summary>
            /// 区域左上角的垂直坐标
            /// </summary>
            public short Top_;
            /// <summary>
            /// 区域的宽度
            /// </summary>
            public short Width_;
            /// <summary>
            /// 区域的高度
            /// </summary>
            public short Height_;
            /// <summary>
            /// 颜色
            /// </summary>
            public int Color;
            /// <summary>
            /// 西文字体索引
            /// </summary>
            public short ASCFont;
            /// <summary>
            /// 汉字字体索引
            /// </summary>
            public short HZFont;
            /// <summary>
            /// 输出位置的水平坐标
            /// </summary>
            public short XPos;
            /// <summary>
            /// 输出位置的垂直坐标
            /// </summary>
            public short YPos;
        }

        /// <summary>
        /// 创建Socket，并记录通讯中用到的超时、重试、控制器IP、控制器类型等参数
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号。若初始化成功，对该设备调用通讯函数时必须使用相同的设备编号</param>
        /// <param name="Password">字符串，访问密码</param>
        /// <param name="IP">字符串，控制器的IP地址</param>
        /// <param name="TimeOut">通讯的延时阈值，时间超过该值尚未收到应答则认为通讯失败，单位：秒</param>
        /// <param name="Retry">通讯失败后的总的重试次数</param>
        /// <param name="UDPPort">建立UDP通讯使用的端口</param>
        /// <param name="SCL2008">TRUE表示控制器是SCL2008，FALSE表示控制器是SuperComm</param>
        /// <returns>若创建Socket成功，返回TRUE(非0)，否则返回FALSE(0)(可能是动态链接库管理的256个通讯设备均已占用，或设备编号重复，或UDP端口重复等)</returns>
        [DllImport("SCL_API_Stdcall.dll", CharSet = CharSet.Ansi)]
        public static extern bool SCL_NetInitial(short DevID, string Password, string IP, int TimeOut, int Retry, short UDPPort, bool SCL2008);

        /// <summary>
        /// 初始化串口通讯资源，保存通讯将用到的超时、重试、控制器的编号参数。控制器的串行通讯使用的是8个数据位，1个停止位，无校验的格式。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号。若初始化成功，对该设备调用通讯函数时必须使用相同的设备编号</param>
        /// <param name="ComPort">通讯时计算机使用的串口号，1-255</param>
        /// <param name="Baudrate">串行通讯的波特率，可选值为2400、4800、9600、19200、38400和57600</param>
        /// <param name="LedNum"> 控制器串行通讯地址码，0-254。SCL2008有独立的串行通讯地址码</param>
        /// <param name="TimeOut">通讯的延时阈值，时间超过该值尚未收到应答则认为通讯失败，单位：秒</param>
        /// <param name="Retry">通讯失败后的总的重试次数</param>
        /// <param name="SCL2008"></param>
        /// <returns>若串口初始化成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_ComInitial(short DevID, int ComPort, int Baudrate, int LedNum, int TimeOut, int Retry, bool SCL2008);

        /// <summary>
        /// 关闭通讯，释放与编号相应的硬件资源
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号。</param>
        /// <returns>一般总返回TRUE(非0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Close(short DevID);

        /// <summary>
        /// 设置控制器的IP地址，必须在调用SCL_NetInitial后方能使用本函数
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="sIP">控制器的IP地址</param>
        /// <returns>若相应编号的设备已经初始化，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetRemoteIP(short DevID, string sIP);

        /// <summary>
        /// 设置控制器的串行通讯地址码
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="LedNum">控制器的编号</param>
        /// <returns>若相应编号的设备已经初始化，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetLEDNum(short DevID, int LedNum);

        /// <summary>
        /// 切换控制器类型。在一个通讯设备同时与SCL2008和SuperComm混合通讯时，此函数通知动态链接库按照对应的控制器类型与之通讯或封装数据包。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="b2008">TRUE表示nDevID对应的设备将与SCL2008控制器通讯，FALSE表示将与SuperComm控制器通讯</param>
        /// <returns>若相应编号的设备已经初始化，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_TargetSCL2008(short DevID, bool b2008);


        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_GetLastResult(short DevID);


        /// <summary>
        /// 为获取封装协议的数据包初始化虚拟通讯设备。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="bNet">指明是网络通讯模式还是串口通讯模式；不同的通讯模式，数据包封装得不一样。TRUE：网络模式，FALSE：串口模式；</param>
        /// <param name="bSCL2008">指明是否封装与SCL2008控制器通讯的数据包。FALSE：SuperComm，TRUE：SCL2008；</param>
        /// <returns>若初始化成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_InitForPackage(short DevID, bool bNet, bool bSCL2008);

        /// <summary>
        /// 获取通讯数据包
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Data">指向存储数据包的内存。该空间一般需要定义为1100字节(SuperComm/SCL2008的每个数据包中，有效数据最大不超过1024字节，加上协议头，最大约为1060字节)。调用本函数后，动态链接库将拷贝封装好的数据包到该空间中。</param>
        /// <param name="AnswerCount">存储返回的期望的应答数据的长度。调用本函数后，动态链接库将根据协议给出应答包应有的字节数，便于程序员处理接收过程；</param>
        /// <returns>Data缓冲区中的字节数</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_GetPackage(short DevID, byte Data, int AnswerCount);

        /// <summary>
        /// 检查控制器给回的应答是否正确。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="AnswerCount">接收到的应答的字节数</param>
        /// <param name="Answer">接收到的应答数据</param>
        /// <returns>应答正确返回TRUE(非0)，应答错误返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_CheckAnswer(short DevID, ref int AnswerCount, ref byte Answer);

        /// <summary>
        /// 将本地文件发送到控制器drv驱动器的Path目录下，文件名与计算机中的文件名一致。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="Path">控制器上的子目录，为空表示发送到根目录；若Path不为空，但该子目录不存在，则本函数返回FALSE</param>
        /// <param name="FileName">本地文件名(含路径)，注意本地文件名必须符合8.3的格式，即不得使用长文件名。</param>
        /// <returns>发送成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SendFile(short DevID, int DrvNo, string Path, string FileName);

        /// <summary>
        /// 将控制器上的文件取回到本地磁盘
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="RemoteName">带路径的8.3格式的控制器上的文件名，如“P02\11.xmp”(子目录P02下的文件)，或“11.xmp”(根目录下的文件)；</param>
        /// <param name="LocalName">  本地文件名(含路径)</param>
        /// <returns>取回文件成功，返回文件长度，否则返回-1</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_ReceiveFile(short DevID, int DrvNo, string RemoteName, string LocalName);


        /// <summary>
        /// 将SCL_SendData、SCL_SaveToFile、SCL_Replay三次数据交换做到一个数据交换中。仅适用于SCL2008控制器和小于1088字节的文件。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="Name">含路径的8.3格式的文件名</param>
        /// <param name="Len">保存为文件的以字节为单位的数据长度</param>
        /// <param name="Time_">该文件在计算机上的创建时间(可由SCL_GetFileDosDateTime函数获得)</param>
        /// <param name="Date_">该文件在计算机上的创建时间</param>
        /// <param name="RestartFlag">文件保存后是否重启节目表标记, 0:不重启, 1:重启节目表</param>
        /// <param name="RestartDrv"></param>
        /// <param name="RestartIndex">节目表编号，0-99</param>
        /// <param name="Data">待保存为文件的数据，其字节数为Len参数的值，不得超过1088字节</param>
        /// <returns>文件存盘成功返回TRUE(非0)，失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SendSmallFile(short DevID, int DrvNo, string Name, int Len, int Time_, int Date_, short RestartFlag, short RestartDrv, short RestartIndex, ref byte Data);


        /// <summary>
        /// 删除控制器上的某个文件
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="FileName">带路径的8.3格式的控制器上的文件名，如“P02\11.xmp”(子目录P02下的文件)，或“11.xmp”(根目录下的文件)</param>
        /// <returns>删除文件成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_RemoveFile(short DevID, int DrvNo, string FileName);


        /// <summary>
        /// 在某驱动器的根目录下创建一个子目录
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="Path">“FON”或格式为“Pxx”的子目录名，这里x为0-9数字字符</param>
        /// <returns>创建子目录成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_MD(short DevID, int DrvNo, string Path);

        /// <summary>
        /// 删除某驱动器根目录下的一个子目录，该子目录必须为空才可被删除
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="Path">“FON”或格式为“Pxx”的子目录名，这里x为0-9数字字符</param>
        /// <returns>删除子目录成功，，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_RD(short DevID, int DrvNo, string Path);


        /// <summary>
        /// 获取控制器磁盘的剩余空间
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <returns>字节计算的磁盘剩余空间，-1表示取磁盘剩余空间的操作失败</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_FreeSpace(short DevID, int DrvNo);


        /// <summary>
        /// 将根目录或某子目录的目录项装入控制器的文件缓冲区，返回文件及子目录的数量
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="Path">子目录名，空串表示取根目录下的文件和子目录的数量</param>
        /// <returns>文件和子目录项数，-1表示取目录项数的操作失败</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_DirItemCount(short DevID, int DrvNo, string Path);


        /// <summary>
        /// 取SCL_DirItemCount针对的路径的文件和子目录项。该函数必须在SCL_DirItemCount后无其他网络操作前调用。一般是先调用SCL_DirItemCount函数获得目录项数，根据目录项数申请缓冲区，然后立即调用SCL_GetDirItem函数取目录保存到刚申请的缓冲区中。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="ItemCount">项数，一般为SCL_DirItemCount的返回值；</param>
        /// <param name="Buff">接收返回的文件和子目录名的空间指针，每个项为32字节，符合FAT16格式的32字节定义，Buff的空间大小必须大于等于ItemCount*32字节。(关于FAT16的目录格式，请参见动态链接库的头文件中的说明)</param>
        /// <returns>取目录成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_GetDirItem(short DevID, int ItemCount, int Buff);


        /// <summary>
        /// 发送一个数据包到控制器的文件缓冲区。控制器上有一个2M字节的文件缓冲区，重复调用本函数可将一个文件的所有数据分批发送到该缓冲区。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Offset">本次发送的数据写入缓冲区的地址偏移量</param>
        /// <param name="SendBytes">本次发送的字节数</param>
        /// <param name="Buff">存放数据的缓冲区</param>
        /// <returns>操作成功返回TRUE(非0)，操作失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SendData(short DevID, int Offset, int SendBytes, ref byte Buff);

        /// <summary>
        /// 从控制器的文件缓冲区读回一批数据。当控制器上的一个文件被读入文件缓冲区后，本函数可分批将数据读回计算机。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Offset">本次回读的数据在控制器文件缓冲区中的地址偏移量；</param>
        /// <param name="ReadBytes">本次要求回读的字节数</param>
        /// <param name="Buff">存放数据的缓冲区</param>
        /// <returns>操作成功返回TRUE(非0)，操作失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_ReceiveData(short DevID, int Offset, int ReadBytes, ref byte Buff);

        /// <summary>
        /// 将已发送到控制器的文件缓冲区的数据保存为磁盘文件
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="FileName">含路径的文件名</param>
        /// <param name="Length">保存为文件的以字节为单位的数据长度</param>
        /// <param name="Da">该文件在计算机上的创建日期(可由SCL_GetFileDosDateTime函数获得)</param>
        /// <param name="Ti">该文件在计算机上的创建时间(可由SCL_GetFileDosDateTime函数获得)</param>
        /// <returns>件存盘成功返回TRUE(非0)，失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SaveFile(short DevID, int DrvNo, string FileName, int Length, int Da, int Ti);

        /// <summary>
        /// 将控制器的某个磁盘文件装入控制器的文件缓冲区。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DrvNo">驱动器编号</param>
        /// <param name="FileName">含路径的文件名</param>
        /// <returns>文件装入成功返回TRUE(非0)，失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_LoadFile(short DevID, int DrvNo, string FileName);


        /// <summary>
        /// 立即显示文字串，要求控制器上预装了字库，并在Config.LY中给出了字库的装载要求
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="TextInfo">文字显示的参数</param>
        /// <param name="Str_">待显示的文字串，含扩展信息，参见“扩展显示码”一节。str的长度不得超过1000个字符</param>
        /// <returns>显示字符串成功返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_ShowString(short DevID, ref short TextInfo, string Str_);


        /// <summary>
        /// 复位控制器
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <returns>若控制器正常复位，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Reset(short DevID);

        /// <summary>
        /// 当全部图片及节目表文件都发送到控制器上后，调用此函数令控制器执行新节目表
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Drv">驱动器编号</param>
        /// <param name="Index">节目表编号，0-99。例如，SCL_Replay(nDevID,2,8)可命令控制器播放C:\P08目录下的PLAYLIST.LY；SCL_Replay(nDevID,1,3)则命令控制器播放B:\P03目录下的节目表PLAYLIST.LY</param>
        /// <returns>若控制器开始播放新节目表，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Replay(short DevID, int Drv, int Index);


        /// <summary>
        /// 自动取计算机的系统时钟校准控制器的时钟
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <returns>校准时钟成功则返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetTimer(short DevID);

        /// <summary>
        /// 设置显示屏的亮度。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Bright">亮度值，有效范围为0-31，其中31表示根据外置的亮度传感器自动调整屏体亮度</param>
        /// <returns>设置亮度成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetBright(short DevID, short Bright);

        /// <summary>
        /// 设置自动开关屏时间。若需要强行关闭显示屏，可设置开屏时间、关屏时间均为0；若需强行打开显示屏，可设置开屏时间和关屏时间均为23点59分。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="OnTime">开屏时间</param>
        /// <param name="OffTime">关屏时间。OnTime和OffTime的格式为 时*100+分。例如，9点28分将写成928</param>
        /// <returns>设置成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetOnOffTime(short DevID, short OnTime, short OffTime);


        /// <summary>
        /// 修正温度传感器DS18B20采样的数据。DS18B20传感器允许±2℃的误差，此修正可令LED显示更准确
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Offset">offset为将加到温度传感器采样结果之上的调整值，范围为-7到+7</param>
        /// <returns>设置成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetTempOffset(short DevID, short Offset);

        /// <summary>
        /// 格式化控制器上的磁盘，此函数可以快速删除控制器某磁盘上的所有文件
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="DevNo">驱动器编号。不支持对SD卡的格式化, SD卡的格式化必须由计算机加读卡器来完成，并必须格式化成FAT16的格式；</param>
        /// <returns>格式化成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_FormatDisk(short DevID, int DevNo);


        /// <summary>
        /// 设置屏体电源模式，实现远程屏体电源控制
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="PwrMode">新的屏体电源模式，0：关闭电源，1：打开电源，2：按照SCL_SetOnOffTime函数设置的开关时间自动控制</param>
        /// <returns>设置电源模式成功，返回TRUE(非0)，否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetPowerMode(short DevID, int PwrMode);


        /// <summary>
        /// 获取控制器当前的运行信息
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="Buff512Bytes">保存运行信息的至少512字节的缓冲区，保存在Buff512Bytes中的数据格式请参见C语言Delphi语言的动态链接库头文件。</param>
        /// <returns>获取运行信息成功，返回TRUE(非0)，Buff512Bytes缓冲区中的内容有效；否则返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_GetRunTimeInfo(short DevID, ref byte Buff512Bytes);

        /// <summary>
        /// 获取控制器当前的播放信息
        /// PlayInfo[0]：当前播放的节目表的驱动器编号; 
        ///PlayInfo[1]：当前播放的第00套-第99套节目的索引号，从0开始编号; 
        ///PlayInfo[2]：当前播放的PLAYLIST.LY文件中的第几个节目，从0开始编号； 
        ///PlayInfo[3]：区域1正在播放的节目项索引,从0开始编号 
        ///PlayInfo[4]：区域2正在播放的节目项索引,从0开始编号 
        ///PlayInfo[5]：区域3正在播放的节目项索引,从0开始编号 
        ///PlayInfo[6]：区域4正在播放的节目项索引,从0开始编号 
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="PlayInfo">存放返回结果的至少7个字节的内存空间</param>
        /// <returns>获取播放信息成功，返回TRUE(非0)，PlayInfo中的数据有效；否则返回FALSE(0)。</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_GetPlayInfo(short DevID, ref byte PlayInfo);


        /// <summary>
        /// 关闭LED的显示，停止控制器上的播放进程。控制器上运行了一个分时操作系统，在大批量高速数据传输时，为了保证传输速度，需要停止播放进程，使CPU全力完成数据收发。在串行通讯时可不调用本函数。
        /// </summary>
        /// <param name="DevID">由程序员给定的16位任意不同的设备编号</param>
        /// <param name="OnOff">TRUE表示打开显示，FALSE表示关闭显示。</param>
        /// <returns>操作成功返回TRUE(非0)，操作失败返回FALSE(0)</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_LedShow(short DevID, bool OnOff);


        /// <summary>
        /// 控制SuperComm完整版的SW2(SCL2008:SW1)的输出状态
        /// </summary>
        /// <param name="DevID"></param>
        /// <param name="OnOff">0关闭，1打开</param>
        /// <returns>设置成功，返回TRUE(非0)，否则返回FALSE(0)。</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SetExtSW(short DevID, short OnOff);


        /// <summary>
        /// 取计算机文件的DOS格式的创建日期和时间。
        /// </summary>
        /// <param name="FileName">带完整的路径的计算机文件名</param>
        /// <param name="Da">日期</param>
        /// <param name="Ti">时间</param>
        /// <returns>正确的获得日期、时间数据则返回TRUE(非0)，否则返回FALSE(0) 
        ///返回TRUE时，Date和Time变量中存放有FileName文件创建的日期和时间。
        ///</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_GetFileDosDateTime(string FileName, ref int Da, ref int Ti);

        /// <summary>
        /// 将BMP或JPEG文件转换成包含一个图片的XMP文件，不支持其它格式的图片文件
        /// </summary>
        /// <param name="ColorType">颜色类型，0：双色256级灰度(用于SuperComm) 1：三色64级灰度(用于SuperComm) 2：双色无灰度(用于SCL2008) 3：三色无灰度(用于SCL2008) </param>
        /// <param name="W">目标图片的宽，单位：象素；</param>
        /// <param name="H">目标图片的高，单位：象素；</param>
        /// <param name="bStretched">是否对源图片进行自动缩放；</param>
        /// <param name="PictFile">输入的BMP或JPEG图片文件名(含路径)；</param>
        /// <param name="XMPFile">输出的XMP文件名(含路径)</param>
        /// <returns>转换成功，返回TRUE(非0)，否则返回FALSE(0)。</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_PictToXMPFile(int ColorType, int W, int H, bool bStretched, string PictFile, string XMPFile);


        /// <summary>
        /// 根据单个节目的分区数和播放该图片的区域是否为最小区域计算一个XMP文件的最大字节数。因为控制器在播放时根据当前节目的分区数自动分配文件预读缓冲区(边播放边将下一待播放的文件读入内存)，所以生成的XMP文件的大小不能超出缓冲区大小。本函数按照控制器的缓冲区分配算法获取XMP文件的最大长度。
        /// </summary>
        /// <param name="TotalBuff">节目的分区数，1-4；</param>
        /// <param name="bSmallest">当前计算的区域是否是最小区域</param>
        /// <param name="bSCL2008">控制器类型。FALSE:SuperComm，TRUE：SCL2008</param>
        /// <returns>以字节为单位的XMP文件大小</returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_GetMaxFileSize(int TotalBuff, bool bSmallest, bool bSCL2008);


        /// <summary>
        /// 将两个XMP格式的图片文件合并为一个XMP文件
        /// </summary>
        /// <param name="InFileName">包含一个或多个图片的XMP文件名(含路径)</param>
        /// <param name="OutFileName">包含一个或多个图片的XMP文件名(含路径)，若OutFilename不存在，则执行InFilename到OutFilename的拷贝操作</param>
        /// <param name="BuffSize">XMP文件的最大长度，使用SCL_GetMaxFileSize的返回值。要求InFilename和OutFilename中的图片的颜色类型、基本宽高都一致才可以合并。</param>
        /// <returns>
        /// 若合并成功，返回0，否则返回错误代码： 
        /// 1：InFilename不存在 
        /// 2：InFilename打开失败  
        /// 3：OutFilename打开失败 
        /// 4：合并后的图片总数超过255  
        /// 5：两个文件的版本不符(每点颜色类型不同) 
        ///  6：两个文件中的图片的基本宽高不同 
        /// 7：InFilename表示的文件，其大小与文件中定义的图片类型及大小不符(文件有损坏)  
        ///  8：OutFilename表示的文件，其大小与文件中定义的图片类型及大小不符(文件有损坏) 
        /// 9：合并后的文件大小超出MaxSize限定的最大长度 
        /// </returns>
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SCL_AddXMPToXMP(string InFileName, string OutFileName, int BuffSize);

         
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Init_Start(short Delay, string RemoteIP, short Port, string NetMask, bool bSCL2008);


        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Init_Get(ref byte NetPara);
         
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Init_Set(ref byte NetPara);

        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_Init_Close();

        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SeekStart(short Delay, string RemoteIP, short Port, string NetMask, bool bSCL2008);
         
        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SeekGetAItem(string IP, string Name);

        [DllImport("SCL_API_Stdcall", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SCL_SeekClose();

    }
}
