using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CoffeeStoreManager.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private int _id;
        private string _name;
        private string _address;
        private string _gmail;
        private string _phone;
        private string _source;
        private string _password;
        private string _newPassword;
        private string _rePassword;
        
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        public string Gmail { get => _gmail; set { _gmail = value; OnPropertyChanged(); } }
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        public string Source { get => _source; set { _source = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(); } }
        public string RePassword { get => _rePassword; set { _rePassword = value; OnPropertyChanged(); } }
        public ICommand OpenAccountChange { get; set; }
        public ICommand SaveAccountCommand { get; set; }
        public ICommand SavePictureCommand { get; set; }
        public ICommand OpenPasswordChange { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }
        public ICommand SavePasswordCommand { get; set; }

        
        public AccountViewModel()
        {
            var select = from s in DataProvider.Ins.DB.TaiKhoanAdmins select s;
            foreach (var data in select)
            {
                Id = 1;
                Name = data.ho_ten;
                Address = data.dia_chi;
                Gmail = data.gmail;
                Phone = data.so_dien_thoai;
                Source = data.image_source;
            }
              
            OpenAccountChange = new RelayCommand<object>((p) => { return true; }, (p) => { openAccountChangeWindow(p); });
            SaveAccountCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SaveAccount(p); });
            SavePictureCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SavePicture(p); });
            OpenPasswordChange = new RelayCommand<object>((p) => { return true; }, (p) => { openPasswordChangeWindow(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { NewPassword = p.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { RePassword = p.Password; });
            SavePasswordCommand = new RelayCommand<object>((p) => { return true; },(p)=>{ SavePassword(p); });
        }
        void openAccountChangeWindow(object p)
        {
            var window = new AccountChangeWindow();
            window.ShowDialog();
        }
        void SaveAccount(object p)
        {
            var change = DataProvider.Ins.DB.TaiKhoanAdmins.SingleOrDefault(x => x.ma_tai_khoan == 1);
            change.ho_ten = Name;
            change.dia_chi = Address;
            change.gmail = Gmail;
            change.so_dien_thoai = Phone;
            DataProvider.Ins.DB.SaveChanges();
            
        }
        void SavePicture(object p)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFileName = ofd.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                Source = bitmap.ToString();

                //Lưu hình ảnh vào database
                var imageChange = DataProvider.Ins.DB.TaiKhoanAdmins.SingleOrDefault(x => x.ma_tai_khoan == Id);
                imageChange.image_source = Source;
                DataProvider.Ins.DB.SaveChanges();
            }
        }
        void openPasswordChangeWindow(object p)
        {
            var window = new PasswordChangeWindow();
            window.ShowDialog();
        }
        void SavePassword(object p)
        {
            var choose = DataProvider.Ins.DB.TaiKhoanAdmins.SingleOrDefault(x => x.ten_dang_nhap == "admin");
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(RePassword))
            {
                System.Windows.MessageBox.Show("Chưa điền đủ thông tin!");
            }
            else
            {
                if (choose.mat_khau == Password)
                {
                    if (NewPassword == RePassword)
                    {
                        var change = DataProvider.Ins.DB.TaiKhoanAdmins.SingleOrDefault(x => x.mat_khau == Password);
                        System.Windows.MessageBox.Show("Đổi mật khẩu thành công");
                        change.mat_khau = NewPassword;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Mật khẩu và xác nhận mật khẩu không trùng khớp");
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Mật khẩu cũ không đúng");
                }
            }
        }
    }
}
