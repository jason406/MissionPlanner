using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using MissionPlanner.GCSViews;
using MissionPlanner.Controls;
using GMap.NET;


namespace MissionPlanner
{
    public class PanoramaPlugin : MissionPlanner.Plugin.Plugin
    {
        public static MissionPlanner.Plugin.PluginHost Host2;
        ToolStripMenuItem menuitem;

        public override string Name
        {
            get { return "Panorama-plugin"; }
        }

        public override string Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public override string Author
        {
            get { return "Jason406"; }
        }

        public override bool Init()
        {
            return true;
        }

        public override bool Loaded()
        {
            Host2 = Host;
            menuitem = new ToolStripMenuItem("Panorama");
            menuitem.Click += menuitem_Click;

            bool hit = false;
            ToolStripItemCollection col = Host.FPMenuMap.Items;
            int index = col.Count;
            foreach (ToolStripItem item in col)
            {
                if (item.Text.Equals(Strings.AutoWP))
                {
                    index = col.IndexOf(item);
                    ((ToolStripMenuItem)item).DropDownItems.Add(menuitem);
                    hit = true;
                    break;
                }
            }

            if (hit == false)
                col.Add(menuitem);

            return true;
        }

        void menuitem_Click(object sender, EventArgs e)
        {
            string altin = "50";
            string overlapIn= "30";
            string focusLengthIn = "35";
            string startAngleIn = "0";
            string useGimbalIn = "false";
            int alt = 0;
            int overlap = 0;
            int focusLength = 35;
            int startAngle = 0;
            bool useGimbal = true;
            if (DialogResult.Cancel == InputBox.Show("输入飞行高度", "高度", ref altin))
                return;
            if (!int.TryParse(altin, out alt))
            {
                CustomMessageBox.Show("Bad altitude");
                return;
            }
            if (DialogResult.Cancel == InputBox.Show("输入相机焦距", "焦距", ref focusLengthIn))
                return;
            if (!int.TryParse(focusLengthIn, out focusLength))
            {
                CustomMessageBox.Show("Bad focuslength");
                return;
            }
            if (DialogResult.Cancel == InputBox.Show("输入重叠度百分比（APS-C画幅）", "重叠度", ref overlapIn))
                return;
            if (!int.TryParse(overlapIn, out overlap))
            {
                CustomMessageBox.Show("Bad overlap");
                return;
            }
            if (DialogResult.Cancel == InputBox.Show("输入起始角度，负数为朝上", "起始角度", ref startAngleIn))
                return;
            if (!int.TryParse(startAngleIn, out startAngle))
            {
                CustomMessageBox.Show("Bad startAngle");
                return;
            }
            if (DialogResult.Cancel == InputBox.Show("是否使用云台？", "云台", ref useGimbalIn))
                return;
            if (!bool.TryParse(useGimbalIn, out useGimbal))
            {
                CustomMessageBox.Show("please input 1 or 0");
                return;
            }

            PointLatLng panoPoint = Host2.FPMenuMapPosition;
            double mlat = panoPoint.Lat;
            double mlon = panoPoint.Lng;
            
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, alt);
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, mlon, mlat, alt);

