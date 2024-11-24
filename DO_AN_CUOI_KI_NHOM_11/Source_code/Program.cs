using System;
using System.Threading;

namespace Cuoi_ki
{
    class Program
    {
        static void PrintColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // Đặt màu chữ
            Console.WriteLine(text); // In ra dòng chữ
            Console.ResetColor(); // Khôi phục màu mặc định
        }
        static void ShowLoadingBar()
        {
            Console.WriteLine("Chương trình đang khởi động, vui lòng đợi...");
            int totalSteps = 50;
            int delay = 30;
            Console.Write("["); // Bắt đầu thanh
            Console.Write(new string(' ', totalSteps)); // Khoảng trống ban đầu
            Console.Write("]"); // Kết thúc thanh
            Console.SetCursorPosition(1, Console.CursorTop); // Đặt con trỏ sau '['

            for (int i = 0; i <= totalSteps; i++)
            {
                double percentage = (i * 100.0) / totalSteps; // Tính phần trăm
                Console.Write("="); // Thêm 1 bước
                Thread.Sleep(delay); // Dừng
                Console.SetCursorPosition(totalSteps + 3, Console.CursorTop); // Đặt con trỏ ở cuối thanh
                Console.Write($"{percentage:0.0}%"); // Cập nhật phần trăm
                Console.SetCursorPosition(i + 1, Console.CursorTop); // Trở lại vị trí để thêm ký tự tiếp theo
            }
            Thread.Sleep(500);
            Console.WriteLine(); // Xuống dòng khi hoàn tất
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            QuanLyHoGiaDinh quanLyHoGiaDinh = new QuanLyHoGiaDinh();

            ShowLoadingBar(); 

            // Tự động tải dữ liệu từ file khi chương trình bắt đầu
            string filePath = "C:\\Users\\ADMIN\\OneDrive\\Desktop\\DACK_C#\\Cuoi_ki\\Input.txt";  
            quanLyHoGiaDinh.LayDuLieuTuFile(filePath);

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear(); // Xóa màn hình trước khi hiển thị lại menu

                Console.WriteLine("-----------------------------");
                Console.WriteLine("|..... QUẢN LÝ KHU PHỐ .....|");
                Console.WriteLine("-----------------------------");
               
                Console.WriteLine("1. Thống kê cơ cấu dân cư");
                Console.WriteLine("2. Thống kê hộ gia đình");
                Console.WriteLine("3. Tìm kiếm hộ gia đình theo tên chủ hộ");
                Console.WriteLine("4. Tìm kiếm hộ gia đình theo số thành viên");
                PrintColoredText("----------------------------", ConsoleColor.Red);
                Console.WriteLine("5. Cập nhật thông tin trong khu phố");
                Console.WriteLine("6. In danh sách hộ gia đình");
                Console.WriteLine("7. Thoát");

                PrintColoredText("Vui lòng chọn một tùy chọn (1-7): ",ConsoleColor.Green);

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "3":
                        Console.Write("Nhập tên chủ hộ: ");
                        string tenChuHo = Console.ReadLine();
                        quanLyHoGiaDinh.TimHoGiaDinhTheoChuHo(tenChuHo);
                        break;

                    case "1":
                        quanLyHoGiaDinh.PhanTichCoCauDanCu();
                        break;

                    case "2":
                        quanLyHoGiaDinh.ThongKeHoGiaDinh();
                        break;

                    case "6":
                        quanLyHoGiaDinh.XuatDanhSachHoGiaDinh();
                        break;

                    case "4":
                        // Thêm phần nhập vào khoảng số thành viên
                        Console.Write("Nhập số thành viên tối thiểu: ");
                        if (int.TryParse(Console.ReadLine(), out int min))
                        {
                            Console.Write("Nhập số thành viên tối đa: ");
                            if (int.TryParse(Console.ReadLine(), out int max))
                            {
                                quanLyHoGiaDinh.TimHoGiaDinhTheoSoThanhVien(min, max);
                            }
                            else
                            {
                                Console.WriteLine("Số thành viên tối đa không hợp lệ.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Số thành viên tối thiểu không hợp lệ.");
                        }
                        break;
                    case "5":
                        string taiKhoan = "truongkhupho";
                        string pass = "matkhau123";
                        Console.Write("Nhập tài khoản của trưởng khu phố : ");
                        string tk = Console.ReadLine();
                        Console.Write("Nhập mật khẩu : ");
                        string passs = Console.ReadLine();
                        if (tk == taiKhoan && passs == pass)
                        {
                            quanLyHoGiaDinh.ChinhSuaHoGiaDinh();
                            quanLyHoGiaDinh.LuuDuLieuRaFile("C:\\Users\\ADMIN\\OneDrive\\Desktop\\DACK_C#\\Cuoi_ki\\Input.txt");
                        }
                        else Console.WriteLine("Không có quyển truy cập chức năng này.");
                        break;

                    case "7":
                        isRunning = false;
                        Console.WriteLine("Đã thoát khỏi chương trình.");
                        break;

                    default:
                        PrintColoredText("Lựa chọn không hợp lệ, vui lòng chọn lại.", ConsoleColor.Red);
                        break;
                }
                Console.WriteLine("\nNhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }
    }
}
