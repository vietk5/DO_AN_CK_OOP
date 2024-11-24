using System;

namespace Cuoi_ki
{
    internal class Person
    {
        private string canCuocCongDan;
        private string hoTen;
        private DateTime ngaySinh;
        private int gioiTinh;
        private string ngheNghiep;
        private string soDienThoai;
        private string tonGiao;
        private string maBaoHiemXaHoi;
        private string quanHeVoiChuHo;

        public string CanCuocCongDan { get => canCuocCongDan; set => canCuocCongDan = value; }
        public string HoTen { get => hoTen; set => hoTen = value; }
        public DateTime NgaySinh { get => ngaySinh; set => ngaySinh = value; }
        public int GioiTinh { get => gioiTinh; set => gioiTinh = value; }
        public string NgheNghiep { get => ngheNghiep; set => ngheNghiep = value; }
        public string SoDienThoai { get => soDienThoai; set => soDienThoai = value; }
        public string TonGiao { get => tonGiao; set => tonGiao = value; }
        public string MaBaoHiemXaHoi { get => maBaoHiemXaHoi; set => maBaoHiemXaHoi = value; }
        public string QuanHeVoiChuHo { get => quanHeVoiChuHo; set => quanHeVoiChuHo = value; }

        public Person() { }

        public Person(string canCuocCongDan, string hoTen, DateTime ngaySinh, int gioiTinh, string ngheNghiep, string soDienThoai, string tonGiao, string maBaoHiemXaHoi, string quanHeVoiChuHo)
        {

            this.CanCuocCongDan = canCuocCongDan;
            this.HoTen = hoTen;
            this.NgaySinh = ngaySinh;
            this.GioiTinh = gioiTinh;
            this.NgheNghiep = ngheNghiep;
            this.SoDienThoai = soDienThoai;
            this.TonGiao = tonGiao;
            this.MaBaoHiemXaHoi = maBaoHiemXaHoi;
            this.QuanHeVoiChuHo = quanHeVoiChuHo;
        }

        public Person NhapThongTin()
        {
            Console.WriteLine("Nhập thông tin thành viên:");

            Console.Write("Nhập căn cước công dân: ");
            string canCuocCongDan = Console.ReadLine();

            Console.Write("Nhập họ tên: ");
            string hoTen = Console.ReadLine();

            DateTime ngaySinh;
            while (true)
            {
                Console.Write("Nhập ngày sinh (dd/MM/yyyy): ");
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
                    break;
                else
                    Console.WriteLine("Định dạng ngày tháng không hợp lệ! Vui lòng nhập lại.");
            }

            int gioiTinh;
            while (true)
            {
                Console.Write("Nhập giới tính (1. Nam, 0. Nữ): ");
                if (int.TryParse(Console.ReadLine(), out gioiTinh) && (gioiTinh == 0 || gioiTinh == 1))
                    break;
                else
                    Console.WriteLine("Giới tính không hợp lệ! Vui lòng nhập lại (1 cho Nam, 0 cho Nữ).");
            }

            Console.Write("Nhập nghề nghiệp hiện tại: ");
            string ngheNghiep = Console.ReadLine();

            Console.Write("Nhập số điện thoại: ");
            string soDienThoai = Console.ReadLine();

            Console.Write("Nhập tôn giáo (Nếu không có thì ghi KHÔNG): ");
            string tonGiao = Console.ReadLine();

            Console.Write("Nhập mã bảo hiểm xã hội (Nếu không có ghi KHÔNG): ");
            string maBaoHiemXaHoi = Console.ReadLine();

            Console.Write("Quan hệ với chủ hộ (Ông, Bà, Vợ, Chồng, Con, Cháu,...): ");
            string quanHeVoiChuHo = Console.ReadLine();

            Person person = new Person(canCuocCongDan, hoTen, ngaySinh, gioiTinh, ngheNghiep, soDienThoai, tonGiao, maBaoHiemXaHoi, quanHeVoiChuHo);
            return person;
        }
    }
}