            if (focusLength == 35)
            {
                if (startAngle == 0)
                {
                    switch (overlap)
                    {
                        case 300://线性调整重叠度
                            panoramaPhotos(15, 0, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -12, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(13, -25, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(11, -40, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(9, -55, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(6, -70, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, true);
                            break;
                        case 30:
                            panoramaPhotos(15, 0, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -16, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(12, -33, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(10, -49, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(7, -63, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(4, -78, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, true);
                            break;
                        case 25:
                            panoramaPhotos(14, 0, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, -18, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(11, -35, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(8, -52, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(6, -68, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, false);
                            break;
                        case 20:
                            panoramaPhotos(13, 0, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(12, -20, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(10, -38, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(7, -57, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(4, -74, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, false);
                            break;
                        case 15:
                            panoramaPhotos(12, 0, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(11, -20, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(9, -41, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(6, -61, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(3, -79, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, false);
                            break;


                    }
                }
                if (startAngle == -40)
                {
                    switch (overlap)
                    {
                        case 25:
                            panoramaPhotos(11, 40, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(12, 26, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, 13, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, 1, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, -15, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(12, -31, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(9, -47, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(6, -64, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -83, mlon, mlat, alt, useGimbal, false);
                            break;
                        case 30:
                            panoramaPhotos(11, 40, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, 27, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, 15, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(15, 3, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -9, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -23, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(12, -37, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(9, -53, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(6, -68, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -87, mlon, mlat, alt, useGimbal, false);
                            break;
                    }
                }

                if (startAngle == -25)
                {
                    switch (overlap)
                    {
                        case 25:                            
                            panoramaPhotos(12, 26, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, 13, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, 1, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(13, -15, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(12, -31, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(9, -47, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(6, -64, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -83, mlon, mlat, alt, useGimbal, false);
                            break;
                        case 30:                            
                            panoramaPhotos(13, 27, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, 15, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(15, 3, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -9, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(14, -23, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(12, -37, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(9, -53, mlon, mlat, alt, useGimbal, false);
                            panoramaPhotos(6, -68, mlon, mlat, alt, useGimbal, true);
                            panoramaPhotos(3, -87, mlon, mlat, alt, useGimbal, false);
                            break;
                    }
                }
            }



            if (focusLength==75)
            {
                switch (overlap)
                {
                    case 20:
                        panoramaPhotos(27, 0, mlon, mlat, alt, useGimbal, true);
                        panoramaPhotos(27, -9, mlon, mlat, alt, useGimbal, false);
                        panoramaPhotos(25, -18, mlon, mlat, alt, useGimbal, true);
                        panoramaPhotos(23, -27, mlon, mlat, alt, useGimbal, false);
                        panoramaPhotos(22, -35, mlon, mlat, alt, useGimbal, true);
                        panoramaPhotos(18, -44, mlon, mlat, alt, useGimbal, false);
                        panoramaPhotos(16, -53, mlon, mlat, alt, useGimbal, true);
                        panoramaPhotos(12, -62, mlon, mlat, alt, useGimbal, false);
                        panoramaPhotos(9, -70, mlon, mlat, alt, useGimbal, true);
                        panoramaPhotos(6, -78, mlon, mlat, alt, useGimbal, false);
                        panoramaPhotos(3, -90, mlon, mlat, alt, useGimbal, true);                        
                        break;
                }
            }


            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, 0, 0, 0, 0, 0, 0, 10);//云台水平
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.RETURN_TO_LAUNCH, 0, 0, 0, 0, 0, 0, 0);//返航
        }

        public override bool Exit()
        {
            return true;
        }

        private void panoramaPhotos(int photoCount, int gimbalAngle, double mlon, double mlat, int alt)
        {
            float angle = 360 / photoCount;
            int loiter_1 = 1;
            int loiter_2 = 1;
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, 0, 0, 0, 0, 10);
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, 0, 0, 0, 0, 0, 0, 0); //转角度
            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_1, 0, 0, 0, mlon, mlat, alt);//等
            for (int i = 0; i < photoCount; i++)
            {
                int azimuth = Convert.ToInt32(angle * i);
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, azimuth, 0, 0, 0, 0, 0, 0); //转角度
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_1, 0, 0, 0, mlon, mlat, alt);//等
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_DIGICAM_CONTROL, 0, 0, 0, 0, 0, 0, 0);//拍照
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_2, 0, 0, 0, mlon, mlat, alt);//等
            }
        }
        private void panoramaPhotos(int photoCount, int gimbalAngle, double mlon, double mlat, int alt, bool useGimbal, bool isCW = true)
        {
            float angle = 360 / photoCount;
            const double loiter_time1_gimbal_yaw = 2.5;
            const double loiter_time2_gimbal_yaw = 0;
            const double loiter_time1_no_gimbal = 3.5;
            const double loiter_time2_no_gimbal = 0;
            double loiter_1;
            double loiter_2;
            int initalYaw = 0;


            if (useGimbal)
            {

                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, 0, 0, 0, 0, 0, 0, 0); //飞机机头指北
                if (isCW)//顺时针，-180转到180
                {
                    initalYaw = -180;
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, initalYaw, 0, 0, 0, 10); //云台指向-180
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等                
                    for (int i = 0; i < photoCount; i++)
                    {
                        int azimuth = Convert.ToInt32(initalYaw + angle * i);
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, azimuth, 0, 0, 0, 10);//云台转角度                        
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time1_gimbal_yaw, 0, 0, 0, mlon, mlat, alt);//等
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_DIGICAM_CONTROL, 0, 0, 0, 0, 0, 0, 0);//拍照
                        if (loiter_time2_gimbal_yaw != 0)
                        {
                            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time2_gimbal_yaw, 0, 0, 0, mlon, mlat, alt);//等}
                        }
                    }

                }
                else//逆时针，180转到-180
                {
                    initalYaw = 180;
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, initalYaw, 0, 0, 0, 10); //云台指向-180
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 2, 0, 0, 0, mlon, mlat, alt);//等                
                    for (int i = 0; i < photoCount; i++)
                    {
                        int azimuth = Convert.ToInt32(initalYaw - angle * i);
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, azimuth, 0, 0, 0, 10);//云台转角度                        
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time1_gimbal_yaw, 0, 0, 0, mlon, mlat, alt);//等
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_DIGICAM_CONTROL, 0, 0, 0, 0, 0, 0, 0);//拍照
                        if (loiter_time2_gimbal_yaw != 0)
                        {
                            FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time2_gimbal_yaw, 0, 0, 0, mlon, mlat, alt);//等}
                        }
                    }
                }


            }
            else //不用云台
            {
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_MOUNT_CONTROL, gimbalAngle, 0, 0, 0, 0, 0, 10);
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, 3, 0, 0, 0, mlon, mlat, alt);//等
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, 0, 0, 0, 0, 0, 0, 0); //转角度
                FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time1_no_gimbal, 0, 0, 0, mlon, mlat, alt);//等
                for (int i = 0; i < photoCount; i++)
                {
                    int azimuth = Convert.ToInt32(angle * i);
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.CONDITION_YAW, azimuth, 0, 0, 0, 0, 0, 0); //转角度
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time1_no_gimbal, 0, 0, 0, mlon, mlat, alt);//等
                    FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.DO_DIGICAM_CONTROL, 0, 0, 0, 0, 0, 0, 0);//拍照
                    if (loiter_time2_no_gimbal != 0)
                    {
                        FlightPlanner.instance.AddCommand(MAVLink.MAV_CMD.LOITER_TIME, loiter_time2_no_gimbal, 0, 0, 0, mlon, mlat, alt);//等}
                    }
                }
            }


        }
    }
}

