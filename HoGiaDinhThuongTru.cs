using System;
using System.Collections.Generic;

namespace Cuoi_ki
{
    internal class HoGiaDinhThuongTru : HoGiaDinh, IHoGiaDinh
    {
        private DateTime ngayBatDauThuongTru;

        public DateTime NgayBatDauThuongTru { get => ngayBatDauThuongTru; set => ngayBatDauThuongTru = value; }

        public HoGiaDinhThuongTru() { }

        public HoGiaDinhThuongTru(string tenChuHo, string maSoHoGiaDinh, int soThanhVien, List<Person> danhSachThanhVien, DiaChi diaChi, string loaiChinhSach, DateTime ngayBatDauThuongTru)
            : base(tenChuHo, maSoHoGiaDinh, soThanhVien, danhSachThanhVien, diaChi, loaiChinhSach)
        {
            this.ngayBatDauThuongTru = ngayBatDauThuongTru;
        }

        // Thêm thành viên mới vào danh sách
        public void ThemThanhVien(Person thanhVien)
        {
            thanhVien = thanhVien.NhapThongTin();  // Nhập thông tin cho thành viên mới
            DanhSachThanhVien.Add(thanhVien);
            SoThanhVien++;  // Cập nhật số lượng thành viên
        }

        // Xóa thành viên dựa trên căn cước công dân
        public void XoaThanhVien(string canCuocCongDan)
        {
            bool daXoa = false;
            for (int i = 0; i < DanhSachThanhVien.Count; i++)
            {
                if (DanhSachThanhVien[i].CanCuocCongDan == canCuocCongDan)
                {
                    DanhSachThanhVien.RemoveAt(i);
                    SoThanhVien--;  // Cập nhật số lượng thành viên
                    daXoa = true;
                    Console.WriteLine("Đã xóa thành viên có căn cước công dân: " + canCuocCongDan);
                    break;
                }
            }
            if (!daXoa)
            {
                Console.WriteLine("Không tìm thấy thành viên với căn cước công dân: " + canCuocCongDan);
            }
        }

        // Chỉnh sửa thông tin của thành viên dựa trên căn cước công dân
        public void ChinhSuaThongTin(string canCuocCongDan)
        {
            Person thanhVien = DanhSachThanhVien.Find(tv => tv.CanCuocCongDan == canCuocCongDan);

            if (thanhVien != null)
            {
                Console.WriteLine("Nhập thông tin mới cho thành viên có căn cước công dân: " + canCuocCongDan);
                thanhVien = thanhVien.NhapThongTin();  // Cập nhật lại thông tin của thành viên
                Console.WriteLine("Đã cập nhật thông tin cho thành viên.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy thành viên với căn cước công dân: " + canCuocCongDan);
            }
        }

        public override void XuatThongTin()
        {
            // In thông tin hộ gia đình trong một khung với | và -
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("|                THÔNG TIN HỘ GIA ĐÌNH              |");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("| Tên chủ hộ: " + TenChuHo); 
            Console.WriteLine("| Mã số hộ gia đình: " +  MaSoHoGiaDinh); 
            Console.WriteLine("| Số thành viên: " +  SoThanhVien); 
            Console.WriteLine("| Địa chỉ: " + DiaChi.InDiaChi()); 
            Console.WriteLine("| Loại chính sách: " + LoaiChinhSach); 
            Console.WriteLine("| Ngày bắt đầu thường trú: " + NgayBatDauThuongTru.ToString("dd/MM/yyyy"));  
            Console.WriteLine("|------------------------------------------------------------|");
            Console.WriteLine("| Danh sách thành viên:                                      |");
            Console.WriteLine("|------------------------------------------------------------|");

            // In thông tin từng thành viên trong hộ gia đình
            foreach (var thanhVien in DanhSachThanhVien)
            {
                Console.WriteLine("|------------------------------------------------------------|");
                Console.WriteLine("| Căn cước công dân: " + thanhVien.CanCuocCongDan);
                Console.WriteLine("| Họ tên: " + thanhVien.HoTen);
                Console.WriteLine("| Ngày sinh: " + thanhVien.NgaySinh.ToString("dd/MM/yyyy"));
                Console.WriteLine("| Giới tính: " + (thanhVien.GioiTinh == 1 ? "Nam" : "Nữ"));
                Console.WriteLine("| Nghề nghiệp: " + thanhVien.NgheNghiep);
                Console.WriteLine("| Số điện thoại: " + thanhVien.SoDienThoai);
                Console.WriteLine("| Tôn giáo: " + thanhVien.TonGiao);
                Console.WriteLine("| Mã bảo hiểm xã hội: " + thanhVien.MaBaoHiemXaHoi);
                Console.WriteLine("| Quan hệ với chủ hộ: " + thanhVien.QuanHeVoiChuHo);
                Console.WriteLine("|------------------------------------------------------------|");
            }
        }
    }
}
