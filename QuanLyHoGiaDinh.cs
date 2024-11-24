using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Cuoi_ki
{
    internal class QuanLyHoGiaDinh
    {
        private List<HoGiaDinh> danhSachHoGiaDinh = new List<HoGiaDinh>();

        static void PrintColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // Đặt màu chữ
            Console.WriteLine(text); // In ra dòng chữ
            Console.ResetColor(); // Khôi phục màu mặc định
        }
        public void LayDuLieuTuFile(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        // Bỏ qua dòng trống nếu có
                        string line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        int loaiGiaDinh = int.Parse(line); // 1 là thường trú, 2 là tạm trú
                        string tenChuHo = sr.ReadLine();
                        string maSoHoGiaDinh = sr.ReadLine();
                        int soThanhVien = int.Parse(sr.ReadLine());
                        List<Person> danhSachThanhVien = new List<Person>();

                        for (int i = 0; i < soThanhVien; i++)
                        {
                            string hoTen = sr.ReadLine();
                            DateTime ngaySinh = DateTime.ParseExact(sr.ReadLine(), "dd/MM/yyyy", null);
                            int gioiTinh = int.Parse(sr.ReadLine());
                            string canCuocCongDan = sr.ReadLine();
                            string tonGiao = sr.ReadLine();
                            string ngheNghiep = sr.ReadLine();
                            string soDienThoai = sr.ReadLine();
                            string quanHeVoiChuHo = sr.ReadLine();
                            string maBaoHiemXaHoi = sr.ReadLine();

                            Person thanhVien = new Person(canCuocCongDan, hoTen, ngaySinh, gioiTinh, ngheNghiep, soDienThoai, tonGiao, maBaoHiemXaHoi, quanHeVoiChuHo);
                            danhSachThanhVien.Add(thanhVien);
                        }

                        // Đọc địa chỉ
                        int soNha = int.Parse(sr.ReadLine());
                        string duong = sr.ReadLine();
                        string phuong = sr.ReadLine();
                        string thanhPho = sr.ReadLine();
                        DiaChi diaChi = new DiaChi(soNha, duong, phuong, thanhPho);

                        // Đọc loại chính sách
                        string loaiChinhSach = sr.ReadLine(); // Loại chính sách

                        if (loaiGiaDinh == 1)
                        {
                            DateTime ngayBatDauThuongTru = DateTime.ParseExact(sr.ReadLine(), "dd/MM/yyyy", null);
                            HoGiaDinhThuongTru hoGiaDinh = new HoGiaDinhThuongTru(tenChuHo, maSoHoGiaDinh, soThanhVien, danhSachThanhVien, diaChi, loaiChinhSach, ngayBatDauThuongTru);
                            danhSachHoGiaDinh.Add(hoGiaDinh);
                        }
                        else
                        {
                            DateTime ngayBatDauTamTru = DateTime.ParseExact(sr.ReadLine(), "dd/MM/yyyy", null);
                            DateTime ngayKetThucTamTru = DateTime.ParseExact(sr.ReadLine(), "dd/MM/yyyy", null);
                            HoGiaDinhTamTru hoGiaDinh = new HoGiaDinhTamTru(tenChuHo, maSoHoGiaDinh, soThanhVien, danhSachThanhVien, diaChi, loaiChinhSach, ngayBatDauTamTru, ngayKetThucTamTru);
                            danhSachHoGiaDinh.Add(hoGiaDinh);
                        }

                        // Đọc dòng trống (nếu có) để phân cách các hộ gia đình
                        line = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            throw new Exception("Dữ liệu không đúng định dạng (mỗi hộ gia đình cần được phân cách bằng một dòng trống).");
                        }
                    }
                }

                Console.WriteLine("Đã tải dữ liệu từ file thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi khi đọc file: " + ex.Message);
            }
        }

        // tìm kiếm hộ gia đình theo tên chủ hộ
        public List<HoGiaDinh> TimHoGiaDinhTheoChuHo(string tenChuHo)
        {
            List<HoGiaDinh> ketQuaTimKiem = danhSachHoGiaDinh
                .Where(hg => hg.TenChuHo.Equals(tenChuHo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (ketQuaTimKiem.Count == 0)
            {
                PrintColoredText("\nKhông tìm thấy hộ gia đình nào có chủ hộ tên: " + tenChuHo, ConsoleColor.Red);
            }
            else
            {
                Console.Clear();
                PrintColoredText($"Tìm thấy {ketQuaTimKiem.Count} hộ gia đình có chủ hộ tên: {tenChuHo}", ConsoleColor.Green);
                foreach (var hoGiaDinh in ketQuaTimKiem)
                {
                    hoGiaDinh.XuatThongTin();
                    Console.WriteLine("-----------------------------------------");
                }
            }

            return ketQuaTimKiem;
        }

        public List<HoGiaDinh> TimHoGiaDinhTheoSoThanhVien(int min, int max)
        {
            // Kiểm tra đầu vào
            if (min > max || min < 0)
            {
                PrintColoredText("\nGiá trị min không hợp lệ. Vui lòng nhập lại!", ConsoleColor.Red);
                return new List<HoGiaDinh>();
            }

            // Tìm kiếm các hộ gia đình trong danh sách
            List<HoGiaDinh> ketQuaTimKiem = danhSachHoGiaDinh
                .Where(hg => hg.SoThanhVien >= min && hg.SoThanhVien <= max)
                .ToList();

            // Kiểm tra kết quả tìm kiếm
            if (!ketQuaTimKiem.Any()) // Không tìm thấy
            {
                PrintColoredText($"\nKhông tìm thấy hộ gia đình nào có số thành viên từ {min} đến {max}.", ConsoleColor.Red);
            }
            else // Tìm thấy
            {
                Console.Clear();
                PrintColoredText($"\nTìm thấy {ketQuaTimKiem.Count} hộ gia đình có số thành viên từ {min} đến {max}:", ConsoleColor.Green);

                foreach (var hoGiaDinh in ketQuaTimKiem)
                {
                    hoGiaDinh.XuatThongTin(); // Gọi phương thức để xuất thông tin hộ gia đình
                    Console.WriteLine("-----------------------------------------");
                }
            }

            return ketQuaTimKiem; // Trả về danh sách kết quả tìm kiếm
        }

        // phan tích cơ cấu dân cơ 
        // 1. Thống kê giới tính
        private void ThongKeGioiTinh()
        {
            int soNam = 0;
            int soNu = 0;

            foreach (var hoGiaDinh in danhSachHoGiaDinh)
            {
                foreach (var thanhVien in hoGiaDinh.DanhSachThanhVien)
                {
                    if (thanhVien.GioiTinh == 1) soNam++;
                    else soNu++;
                }
            }

            //Console.WriteLine($"\nSố Nam: {soNam}");
            //Console.WriteLine($"Số Nữ: {soNu}");

            // Độ dài tối đa của cột (tương ứng với chiều cao của cột trong console)
            int maxBarHeight = 20;

            // Tính tỷ lệ cột
            double maleBarHeight = (double)soNam / (soNam + soNu) * maxBarHeight;
            double femaleBarHeight = (double)soNu / (soNam + soNu) * maxBarHeight;

            Console.Clear();

            // Vẽ biểu đồ
            Console.WriteLine("Biểu đồ tương đối số lượng Nam và Nữ");
            Console.WriteLine("====================================");

            // Vẽ cột cho Nam
            PrintColoredText($"Nam:    {new string('#', (int)maleBarHeight)} ({soNam})", ConsoleColor.Blue);

            // Vẽ cột cho Nữ
            PrintColoredText($"Nữ:     {new string('#', (int)femaleBarHeight)} ({soNu})", ConsoleColor.Magenta);

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(); // Đợi người dùng nhập một phím
        }

        // 2. Thống kê bảo hiểm xã hội
        private void ThongKeBaoHiemXaHoi()
        {
            int coBaoHiem = 0;
            int khongBaoHiem = 0;

            foreach (var hoGiaDinh in danhSachHoGiaDinh)
            {
                foreach (var thanhVien in hoGiaDinh.DanhSachThanhVien)
                {
                    if (thanhVien.MaBaoHiemXaHoi != "KHONG") coBaoHiem++;
                    else khongBaoHiem++;
                }
            }

            //Console.WriteLine($"\nSố người có bảo hiểm xã hội: {coBaoHiem}");
            //Console.WriteLine($"Số người không có bảo hiểm xã hội: {khongBaoHiem}");

            // Độ dài tối đa của cột (tương ứng với chiều cao của cột trong console)
            int maxBarHeight = 20;

            // Tính tỷ lệ cột
            double maleBarHeight = (double)coBaoHiem / (coBaoHiem + khongBaoHiem) * maxBarHeight;
            double femaleBarHeight = (double)khongBaoHiem / (coBaoHiem + khongBaoHiem) * maxBarHeight;

            Console.Clear();

            // Vẽ biểu đồ
            Console.WriteLine("Biểu đồ tương đối số người có và không có bảo hiểm xã hội");
            Console.WriteLine("=========================================================");
            PrintColoredText($"Có bảo hiểm:           {new string('#', (int)maleBarHeight)} ({coBaoHiem})", ConsoleColor.Blue);
            PrintColoredText($"Không có bảo hiểm:     {new string('#', (int)femaleBarHeight)} ({khongBaoHiem})", ConsoleColor.Red);

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(); // Đợi người dùng nhập một phím
        }

        // 3. Thống kê thành viên có việc làm
        private void ThongKeThanhVienCoViecLam()
        {
            int coViecLam = 0;
            int khongViecLam = 0;

            foreach (var hoGiaDinh in danhSachHoGiaDinh)
            {
                foreach (var thanhVien in hoGiaDinh.DanhSachThanhVien)
                {
                    if (!string.IsNullOrEmpty(thanhVien.NgheNghiep) && thanhVien.NgheNghiep != "KHONG")
                    {
                        coViecLam++;
                    }
                    else
                    {
                        khongViecLam++;
                    }
                }
            }

            //Console.WriteLine($"\nSố thành viên có việc làm: {coViecLam}");
            //Console.WriteLine($"Số thành viên không có việc làm: {khongViecLam}");

            int maxBarHeight = 20;

            // Tính tỷ lệ cột
            double maleBarHeight = (double)coViecLam / (coViecLam + khongViecLam) * maxBarHeight;
            double femaleBarHeight = (double)khongViecLam / (coViecLam + khongViecLam) * maxBarHeight;

            Console.Clear();

            // Vẽ biểu đồ
            Console.WriteLine("Biểu đồ tương đối số người có và không có việc làm");
            Console.WriteLine("===================================================");
            PrintColoredText($"Có việc làm:           {new string('#', (int)maleBarHeight)} ({coViecLam})", ConsoleColor.Green);
            PrintColoredText($"Không có việc làm:     {new string('#', (int)femaleBarHeight)} ({khongViecLam})", ConsoleColor.Red);

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(); // Đợi người dùng nhập một phím
        }

        // thống kê dân cư theo nhóm tuổi.
        public void ThongKeTheoNhomTuoi()
        {
            var thongKeNhomTuoi = new Dictionary<string, int>
            {
                { "Trẻ em (dưới 18 tuổi)", 0 },
                { "Người lao động (18-60 tuổi)", 0 },
                { "Người cao tuổi (trên 60 tuổi)", 0 }
            };

            foreach (var hoGiaDinh in danhSachHoGiaDinh)
            {
                foreach (var thanhVien in hoGiaDinh.DanhSachThanhVien)
                {
                    int tuoi = DateTime.Now.Year - thanhVien.NgaySinh.Year;
                    if (tuoi < 18) thongKeNhomTuoi["Trẻ em (dưới 18 tuổi)"]++;
                    else if (tuoi <= 60) thongKeNhomTuoi["Người lao động (18-60 tuổi)"]++;
                    else thongKeNhomTuoi["Người cao tuổi (trên 60 tuổi)"]++;
                }
            }
            Console.WriteLine();
            int cnt = 0;
            foreach (var nhom in thongKeNhomTuoi)
            {
                ++cnt;
                if (cnt == 1) PrintColoredText($"{nhom.Key}: {nhom.Value} thành viên", ConsoleColor.Green);
                else if (cnt == 2) PrintColoredText($"{nhom.Key}: {nhom.Value} thành viên", ConsoleColor.Yellow);
                else PrintColoredText($"{nhom.Key}: {nhom.Value} thành viên", ConsoleColor.DarkRed);
            }

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(); // Đợi người dùng nhập một phím
        }

        // dự báo xu hướng dân cư
        public void DuBaoXuHuongDanCu()
        {
            Console.WriteLine("\n--- Dự báo xu hướng dân cư khu phố ---");

            // Khởi tạo các biến để lưu trữ số lượng thành viên trong từng nhóm tuổi
            int treEm = 0;
            int thanhNien = 0;
            int trungNien = 0;
            int nguoiCaoTuoi = 0;

            // Duyệt qua danh sách hộ gia đình
            foreach (var hoGiaDinh in danhSachHoGiaDinh)
            {
                // Phân loại nhóm tuổi cho mỗi thành viên trong hộ gia đình
                foreach (var tv in hoGiaDinh.DanhSachThanhVien)// Sử dụng vòng lặp for thay vì foreach
                {
                    int tuoi = DateTime.Now.Year - tv.NgaySinh.Year;
                    if (tuoi >= 0 && tuoi <= 14)
                        treEm++;
                    else if (tuoi >= 15 && tuoi<= 24)
                        thanhNien++;
                    else if (tuoi >= 25 && tuoi <= 64)
                        trungNien++;
                    else if (tuoi >= 65)
                        nguoiCaoTuoi++;
                }
            }

            // Tính tổng dân số khu phố
            int tongDanSo = treEm + thanhNien + trungNien + nguoiCaoTuoi;

            if (tongDanSo == 0)
            {
                //Console.WriteLine("Không có dữ liệu dân cư để dự báo.");
                PrintColoredText("Không có dữ liệu dân cư để dự báo.", ConsoleColor.Red);
                return;
            }

            // Tính tỷ lệ phần trăm các nhóm tuổi
            double tyLeTreEm = (double)treEm / tongDanSo * 100;
            double tyLeThanhNien = (double)thanhNien / tongDanSo * 100;
            double tyLeTrungNien = (double)trungNien / tongDanSo * 100;
            double tyLeNguoiCaoTuoi = (double)nguoiCaoTuoi / tongDanSo * 100;

            // Hiển thị kết quả phân loại nhóm tuổi trong khu phố
            Console.WriteLine($"Tỷ lệ trẻ em (0-14 tuổi): {tyLeTreEm:F2}%");
            Console.WriteLine($"Tỷ lệ thanh niên (15-24 tuổi): {tyLeThanhNien:F2}%");
            Console.WriteLine($"Tỷ lệ trung niên (25-64 tuổi): {tyLeTrungNien:F2}%");
            Console.WriteLine($"Tỷ lệ người cao tuổi (65+ tuổi): {tyLeNguoiCaoTuoi:F2}%");

            // Dự báo xu hướng dân cư khu phố
            Console.Write("\nDự báo: ");
            if (tyLeTreEm > 40)
            {
                //Console.WriteLine("Dân cư khu phố đang trẻ hóa với tỷ lệ trẻ em chiếm ưu thế. Có thể dự báo sự gia tăng dân số trong tương lai.");
                PrintColoredText("Dân cư khu phố đang trẻ hóa với tỷ lệ trẻ em chiếm ưu thế. Có thể dự báo sự gia tăng dân số trong tương lai.", ConsoleColor.Green);
            }
            else if (tyLeThanhNien > 40)
            {
                //Console.WriteLine("Khu phố có lực lượng lao động trẻ dồi dào, có thể thúc đẩy phát triển kinh tế.");
                PrintColoredText("Khu phố có lực lượng lao động trẻ dồi dào, có thể thúc đẩy phát triển kinh tế.", ConsoleColor.Green);
            }
            else if (tyLeTrungNien > 50)
            {
                //Console.WriteLine("Khu phố chủ yếu là trung niên, xu hướng ổn định dân số.");
                PrintColoredText("Khu phố chủ yếu là trung niên, xu hướng ổn định dân số.", ConsoleColor.Blue);
            }
            else if (tyLeNguoiCaoTuoi > 30)
            {
                //Console.WriteLine("Khu phố đang già hóa với tỷ lệ người cao tuổi tăng cao. Cần chuẩn bị cho các dịch vụ chăm sóc người già.");
                PrintColoredText("Khu phố đang già hóa với tỷ lệ người cao tuổi tăng cao. Cần chuẩn bị cho các dịch vụ chăm sóc người già.", ConsoleColor.Red);
            }
            else
            {
                //Console.WriteLine("Khu phố có sự cân bằng giữa các nhóm tuổi, xu hướng dân số ổn định.");
                PrintColoredText("Khu phố có sự cân bằng giữa các nhóm tuổi, xu hướng dân số ổn định.", ConsoleColor.Blue);
            }

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey(); // Đợi người dùng nhập một phím
        }

        // tình tổng dân cư
        public int GetTongSoDanCu(List<HoGiaDinh> danhSachHoGiaDinh)
        {
            return danhSachHoGiaDinh.Sum(ho => ho.SoThanhVien);
        }

        // phân tích
        public void PhanTichCoCauDanCu()
        {
            int tongSoDanCu = GetTongSoDanCu(danhSachHoGiaDinh);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Phân tích cơ cấu dân cư ---");
                PrintColoredText($"Tổng số dân cư: {tongSoDanCu} người", ConsoleColor.DarkYellow); // In tổng số dân cư
                Console.WriteLine("\n--- Các tùy chọn ---");
                Console.WriteLine("1. Thống kê số lượng Nam/Nữ");
                Console.WriteLine("2. Thống kê bảo hiểm xã hội (có/không)");
                Console.WriteLine("3. Thống kê thành viên có việc làm/không việc làm");
                Console.WriteLine("4. Thống kê theo nhóm tuổi");
                Console.WriteLine("5. Dự báo xu hướng dân cư tương lai");
                Console.WriteLine("6. Quay lại");
                Console.Write("Vui lòng chọn một tùy chọn (1-6): ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThongKeGioiTinh();
                        break;

                    case "2":
                        ThongKeBaoHiemXaHoi();
                        break;

                    case "3":
                        ThongKeThanhVienCoViecLam();
                        break;

                    case "4":
                        ThongKeTheoNhomTuoi();
                        break;

                    case "5":
                        DuBaoXuHuongDanCu();
                        break;

                    case "6":
                        Console.WriteLine("Thoát phân tích cơ cấu dân cư.");
                        return;

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại!");
                        break;
                }
            }
        }

        // Thống kê số lượng hộ gia đình, hộ gia đình chính sách, hộ gia đình tạm trú/thường trú
        public void ThongKeHoGiaDinh()
        {
            // Đếm số lượng các loại hộ gia đình
            int soHoGiaDinh = danhSachHoGiaDinh.Count();
            int soHoGiaDinhChinhSach = danhSachHoGiaDinh.Count(ho => !string.IsNullOrEmpty(ho.LoaiChinhSach) && ho.LoaiChinhSach != "KHONG");
            int soHoGiaDinhThuongTru = danhSachHoGiaDinh.OfType<HoGiaDinhThuongTru>().Count();
            int soHoGiaDinhTamTru = danhSachHoGiaDinh.OfType<HoGiaDinhTamTru>().Count();

            while (true)
            {
                Console.Clear();
                // Hiển thị thống kê
                PrintColoredText($"Tổng số hộ gia đình: {soHoGiaDinh}",ConsoleColor.DarkYellow);
                PrintColoredText($"Số hộ gia đình chính sách: {soHoGiaDinhChinhSach}",ConsoleColor.DarkYellow);
                PrintColoredText($"Số hộ gia đình thường trú: {soHoGiaDinhThuongTru}", ConsoleColor.DarkYellow);
                PrintColoredText($"Số hộ gia đình tạm trú: {soHoGiaDinhTamTru}", ConsoleColor.DarkYellow);
              
                // Hiển thị menu lựa chọn
                Console.WriteLine("\n--- Chọn loại danh sách muốn xem ---");
                Console.WriteLine("1. Danh sách hộ gia đình chính sách");
                Console.WriteLine("2. Danh sách hộ gia đình thường trú");
                Console.WriteLine("3. Danh sách hộ gia đình tạm trú");
                Console.WriteLine("4. Thoát");
                Console.Write("Vui lòng chọn (1-4): ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {

                    case "1":
                        Console.WriteLine("\nDanh sách hộ gia đình chính sách:");
                        var danhSachChinhSach = danhSachHoGiaDinh
                            .Where(ho => !string.IsNullOrEmpty(ho.LoaiChinhSach) && ho.LoaiChinhSach != "KHONG").ToList();
                        InDanhSachHoGiaDinh(danhSachChinhSach);
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey(); // Đợi người dùng nhập một phím
                        break;

                    case "2":
                        Console.WriteLine("\nDanh sách hộ gia đình thường trú:");
                        var danhSachThuongTru = danhSachHoGiaDinh.OfType<HoGiaDinhThuongTru>().ToList();
                        InDanhSachHoGiaDinh(danhSachThuongTru);
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey(); // Đợi người dùng nhập một phím
                        break;

                    case "3":
                        Console.WriteLine("\nDanh sách hộ gia đình tạm trú:");
                        var danhSachTamTru = danhSachHoGiaDinh.OfType<HoGiaDinhTamTru>().ToList();
                        InDanhSachHoGiaDinh(danhSachTamTru);
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey(); // Đợi người dùng nhập một phím
                        break;

                    case "4":
                        Console.WriteLine("Thoát thống kê hộ gia đình.");
                        return;

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại!");
                        break;
                }
            }
        }

        // Hàm hỗ trợ in danh sách tên chủ hộ và mã số hộ gia đình
        private void InDanhSachHoGiaDinh(IEnumerable<HoGiaDinh> danhSach)
        {
            //Console.WriteLine($"{"Chủ hộ".PadRight(25)}{"Mã số hộ gia đình".PadRight(20)}");
            Console.Write($"{"Chủ hộ".PadRight(25)}");
            PrintColoredText($"{"Mã số hộ gia đình".PadRight(20)}", ConsoleColor.Cyan);
            Console.WriteLine(new string('-', 42));
            foreach (var hoGiaDinh in danhSach)
            {
                Console.Write($"{hoGiaDinh.TenChuHo.PadRight(25)}");
                PrintColoredText($"{ hoGiaDinh.MaSoHoGiaDinh.PadRight(20)}", ConsoleColor.Cyan);
            }
        }

        // chỉnh sửa thông tin các hộ gia đình trong khu phố 
        public void ChinhSuaHoGiaDinh()
        {
            bool isEditing = true;
            while (isEditing)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine("|... CHỈNH SỬA HỘ GIA ĐÌNH ...|");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("1. Thêm hộ gia đình mới");
                Console.WriteLine("2. Xóa hộ gia đình theo mã số");
                Console.WriteLine("3. Thêm thành viên vào hộ gia đình");
                Console.WriteLine("4. Xóa thành viên khỏi hộ gia đình");
                Console.WriteLine("5. Quay lại menu chính");
                PrintColoredText("Vui lòng chọn một tùy chọn (1-5): ", ConsoleColor.DarkYellow);
                string luaChon = Console.ReadLine();
                switch (luaChon)
                {
                    case "1":
                        ThemHoGiaDinh();
                        break;
                    case "2":
                        Console.Write("Nhập mã số hộ gia đình cần xóa: ");
                        string maHoXoa = Console.ReadLine();
                        XoaHoGiaDinhTheoMa(maHoXoa);
                        break;
                    case "3":
                        Console.Write("Nhập mã số hộ gia đình cần thêm thành viên: ");
                        string maHoThem = Console.ReadLine();
                        ThemThanhVienVaoHo(maHoThem);
                        break;
                    case "4":
                        Console.Write("Nhập mã số hộ gia đình cần xóa thành viên: ");
                        string maHoXoaTV = Console.ReadLine();
                        XoaThanhVienKhoiHo(maHoXoaTV);
                        break;
                    case "5":
                        isEditing = false;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ, vui lòng chọn lại.");
                        break;
                }
                if (isEditing)
                {
                    Console.WriteLine("\nNhấn Enter để tiếp tục...");
                    Console.ReadLine();
                }
            }
        }
        // thêm hộ gia đình 
        //public void ThemHoGiaDinh()
        //{
        //    Console.Write("Nhập mã số hộ gia đình: ");
        //    string maSo = Console.ReadLine();

        //    Console.Write("Nhập tên chủ hộ: ");
        //    string tenChuHo = Console.ReadLine();

        //    Console.WriteLine("Nhập địa chỉ: ");
        //    Console.Write("Nhập số nhà : ");
        //    int soNhaa = int.Parse(Console.ReadLine());
        //    Console.Write("Nhập trên đường : ");
        //    string duong = Console.ReadLine();
        //    Console.Write("Nhập tên quận : ");
        //    string quan = Console.ReadLine();
        //    Console.Write("Nhập tên thành phố : ");
        //    string thanhPhoo = Console.ReadLine();
        //    DiaChi diachii = new DiaChi(soNhaa, duong, quan, thanhPhoo);
        //    Console.Write("Loại hộ gia đình (1. Thường trú, 2. Tạm trú) : ");
        //    int loaiGD = int.Parse(Console.ReadLine());
        //    Console.Write("Nhập loại chính sách của gia đình (Ho can ngheo, Ho ngeo, ..., nếu không có thì 'KHONG') : ");
        //    string loaiChinhSach = Console.ReadLine();
        //    Console.Write("Nhập số lượng thành viên trong hộ gia đình: ");
        //    if (int.TryParse(Console.ReadLine(), out int soThanhVien) && soThanhVien > 0)
        //    {
        //        List <Person> thanhViens = new List <Person>();
        //        for (int i = 0; i < soThanhVien; i++)
        //        {
        //            Console.WriteLine($"\n--- Nhập thông tin thành viên thứ {i + 1} ---");
        //            Console.Write("Nhập căn cước công dân : ");
        //            string canCuoc = Console.ReadLine();
        //            Console.Write("Nhập tên thành viên mới: ");
        //            string tenThanhVien = Console.ReadLine();
        //            Console.Write("Nhập ngày tháng năm sinh (dd/MM/YYYY): ");
        //            string ngaySinhIn = Console.ReadLine();
        //            if (DateTime.TryParseExact(ngaySinhIn, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinhh))
        //            {
        //                Console.Write("Nhập giới tính (1. Nam , 0. Nữ): ");
        //                int gioiTinh = int.Parse(Console.ReadLine());
        //                Console.Write("Nhập nghề nghiệp, nếu chưa có thì điền 'KHONG': ");
        //                string ngheNghiepp = Console.ReadLine();
        //                Console.Write("Nhập số điện thoại (nếu không có thì điền 'KHONG'): ");
        //                string sdt = Console.ReadLine();
        //                Console.Write("Nhập tôn giáo, nếu không có thì điền 'KHONG': ");
        //                string tonGiao = Console.ReadLine();
        //                Console.Write("Nhập mã bảo hiểm xã hội, nếu chưa có thì điền 'KHONG': ");
        //                string baoHiemXaHoi = Console.ReadLine();
        //                Console.Write("Nhập quan hệ với chủ hộ : ");
        //                string quanHeVoiChuHo = Console.ReadLine();
        //                Person thanhVienMoi = new Person(canCuoc, tenThanhVien, ngaySinhh, gioiTinh, ngheNghiepp, sdt, tonGiao, baoHiemXaHoi, quanHeVoiChuHo);
        //                thanhViens.Add(thanhVienMoi);
        //            }
        //        }
        //        if (loaiGD == 1)
        //        {
        //            Console.Write("Nhập ngày tháng năm bắt đầu thường trú (dd/MM/yyyy) : ");
        //            DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayBatDauThuongTru);
        //            HoGiaDinhThuongTru gdThuongTru = new HoGiaDinhThuongTru(tenChuHo, maSo, soThanhVien, thanhViens, diachii, loaiChinhSach, ngayBatDauThuongTru);
        //            danhSachHoGiaDinh.Add(gdThuongTru);
        //        }
        //        else if (loaiGD == 2)
        //        {

        //            Console.Write("Nhập ngày tháng năm bắt đầu tạm trú (dd/MM/yyyy) : ");
        //            DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayBatDauTamTru);
        //            Console.Write("Nhập ngày tháng năm kết thúc tạm trú (dd/MM/yyyy) : ");
        //            DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayKetThucTamTru);
        //            HoGiaDinhTamTru gdTamTru = new HoGiaDinhTamTru(tenChuHo, maSo, soThanhVien, thanhViens, diachii, loaiChinhSach, ngayBatDauTamTru, ngayKetThucTamTru);
        //            danhSachHoGiaDinh.Add(gdTamTru);
        //        }

        //    }
        //    PrintColoredText("\nĐã thêm hộ gia đình mới thành công.", ConsoleColor.Red);
        //}
        public void ThemHoGiaDinh()
        {
            Console.Write("Nhập mã số hộ gia đình: ");
            string maSo = Console.ReadLine();

            Console.Write("Nhập tên chủ hộ: ");
            string tenChuHo = Console.ReadLine();

            Console.WriteLine("--------------Nhập địa chỉ--------------");

            int soNhaa;
            while (true)
            {
                Console.Write("Nhập số nhà: ");
                if (int.TryParse(Console.ReadLine(), out soNhaa)) break;
                Console.WriteLine("Lỗi: Số nhà phải là một số nguyên. Vui lòng nhập lại!");
            }

            Console.Write("Nhập trên đường: ");
            string duong = Console.ReadLine();
            Console.Write("Nhập tên quận: ");
            string quan = Console.ReadLine();
            Console.Write("Nhập tên thành phố: ");
            string thanhPhoo = Console.ReadLine();
            DiaChi diachii = new DiaChi(soNhaa, duong, quan, thanhPhoo);

            int loaiGD;
            while (true)
            {
                Console.Write("Loại hộ gia đình (1. Thường trú, 2. Tạm trú): ");
                if (int.TryParse(Console.ReadLine(), out loaiGD) && (loaiGD == 1 || loaiGD == 2)) break;
                Console.WriteLine("Lỗi: Loại hộ gia đình chỉ nhận giá trị 1 hoặc 2. Vui lòng nhập lại!");
            }

            Console.Write("Nhập loại chính sách của gia đình (Hộ cận nghèo, Hộ nghèo,... nếu không có thì 'KHONG'): ");
            string loaiChinhSach = Console.ReadLine();

            int soThanhVien;
            while (true)
            {
                Console.Write("Nhập số lượng thành viên trong hộ gia đình: ");
                if (int.TryParse(Console.ReadLine(), out soThanhVien) && soThanhVien > 0) break;
                Console.WriteLine("Lỗi: Số lượng thành viên phải là số nguyên dương. Vui lòng nhập lại!");
            }

            List<Person> thanhViens = new List<Person>();
            for (int i = 0; i < soThanhVien; i++)
            {
                Console.WriteLine($"\n--- Nhập thông tin thành viên thứ {i + 1} ---");
                //Console.Write("Nhập căn cước công dân: ");
                //string canCuoc = Console.ReadLine();
                string canCuoc;
                while (true)
                {
                    Console.Write("Nhập căn cước công dân: ");
                    canCuoc = Console.ReadLine().Trim();

                    // Kiểm tra chỉ chứa chữ số
                    bool isValid = true;
                    foreach (char c in canCuoc)
                    {
                        if (!char.IsDigit(c)) // Chỉ cho phép chữ số
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid && !string.IsNullOrEmpty(canCuoc)) // Kiểm tra không để trống
                    {
                        //Console.WriteLine($"Căn cước công dân hợp lệ: {canCuoc}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Căn cước công dân không hợp lệ. Vui lòng chỉ nhập chữ số.");
                    }
                }

                Console.Write("Nhập tên thành viên mới: ");
                string tenThanhVien = Console.ReadLine();

                DateTime ngaySinhh;
                while (true)
                {
                    Console.Write("Nhập ngày tháng năm sinh (dd/MM/yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinhh)) break;
                    Console.WriteLine("Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)!");
                }

                int gioiTinh;
                while (true)
                {
                    Console.Write("Nhập giới tính (1. Nam , 0. Nữ): ");
                    if (int.TryParse(Console.ReadLine(), out gioiTinh) && (gioiTinh == 1 || gioiTinh == 0)) break;
                    Console.WriteLine("Lỗi: Giới tính chỉ nhận giá trị 1 (Nam) hoặc 0 (Nữ). Vui lòng nhập lại!");
                }

                Console.Write("Nhập nghề nghiệp, nếu chưa có thì điền 'KHONG': ");
                string ngheNghiepp = Console.ReadLine();

                //Console.Write("Nhập số điện thoại (nếu không có thì điền 'KHONG'): ");
                //string sdt = Console.ReadLine();
                string sdt;
                while (true)
                {
                    Console.Write("Nhập số điện thoại (nếu không có thì điền 'KHONG'): ");
                    sdt = Console.ReadLine().Trim();

                    // Kiểm tra nếu người dùng nhập "KHONG"
                    if (sdt.ToUpper() == "KHONG")
                    {
                        Console.WriteLine("Bạn đã chọn không cung cấp số điện thoại.");
                        break;
                    }

                    // Loại bỏ các ký tự không hợp lệ
                    bool isValid = true;
                    foreach (char c in sdt)
                    {
                        if (!char.IsDigit(c)) // Chỉ cho phép chữ số
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Số điện thoại không hợp lệ. Vui lòng chỉ nhập chữ số hoặc điền 'KHONG'.");
                    }
                }


        Console.Write("Nhập tôn giáo, nếu không có thì điền 'KHONG': ");
                string tonGiao = Console.ReadLine();

                Console.Write("Nhập mã bảo hiểm xã hội, nếu chưa có thì điền 'KHONG': ");
                string baoHiemXaHoi = Console.ReadLine();

                Console.Write("Nhập quan hệ với chủ hộ: ");
                string quanHeVoiChuHo = Console.ReadLine();

                Person thanhVienMoi = new Person(canCuoc, tenThanhVien, ngaySinhh, gioiTinh, ngheNghiepp, sdt, tonGiao, baoHiemXaHoi, quanHeVoiChuHo);
                thanhViens.Add(thanhVienMoi);
            }

            if (loaiGD == 1)
            {
                DateTime ngayBatDauThuongTru;
                while (true)
                {
                    Console.Write("Nhập ngày tháng năm bắt đầu thường trú (dd/MM/yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayBatDauThuongTru)) break;
                    Console.WriteLine("Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)!");
                }

                HoGiaDinhThuongTru gdThuongTru = new HoGiaDinhThuongTru(tenChuHo, maSo, soThanhVien, thanhViens, diachii, loaiChinhSach, ngayBatDauThuongTru);
                danhSachHoGiaDinh.Add(gdThuongTru);
            }
            else if (loaiGD == 2)
            {
                DateTime ngayBatDauTamTru, ngayKetThucTamTru;
                while (true)
                {
                    Console.Write("Nhập ngày tháng năm bắt đầu tạm trú (dd/MM/yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayBatDauTamTru)) break;
                    Console.WriteLine("Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)!");
                }

                while (true)
                {
                    Console.Write("Nhập ngày tháng năm kết thúc tạm trú (dd/MM/yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayKetThucTamTru)) break;
                    Console.WriteLine("Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)!");
                }

                HoGiaDinhTamTru gdTamTru = new HoGiaDinhTamTru(tenChuHo, maSo, soThanhVien, thanhViens, diachii, loaiChinhSach, ngayBatDauTamTru, ngayKetThucTamTru);
                danhSachHoGiaDinh.Add(gdTamTru);
            }

            PrintColoredText("\nĐã thêm hộ gia đình mới thành công.", ConsoleColor.Red);
        }


        // xóa họo gia đình ra khỏi khu phố
        public void XoaHoGiaDinhTheoMa(string maSo)
        {
            var hoGiaDinh = danhSachHoGiaDinh.FirstOrDefault(h => h.MaSoHoGiaDinh == maSo);
            if (hoGiaDinh != null)
            {
                danhSachHoGiaDinh.Remove(hoGiaDinh);
                PrintColoredText("Đã xóa hộ gia đình thành công.", ConsoleColor.Green);
            }
            else
            {
                PrintColoredText("Không tìm thấy hộ gia đình với mã số này.", ConsoleColor.Red);
            }
        }
        //
        public void ThemThanhVienVaoHo(string maSo)
        {
            var hoGiaDinh = danhSachHoGiaDinh.FirstOrDefault(h => h.MaSoHoGiaDinh == maSo);
            if (hoGiaDinh != null)
            {
                Console.WriteLine("---- Nhập thông tin thành viên mới ----");
                string canCuoc;
                while (true)
                {
                    Console.Write("Nhập căn cước công dân: ");
                    canCuoc = Console.ReadLine().Trim();

                    // Kiểm tra chỉ chứa chữ số
                    bool isValid = true;
                    foreach (char c in canCuoc)
                    {
                        if (!char.IsDigit(c)) // Chỉ cho phép chữ số
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid && !string.IsNullOrEmpty(canCuoc)) // Kiểm tra không để trống
                    {
                        //Console.WriteLine($"Căn cước công dân hợp lệ: {canCuoc}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Căn cước công dân không hợp lệ. Vui lòng chỉ nhập chữ số.");
                    }
                }

                Console.Write("Nhập tên thành viên mới: ");
                string tenThanhVien = Console.ReadLine();

                DateTime ngaySinhh;
                while (true)
                {
                    Console.Write("Nhập ngày tháng năm sinh (dd/MM/yyyy): ");
                    string ngaySinhIn = Console.ReadLine();
                    if (DateTime.TryParseExact(ngaySinhIn, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinhh))
                        break;
                    Console.WriteLine("Lỗi: Ngày sinh không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)!");
                }

                int gioiTinh;
                while (true)
                {
                    Console.Write("Nhập giới tính (1. Nam, 0. Nữ): ");
                    if (int.TryParse(Console.ReadLine(), out gioiTinh) && (gioiTinh == 1 || gioiTinh == 0))
                        break;
                    Console.WriteLine("Lỗi: Giới tính chỉ nhận giá trị 1 (Nam) hoặc 0 (Nữ). Vui lòng nhập lại!");
                }

                Console.Write("Nhập nghề nghiệp, nếu chưa có thì điền 'KHONG': ");
                string ngheNghiepp = Console.ReadLine();

                //Console.Write("Nhập số điện thoại (nếu không có thì điền 'KHONG'): ");
                //string sdt = Console.ReadLine();
                string sdt;
                while (true)
                {
                    Console.Write("Nhập số điện thoại (nếu không có thì điền 'KHONG'): ");
                    sdt = Console.ReadLine().Trim();

                    // Kiểm tra nếu người dùng nhập "KHONG"
                    if (sdt.ToUpper() == "KHONG")
                    {
                        Console.WriteLine("Bạn đã chọn không cung cấp số điện thoại.");
                        break;
                    }

                    // Loại bỏ các ký tự không hợp lệ
                    bool isValid = true;
                    foreach (char c in sdt)
                    {
                        if (!char.IsDigit(c)) // Chỉ cho phép chữ số
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Số điện thoại không hợp lệ. Vui lòng chỉ nhập chữ số hoặc điền 'KHONG'.");
                    }
                }

                Console.Write("Nhập tôn giáo, nếu không có thì điền 'KHONG': ");
                string tonGiao = Console.ReadLine();

                Console.Write("Nhập mã bảo hiểm xã hội, nếu chưa có thì điền 'KHONG': ");
                string baoHiemXaHoi = Console.ReadLine();

                Console.Write("Nhập quan hệ với chủ hộ: ");
                string quanHeVoiChuHo = Console.ReadLine();

                // Tạo đối tượng thành viên mới
                Person thanhVienMoi = new Person(
                    canCuoc,
                    tenThanhVien,
                    ngaySinhh,
                    gioiTinh,
                    ngheNghiepp,
                    sdt,
                    tonGiao,
                    baoHiemXaHoi,
                    quanHeVoiChuHo
                );

                // Thêm thành viên mới vào danh sách
                hoGiaDinh.DanhSachThanhVien.Add(thanhVienMoi);
                PrintColoredText("Đã thêm thành viên mới thành công.", ConsoleColor.Green);
            }
            else
            {
                PrintColoredText("Không tìm thấy hộ gia đình với mã số này.", ConsoleColor.Red);
            }
        }

        // lưu dữ liệu vào file
        public void LuuDuLieuRaFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var hoGiaDinh in danhSachHoGiaDinh)
                    {
                        // Ghi thông tin loại gia đình: 1 (thường trú) hoặc 2 (tạm trú)
                        if (hoGiaDinh is HoGiaDinhThuongTru)
                        {
                            writer.WriteLine("1");
                            var hoGiaDinhThuongTru = (HoGiaDinhThuongTru)hoGiaDinh;

                            // Ghi thông tin chủ hộ và mã số hộ gia đình
                            writer.WriteLine(hoGiaDinhThuongTru.TenChuHo);
                            writer.WriteLine(hoGiaDinhThuongTru.MaSoHoGiaDinh);

                            // Ghi số thành viên
                            writer.WriteLine(hoGiaDinhThuongTru.DanhSachThanhVien.Count);

                            // Ghi thông tin các thành viên trong gia đình
                            foreach (var thanhVien in hoGiaDinhThuongTru.DanhSachThanhVien)
                            {
                                writer.WriteLine(thanhVien.HoTen);
                                writer.WriteLine(thanhVien.NgaySinh.ToString("dd/MM/yyyy"));
                                writer.WriteLine(thanhVien.GioiTinh);
                                writer.WriteLine(thanhVien.CanCuocCongDan);
                                writer.WriteLine(thanhVien.TonGiao);
                                writer.WriteLine(thanhVien.NgheNghiep);
                                writer.WriteLine(thanhVien.SoDienThoai);
                                writer.WriteLine(thanhVien.QuanHeVoiChuHo);
                                writer.WriteLine(thanhVien.MaBaoHiemXaHoi);
                            }

                            // Ghi địa chỉ
                            var diaChi = hoGiaDinhThuongTru.DiaChi;
                            writer.WriteLine(diaChi.SoNha);
                            writer.WriteLine(diaChi.TenDuong);
                            writer.WriteLine(diaChi.TenQuan);
                            writer.WriteLine(diaChi.ThanhPho);

                            // Ghi loại chính sách và ngày bắt đầu
                            writer.WriteLine(hoGiaDinhThuongTru.LoaiChinhSach);
                            writer.WriteLine(hoGiaDinhThuongTru.NgayBatDauThuongTru.ToString("dd/MM/yyyy"));
                        }
                        else if (hoGiaDinh is HoGiaDinhTamTru)
                        {
                            writer.WriteLine("2");
                            var hoGiaDinhTamTru = (HoGiaDinhTamTru)hoGiaDinh;

                            // Ghi thông tin chủ hộ và mã số hộ gia đình
                            writer.WriteLine(hoGiaDinhTamTru.TenChuHo);
                            writer.WriteLine(hoGiaDinhTamTru.MaSoHoGiaDinh);

                            // Ghi số thành viên
                            writer.WriteLine(hoGiaDinhTamTru.DanhSachThanhVien.Count);

                            // Ghi thông tin các thành viên trong gia đình
                            foreach (var thanhVien in hoGiaDinhTamTru.DanhSachThanhVien)
                            {
                                writer.WriteLine(thanhVien.HoTen);
                                writer.WriteLine(thanhVien.NgaySinh.ToString("dd/MM/yyyy"));
                                writer.WriteLine(thanhVien.GioiTinh);
                                writer.WriteLine(thanhVien.CanCuocCongDan);
                                writer.WriteLine(thanhVien.TonGiao);
                                writer.WriteLine(thanhVien.NgheNghiep);
                                writer.WriteLine(thanhVien.SoDienThoai);
                                writer.WriteLine(thanhVien.QuanHeVoiChuHo);
                                writer.WriteLine(thanhVien.MaBaoHiemXaHoi);
                            }

                            // Ghi địa chỉ
                            var diaChi = hoGiaDinhTamTru.DiaChi;
                            writer.WriteLine(diaChi.SoNha);
                            writer.WriteLine(diaChi.TenDuong);
                            writer.WriteLine(diaChi.TenQuan);
                            writer.WriteLine(diaChi.ThanhPho);

                            // Ghi loại chính sách và ngày bắt đầu, ngày kết thúc
                            writer.WriteLine(hoGiaDinhTamTru.LoaiChinhSach);
                            writer.WriteLine(hoGiaDinhTamTru.NgayBatDau.ToString("dd/MM/yyyy"));
                            writer.WriteLine(hoGiaDinhTamTru.NgayKetThuc.ToString("dd/MM/yyyy"));
                        }

                        // Thêm dòng trống để phân cách hộ gia đình
                        writer.WriteLine();
                    }
                }

                Console.WriteLine("Dữ liệu đã được lưu vào file thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi khi ghi file: " + ex.Message);
            }
        }
        // xóa thánh viên ra khỏi họ gia đình nào đó 
        public void XoaThanhVienKhoiHo(string maSo)
        {
            var hoGiaDinh = danhSachHoGiaDinh.FirstOrDefault(h => h.MaSoHoGiaDinh == maSo);
            if (hoGiaDinh != null)
            {
                Console.Write("Nhập tên thành viên cần xóa: ");
                string tenThanhVien = Console.ReadLine();

                var thanhVien = hoGiaDinh.DanhSachThanhVien.FirstOrDefault(tv => tv.HoTen == tenThanhVien);
                if (thanhVien != null)
                {
                    hoGiaDinh.DanhSachThanhVien.Remove(thanhVien);
                    Console.WriteLine("Đã xóa thành viên thành công.");
                }
                else
                {
                    PrintColoredText("Không tìm thấy thành viên với tên này.", ConsoleColor.Red);
                }
            }
            else
            {
                PrintColoredText("Không tìm thấy hộ gia đình với mã số này.", ConsoleColor.Red);
            }
        }
        // in ra danh sách hộ gia đình 
        public void XuatDanhSachHoGiaDinh()
                {
                    // Cài đặt tài khoản và mật khẩu trưởng khu phố
                    const string taiKhoanTruongKhuPho = "truongkhupho";
                    const string matKhauTruongKhuPho = "matkhau123";

                    // Yêu cầu đăng nhập
                    Console.WriteLine("Vui lòng đăng nhập để truy cập danh sách hộ gia đình.");
                    Console.Write("Tài khoản: ");
                    string taiKhoanNhap = Console.ReadLine();
                    Console.Write("Mật khẩu: ");
                    string matKhauNhap = Console.ReadLine();

                    // Kiểm tra thông tin đăng nhập
                    if (taiKhoanNhap == taiKhoanTruongKhuPho && matKhauNhap == matKhauTruongKhuPho)
                    {
                        Console.Clear();
                        PrintColoredText("Đăng nhập thành công! Dưới đây là danh sách thông tin hộ gia đình", ConsoleColor.Green);
                        Thread.Sleep(2000);
                        Console.Clear();
                        foreach (var hoGiaDinh in danhSachHoGiaDinh)
                        {
                            hoGiaDinh.XuatThongTin();
                            Console.WriteLine("-----------------------------------------");
                        }
                    }
                    else
                    {
                        PrintColoredText("Đăng nhập thất bại! Bạn không có quyền truy cập vào thông tin này.", ConsoleColor.Red);
                    }
                }

            }
        }
