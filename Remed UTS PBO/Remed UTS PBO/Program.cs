using System;
using System.Runtime.CompilerServices;
using LayananDigitalBank;

namespace LayananDigitalBank
{
    abstract class user
    {
        private int _nomorRekening;
        private string _nama;
        private decimal _saldo;

        public int NomorRekening
        {
            get { return _nomorRekening; }
            private set { _nomorRekening = value; }
        }

        public string NamaNasabah
        {
            get { return _nama; }
            private set { _nama = value; }
        }

        public decimal SaldoNasabah
        {
            get { return _saldo; }
            internal set { _saldo = value; }
        }

        protected user(int rekening, string nama, decimal saldoAwal)
        {
            NomorRekening = rekening;
            NamaNasabah = nama;
            SaldoNasabah = saldoAwal;
        }

        public virtual void infoRekening()
        {
            Console.WriteLine("\n----- MENU REKENING -----");
            Console.WriteLine($"nomor rekening anda: {NomorRekening}" );
            Console.WriteLine($"nama anda: {NamaNasabah}" );
            Console.WriteLine($"saldo anda: {SaldoNasabah:N2}" );
        }

        public abstract void Transaksi();
    }

    class Nasabah : user
    {
        private List<user> _daftarNasabah;

        public Nasabah(int rekening, string nama, decimal saldo, List<user> daftarNasabah = null)
            : base(rekening, nama, saldo)
        {
            _daftarNasabah = daftarNasabah;
        }

        public override void Transaksi()
        {

        }
    }
    class penarikanDana : user
    {
        public penarikanDana(int rekening, string nama, decimal saldoAwal) : base(rekening, nama, saldoAwal) { }
        public override void Transaksi()
        {
            bool backToMenuNarik = true;
            while (backToMenuNarik)
            {
                Console.Clear();
                Console.WriteLine("\n----- TARIK DANA -----");
                infoRekening();
                try
                {
                    Console.Write("berapa nominal yang ingin ditarik?: Rp. ");
                    decimal jumlah = Convert.ToDecimal(Console.ReadLine());
                    if (jumlah <= 0)
                    {
                        Console.WriteLine("jumlah penarikan wajib lebih dari 0");
                    }
                    else if (jumlah > SaldoNasabah)
                    {
                        Console.WriteLine($"saldo anda tidak mencukupi, saldo saat ini: Rp. {SaldoNasabah:N2}");
                    }
                    else
                    {
                        SaldoNasabah -= jumlah;
                        Console.WriteLine($"oke penarikan sukses, saldo anda saat ini: Rp. {SaldoNasabah:N2}");
                        backToMenuNarik=false;
                    }
                }
                catch
                {
                    Console.WriteLine("ERROR: karena ini sebuah program berfokus untuk matkul OOP, message error gini saja");
                }
            }
        }
    }

    class setorTunai : user
    {
        public setorTunai(int rekening, string nama, decimal saldoAwal) : base(rekening, nama, saldoAwal) { }
        public override void Transaksi()
        {
            bool backToMenuSetor = true;
            while (backToMenuSetor)
            {
                Console.Clear();
                Console.WriteLine("\n----- SETOR TUNAI -----");
                infoRekening();
                try
                {
                    Console.Write("berapa jumlah yang ingin disetor?: Rp. "); 
                    decimal jumlah = Convert.ToDecimal(Console.ReadLine());
                    if (jumlah <= 0)
                    {
                        Console.WriteLine("jumlah setor wajib lebih dari 0");
                    }
                    else
                    {
                        SaldoNasabah += jumlah;
                        Console.WriteLine($"transfer anda berhasil, saldo saat ini: Rp. {SaldoNasabah:N2}");
                        backToMenuSetor = false;
                    }
                }
                catch
                {
                    Console.WriteLine("ERROR: karena ini sebuah program berfokus untuk matkul OOP, message error gini saja");
                }
            }
        }
    }

