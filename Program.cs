using System;
using System.Collections.Generic;
using System.Globalization;

namespace SistemKaryawan
{
    // ===================== KELAS DASAR =====================
    class Karyawan
    {
        public string Nama { get; set; }
        public double Gaji { get; set; }

        public Karyawan(string nama, double gaji)
        {
            Nama = nama;
            Gaji = gaji;
        }

        // Format angka ke Rupiah, contoh: Rp15.000.000
        protected static string Rp(double nilai)
        {
            return "Rp" + nilai.ToString("N0", new CultureInfo("id-ID"));
        }

        public virtual void Kerja()
        {
            Console.WriteLine($"{Nama} sedang bekerja.");
        }

        public virtual void InfoKaryawan()
        {
            Console.WriteLine($"Jenis      : {GetType().Name}");
            Console.WriteLine($"Nama       : {Nama}");
            Console.WriteLine($"Gaji       : {Rp(Gaji)}");
        }
    }

    // ===================== KARYAWAN TETAP =====================
    class Tetap : Karyawan
    {
        public double Tunjangan { get; set; }

        public Tetap(string nama, double gaji, double tunjangan) : base(nama, gaji)
        {
            Tunjangan = tunjangan;
        }

        public double HitungGajiTotal()
        {
            return Gaji + Tunjangan;
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Karyawan Tetap) bekerja sesuai jam kantor.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"Tunjangan  : {Rp(Tunjangan)}");
            Console.WriteLine($"Gaji Total : {Rp(HitungGajiTotal())}");
        }
    }

    class Manager : Tetap
    {
        public Manager(string nama, double gaji, double tunjangan) : base(nama, gaji, tunjangan) { }

        public void Memimpin()
        {
            Console.WriteLine($"{Nama} memimpin rapat dan mengarahkan tim.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Manager) mengatur strategi dan mengawasi tim.");
        }
    }

    class Staff : Tetap
    {
        public Staff(string nama, double gaji, double tunjangan) : base(nama, gaji, tunjangan) { }

        public void KerjakanTugas()
        {
            Console.WriteLine($"{Nama} mengerjakan tugas yang diberikan atasan.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Staff) mengerjakan tugas harian sesuai arahan.");
        }
    }

    // ===================== KARYAWAN KONTRAK =====================
    class Kontrak : Karyawan
    {
        public int Durasi { get; set; } // dalam bulan

        public Kontrak(string nama, double gaji, int durasi) : base(nama, gaji)
        {
            Durasi = durasi;
        }

        public void CekKontrak()
        {
            Console.WriteLine($"Kontrak {Nama} berlaku selama {Durasi} bulan.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Kontrak) bekerja sesuai masa kontrak.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"Durasi     : {Durasi} bulan");
        }
    }

    class Magang : Kontrak
    {
        public Magang(string nama, double gaji, int durasi) : base(nama, gaji, durasi) { }

        public void Belajar()
        {
            Console.WriteLine($"{Nama} sedang belajar dari para senior.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Magang) membantu pekerjaan sambil belajar.");
        }
    }

    class Freelancer : Kontrak
    {
        public Freelancer(string nama, double gaji, int durasi) : base(nama, gaji, durasi) { }

        public void AmbilProyek()
        {
            Console.WriteLine($"{Nama} mengambil proyek baru dari klien.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Freelancer) mengerjakan proyek dari klien.");
        }
    }

    // ===================== PERUSAHAAN =====================
    class Perusahaan
    {
        public string Nama { get; set; }
        private List<Karyawan> daftarKaryawan = new List<Karyawan>();

        // Akses baca-saja ke koleksi, dipakai di Main untuk demo polymorphism
        public IReadOnlyList<Karyawan> Daftar => daftarKaryawan;

        public Perusahaan(string nama)
        {
            Nama = nama;
        }

        public void TambahKaryawan(Karyawan karyawan)
        {
            daftarKaryawan.Add(karyawan);
            Console.WriteLine($"{karyawan.Nama} ditambahkan ke {Nama}.");
        }

        public void DaftarKaryawan()
        {
            int no = 1;
            foreach (Karyawan k in Daftar)
            {
                Console.WriteLine($"--- Karyawan {no} ---");
                k.InfoKaryawan();
                Console.WriteLine();
                no++;
            }
        }
    }

    // ===================== MAIN =====================
    class Program
    {
        static void Judul(string teks)
        {
            Console.WriteLine();
            Console.WriteLine("=== " + teks + " ===");
        }

        static void Main(string[] args)
        {
            // 1. Buat objek perusahaan
            Perusahaan perusahaan = new Perusahaan("PT Maju Jaya");

            // 2. Buat beberapa objek karyawan
            Manager manager = new Manager("Hal Jordan", 15000000, 5000000);
            Staff staff = new Staff("John Stewart", 8000000, 2000000);
            Magang magang = new Magang("Guy Gardner", 2000000, 6);
            Freelancer freelancer = new Freelancer("Mogo", 5000000, 3);

            // 3. Tambahkan ke perusahaan
            Judul("Tambah Karyawan");
            perusahaan.TambahKaryawan(manager);
            perusahaan.TambahKaryawan(staff);
            perusahaan.TambahKaryawan(magang);
            perusahaan.TambahKaryawan(freelancer);

            // 4. Tampilkan semua data
            Judul("Daftar Karyawan");
            perusahaan.DaftarKaryawan();

            // 5. Demonstrasi polymorphism
            Judul("Polymorphism: Kerja() pada semua karyawan");
            foreach (Karyawan k in perusahaan.Daftar)
            {
                k.Kerja();
            }

            // 6. Panggil method khusus
            Judul("Method Khusus");
            manager.Memimpin();
            staff.KerjakanTugas();
            magang.Belajar();
            freelancer.AmbilProyek();
            magang.CekKontrak();
            freelancer.CekKontrak();

            // ===================== JAWABAN SOAL =====================

            Judul("Soal 1: Kerja() pada Manager dan Freelancer");
            manager.Kerja();
            freelancer.Kerja();

            Judul("Soal 2: Memimpin() pada Manager");
            manager.Memimpin();

            Judul("Soal 3: Informasi lengkap Manager");
            manager.InfoKaryawan();

            Judul("Soal 4: Belajar() pada Magang");
            magang.Belajar();

            Judul("Soal 5: Variabel Karyawan berisi objek Staff");
            Karyawan karyawan = new Staff("John Stewart", 7000000, 1500000);
            karyawan.Kerja(); // yang jalan: Kerja() milik Staff

            // karyawan.KerjakanTugas(); // ERROR: tipe Karyawan tidak punya method ini
            if (karyawan is Staff s)
            {
                s.KerjakanTugas(); // bisa dipanggil setelah di-cast ke Staff
            }

            Console.ReadKey();
        }
    }
}
