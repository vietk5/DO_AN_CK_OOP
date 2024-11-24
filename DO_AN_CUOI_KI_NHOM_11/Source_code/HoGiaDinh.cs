using Cuoi_ki;
using System.Collections.Generic;
using System.Net;
abstract class HoGiaDinh
{
    private string tenChuHo;
    private string maSoHoGiaDinh;
    private int soThanhVien;
    private List<Person> danhSachThanhVien;
    private DiaChi diaChi;
    private string loaiChinhSach;

    public HoGiaDinh()
    {
    }
    public HoGiaDinh(string tenChuHo, string maSoHoGiaDinh, int soThanhVien, List<Person> danhSachThanhVien, DiaChi diaChi, string loaiChinhSach)
    {
        this.tenChuHo = tenChuHo;
        this.maSoHoGiaDinh = maSoHoGiaDinh;
        this.soThanhVien = soThanhVien;
        this.danhSachThanhVien = danhSachThanhVien;
        this.diaChi = diaChi;
        this.loaiChinhSach = loaiChinhSach;
    }

    public string TenChuHo { get => tenChuHo; set => tenChuHo = value; }
    public string MaSoHoGiaDinh { get => maSoHoGiaDinh; set => maSoHoGiaDinh = value; }
    public int SoThanhVien { get => soThanhVien; set => soThanhVien = value; }
    public List<Person> DanhSachThanhVien { get => danhSachThanhVien; set => danhSachThanhVien = value; }
    public DiaChi DiaChi { get => diaChi; set => diaChi = value; }
    public string LoaiChinhSach { get => loaiChinhSach; set => loaiChinhSach = value; }

    public abstract void XuatThongTin();
}