    class transferCrossRekening : user
    {
        private List<user> anotherDataNasabah;
        public transferCrossRekening(int rekening, string nama, decimal saldoAwal, List<user> dataNasabah) : base(rekening, nama, saldoAwal)
        {
            anotherDataNasabah = dataNasabah;
        }
        public override void Transaksi()
        {
            bool backToMenuTransfer = true;
            while (backToMenuTransfer)
            {
                Console.Clear();
                Console.WriteLine("\n----- TRANSFER KE REKENING LAIN -----");
                infoRekening();
                try
                {
                    Console.Write("masukkan nomor rekening tujuan: ");
                    int nomor = Convert.ToInt32(Console.ReadLine());
                    user tujuan = anotherDataNasabah.Find(u => u.NomorRekening == nomor);
                    if (tujuan == null)
                    {
                        Console.WriteLine("rekening tidak ada");
                        Console.WriteLine("Press enter to try again");
                        Console.ReadLine();
                        continue;
                    }

                    if (tujuan.NomorRekening == this.NomorRekening)
                    {
                        Console.WriteLine("tidak bisa transfer ke rekening sendiri");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine("masukkan jumlah transfer: Rp. ");
                    decimal jumlah = Convert.ToDecimal(Console.ReadLine());
                    if (jumlah <= 0)
                    {
                        Console.WriteLine("jumlah transfer wajib lebih dari 0");
                    }
                    else if (jumlah > SaldoNasabah)
                    {
                        Console.WriteLine("saldo anda tidak cukup");
                    }
                    else
                    {
                        SaldoNasabah -= jumlah;
                        tujuan.SaldoNasabah += jumlah;
                        Console.WriteLine($"transfer sudah dilakukan, saldo anda saat ini: Rp.{SaldoNasabah:N2}");
                        backToMenuTransfer=false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: karena ini sebuah program berfokus untuk matkul OOP, message error gini saja");
                }
            }
            
        }
    }

    class programUtama
    {
        static List<user> dataNasabah = new List<user>();

        static void Main()
        {
            dataNasabah.Add(new Nasabah(111, "Remi", 1000000, dataNasabah));
            dataNasabah.Add(new Nasabah(222, "Raihan", 500000, dataNasabah));
            dataNasabah.Add(new Nasabah(333, "Bimbimbim", 2000000, dataNasabah));

            bool programIsRunning = true;

            while (programIsRunning)
            {
                Console.Clear();
                Console.WriteLine("===== BANK PELITA DIGITAL =====");
                Console.WriteLine();
                Console.WriteLine("Hola! selamat datang di bank digital kami, silahkan mau ngapain:");
                Console.WriteLine();
                Console.WriteLine("1. Tarik Tunai");
                Console.WriteLine("2. Setor Tunai");
                Console.WriteLine("3. Transfer");
                Console.WriteLine("4. Keluar");
                Console.Write("pilih menu (1-4): ");

                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        JalankanPenarikan();
                        break;

                    case "2":
                        JalankanSetoran();
                        break;

                    case "3":
                        JalankanTransfer();
                        break;

                    case "4":
                        programIsRunning = false;
                        Console.WriteLine("terimakasih karena memercayai kami sebagai bank anda, selamat berjumpa lagi");
                        break;

                    default:
                        Console.WriteLine("ga ada yah, coba lagi");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void JalankanPenarikan()
        {
            Console.Write("masukkan nomor rekening: ");
            int norek = Convert.ToInt32(Console.ReadLine());

            user nasabah = dataNasabah.Find(u => u.NomorRekening == norek);

            if (nasabah != null)
            {
                var penarikan = new penarikanDana(nasabah.NomorRekening, nasabah.NamaNasabah, nasabah.SaldoNasabah);
                penarikan.Transaksi();
                nasabah.SaldoNasabah = penarikan.SaldoNasabah;
            }
            else
            {
                Console.WriteLine("rekening tidak ada");
            }
            Console.ReadKey();
        }

        static void JalankanSetoran()
        {
            Console.Write("Masukkan nomor rekening: ");
            int norek = Convert.ToInt32(Console.ReadLine());

            user nasabah = dataNasabah.Find(u => u.NomorRekening == norek);

            if (nasabah != null)
            {
                setorTunai setor = new setorTunai(nasabah.NomorRekening, nasabah.NamaNasabah, nasabah.SaldoNasabah);
                setor.Transaksi();

                nasabah.SaldoNasabah = setor.SaldoNasabah;

                Console.WriteLine("Setoran berhasil!");
            }
            else
            {
                Console.WriteLine("Rekening tidak ditemukan");
            }
            Console.ReadKey();
        }

        static void JalankanTransfer()
        {
            Console.Write("masukkan nomor rekening: ");
            int norek = Convert.ToInt32(Console.ReadLine());

            user nasabah = dataNasabah.Find(u => u.NomorRekening == norek);

            if (nasabah != null)
            {
                transferCrossRekening transfer = new transferCrossRekening(
                nasabah.NomorRekening,
                nasabah.NamaNasabah,
                nasabah.SaldoNasabah,
                dataNasabah);

                transfer.Transaksi();

                nasabah.SaldoNasabah = transfer.SaldoNasabah;
            }
            else
            {
                Console.WriteLine("rekening tidak ada");
            }
            Console.ReadKey();
        }
    }
}
