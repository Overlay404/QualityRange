using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QualityRange.Commands.Base;

namespace QualityRange.ViewModel.WindowsVM
{
    internal class AuthRegWindowVM : ViewModel.Base.ViewModel
    {
        #region Property

        #endregion

        #region Commands
        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter) => true;
        private void OnShutdownApplicationExecute(object parameter)
        {
            AuthRegWindow.Instance.Hide();
        }

        public ICommand DragMoveWindow { get; }
        private bool CanDragMoveWindowExecute(object parameter) => true;
        private void OnDragMoveWindowExecute(object parameter)
        {
            
            AuthRegWindow.Instance.DragMove();
        }
        #endregion

        public AuthRegWindowVM()
        {
            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
        }
    }
}
