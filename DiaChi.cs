using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuoi_ki
{
    internal class DiaChi
    {
        private int soNha;
        private string tenDuong;
        private string tenQuan;
        private string thanhPho;

        public int SoNha { get => soNha; set => soNha = value; }
        public string TenDuong { get => tenDuong; set => tenDuong = value; }
        public string TenQuan { get => tenQuan; set => tenQuan = value; }
        public string ThanhPho { get => thanhPho; set => thanhPho = value; }

        public DiaChi() { }

        public DiaChi(int soNha, string tenDuong, string tenQuan, string thanhPho)
        {
            SoNha = soNha;
            TenDuong = tenDuong;
            TenQuan = tenQuan;
            ThanhPho = thanhPho;
        }
      
        public string InDiaChi()
        {
            return $"{this.soNha}, {this.tenDuong}, {this.tenQuan},{this.thanhPho}";
        }


    }
}
