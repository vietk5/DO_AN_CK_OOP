using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuoi_ki
{
    internal class HoGiaDinhTamTru : HoGiaDinh, IHoGiaDinh
    {
        private DateTime ngayBatDau;
        private DateTime ngayKetThuc;
        public HoGiaDinhTamTru()
        {
        }
        public HoGiaDinhTamTru(string tenChuHo, string maSoHoGiaDinh, int soThanhVien, List<Person> danhSachThanhVien,
            DiaChi diaChi, string loaiChinhSach, DateTime ngayBatDau, DateTime ngayKetThuc) : base(tenChuHo, maSoHoGiaDinh, soThanhVien, danhSachThanhVien, diaChi, loaiChinhSach)
        {
            this.ngayBatDau = ngayBatDau;
            this.ngayKetThuc = ngayKetThuc;
        }

        public DateTime NgayBatDau { get => ngayBatDau; set => ngayBatDau = value; }
        public DateTime NgayKetThuc { get => ngayKetThuc; set => ngayKetThuc = value; }
        public void ThemThanhVien(Person thanhVien)
        {
            thanhVien = thanhVien.NhapThongTin();  // Nhập thông tin cho thành viên mới
            DanhSachThanhVien.Add(thanhVien);
            SoThanhVien++;  // Cập nhật số lượng thành viên
        }
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
            // In thông tin hộ gia đình trong một khung
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("|                THÔNG TIN HỘ GIA ĐÌNH              |");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("| Tên chủ hộ: " + TenChuHo);
            Console.WriteLine("| Mã số hộ gia đình: " + MaSoHoGiaDinh);
            Console.WriteLine("| Số thành viên: " + SoThanhVien);
            Console.WriteLine("| Địa chỉ: " + DiaChi.InDiaChi());
            Console.WriteLine("| Loại chính sách: " + LoaiChinhSach);
            Console.WriteLine("| Ngày bắt đầu tạm trú: " + NgayBatDau.ToString("dd/MM/yyyy"));
            Console.WriteLine("| Ngày kết thúc tạm trú: " + NgayKetThuc.ToString("dd/MM/yyyy"));
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("| Danh sách thành viên:                            |");
            Console.WriteLine("-----------------------------------------------------");

            // In thông tin từng thành viên trong hộ gia đình
            foreach (var thanhVien in DanhSachThanhVien)
            {
                Console.WriteLine("|-----------------------------------------------|");
                Console.WriteLine("| Căn cước công dân: " + thanhVien.CanCuocCongDan);
                Console.WriteLine("| Họ tên: " + thanhVien.HoTen);
                Console.WriteLine("| Ngày sinh: " + thanhVien.NgaySinh.ToString("dd/MM/yyyy"));
                Console.WriteLine("| Giới tính: " + (thanhVien.GioiTinh == 1 ? "Nam" : "Nữ"));
                Console.WriteLine("| Nghề nghiệp: " + thanhVien.NgheNghiep);
                Console.WriteLine("| Số điện thoại: " + thanhVien.SoDienThoai);
                Console.WriteLine("| Tôn giáo: " + thanhVien.TonGiao);
                Console.WriteLine("| Mã bảo hiểm xã hội: " + thanhVien.MaBaoHiemXaHoi);
                Console.WriteLine("| Quan hệ với chủ hộ: " + thanhVien.QuanHeVoiChuHo);
                Console.WriteLine("|-----------------------------------------------|");
            }
            Console.WriteLine("-----------------------------------------------------");
        }

    }
}

