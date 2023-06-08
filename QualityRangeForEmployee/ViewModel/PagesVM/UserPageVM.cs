using DataBase.Model;
using DataBase;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRangeForEmployee.ViewModel.PagesVM
{
    internal class UserPageVM : ViewModel.Base.ViewModel
    {
        public static UserPageVM Instance { get; private set; }

        #region Property
        private ObservableCollection<Client> _clients = new ObservableCollection<Client>(ConnectionDataBase.db.Client.Local);
        public ObservableCollection<Client> Clients { get => _clients; set => Set(ref _clients, value); }
        
        
        private ObservableCollection<Salesman> _salesmens = new ObservableCollection<Salesman>(ConnectionDataBase.db.Salesman.Local);
        public ObservableCollection<Salesman> Salesmens { get => _salesmens; set => Set(ref _salesmens, value); }
        #endregion


        #region Commands
        public ICommand ShowClients { get; }
        private bool CanShowClientsExecute(object parameter) => true;
        private void OnShowClientsExecute(object parameter)
        {
            UserPage.Instance.MainFrame.Navigate(new ClientsListViewPage());
        }


        public ICommand ShowSalesmans { get; }
        private bool CanShowSalesmansExecute(object parameter) => true;
        private void OnShowSalesmansExecute(object parameter)
        {
            UserPage.Instance.MainFrame.Navigate(new SalesmansListViewPage());
        }

        public ICommand Ban { get; }
        private bool CanBanExecute(object parameter) => true;
        private void OnBanExecute(object parameter)
        {
            if (parameter is Client client)
            {
                client.User.Removed = true;
            }
            else
            {
                (parameter as Salesman).User.Removed = true;
                foreach (var item in (parameter as Salesman).Product)
                {
                    item.ID_Status = 3;
                }
            }

            RefreshItems();
        }


        public ICommand Unban { get; }
        private bool CanUnbanExecute(object parameter) => true;
        private void OnUnbanExecute(object parameter)
        {
            if (parameter is Client client)
            {
                client.User.Removed = false;
            }
            else
            {
                (parameter as Salesman).User.Removed = false;
                foreach (var item in (parameter as Salesman).Product)
                {
                    item.ID_Status = 2;
                }
            }

            RefreshItems();
        }
        #endregion


        public UserPageVM()
        {
            Instance = this;

            ShowClients = new LambdaCommand(OnShowClientsExecute, CanShowClientsExecute);
            ShowSalesmans = new LambdaCommand(OnShowSalesmansExecute, CanShowSalesmansExecute);
            Ban = new LambdaCommand(OnBanExecute, CanBanExecute);
            Unban = new LambdaCommand(OnUnbanExecute, CanUnbanExecute);
        }


        private static void RefreshItems()
        {
            if(ClientsListViewPage.Instance == null)
            {
                SalesmansListViewPage.Instance.ListProduct.Items.Refresh();
                return;
            }

            if (SalesmansListViewPage.Instance == null)
            {
                ClientsListViewPage.Instance.ListProduct.Items.Refresh();
                return;
            }

            ClientsListViewPage.Instance.ListProduct.Items.Refresh();
            SalesmansListViewPage.Instance.ListProduct.Items.Refresh();
        }
    }
}
