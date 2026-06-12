using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; 

namespace WinFormsApp1
{
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string MaLop { get; set; }
    }
    public class Database
    {
        public static string connectionString = @"Server=DoTienLoc;Database=QuanLySinhVien;User ID=sa;Password=123456;TrustServerCertificate=True;";
        public List<SinhVien> LayTatCaSinhVien()
        {
            List<SinhVien> list = new List<SinhVien>();
            string query = "SELECT * FROM SinhVien";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SinhVien
                        {
                            MaSV = reader["MaSV"].ToString(),
                            HoTen = reader["HoTen"].ToString(),
                            NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                            GioiTinh = reader["GioiTinh"].ToString(),
                            MaLop = reader["MaLop"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}