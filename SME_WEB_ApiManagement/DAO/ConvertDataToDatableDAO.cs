using SME_WEB_ApiManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SME_WEB_ApiManagement.DAO
{
    public class ConvertDataToDatableDAO
    {
        #region Convert Data to Datatable
        //public static DataTable ConvertToDataTable(List<PunchDataModels> list)
        //{
        //    try
        //    {
        //        CultureInfo englishCulture = new CultureInfo("en-US");
        //        DataTable table = new DataTable();

        //        // สร้างคอลัมน์ตาม properties ของ Model
        //        table.Columns.Add("ID", typeof(int));
        //        table.Columns.Add("DataItem", typeof(string));
        //        table.Columns.Add("Fix1", typeof(string));
        //        table.Columns.Add("YearItem", typeof(string));
        //        table.Columns.Add("MonthItem", typeof(string));
        //        table.Columns.Add("DateItem", typeof(string));

        //        table.Columns.Add("ClockTime", typeof(string));
        //        table.Columns.Add("Fix2", typeof(string));
        //        table.Columns.Add("EmpID", typeof(string));
        //        table.Columns.Add("MachineID", typeof(string));
        //        table.Columns.Add("FileName", typeof(string));
        //        table.Columns.Add("PunchDate", typeof(DateTime));
        //        table.Columns.Add("CreateDate", typeof(DateTime));
        //        table.Columns.Add("BatchDate", typeof(DateTime));
        //        table.Columns.Add("State", typeof(string));
        //        table.Columns.Add("Message", typeof(string));
        //        // เพิ่มแถวข้อมูลใน DataTable ตามข้อมูลใน List
        //        foreach (var item in list)
        //        {
        //            DataRow row = table.NewRow();
        //            row["ID"] = 0;
        //            row["DataItem"] = item.PunchData;
        //            row["Fix1"] = item.Fix1;
        //            row["YearItem"] = item.PunchYear;
        //            row["MonthItem"] = item.PunchMonth;
        //            row["DateItem"] = item.PunchDate;

        //            row["ClockTime"] = item.ClockTime;
        //            row["Fix2"] = item.Fix2;
        //            row["EmpID"] = item.EmployeeID;
        //            row["MachineID"] = item.MachineID;
        //            row["FileName"] = item.FileName;

        //            string mmx = Service_CenterDAO.Gettime(null, "", "", "", item.ClockTime, "", "").ToString("HH:mm");
        //            string dateString = item.PunchYear.ToString() + "-" + item.PunchMonth.ToString() + "-" + item.PunchDate.ToString() + " " + mmx;

        //            DateTime xdateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm", englishCulture);
        //            row["PunchDate"] = xdateTime;
        //            row["CreateDate"] = DateTime.Now;  // ใช้ DateTime โดยตรง
        //            row["BatchDate"] = DateTime.Now;   // ใช้ DateTime โดยตรง
        //            row["State"] = "";
        //            row["Message"] = "";
        //            table.Rows.Add(row);
        //        }

        //        return table;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        #endregion Convert Data to Datatable
    }
}
