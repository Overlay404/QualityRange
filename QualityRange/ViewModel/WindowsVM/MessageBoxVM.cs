using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Windows;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel.WindowsVM
{
    internal class MessageBoxVM : ViewModel.Base.ViewModel
    {
        public static MessageBoxVM Instance { get; private set; }

        #region Property
        private string _message;
        public string Message { get => _message; set => Set(ref _message, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            MessageBox.Instance.Close();
        }

        public ICommand DragMoveWindow { get; }
        private bool CanDragMoveWindowExecute(object parameter) => true;
        private void OnDragMoveWindowExecute(object parameter)
        {
            MessageBox.Instance.DragMove();
        }
        #endregion


        public MessageBoxVM()
        {
            Instance = this;

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
        }

        public static void SetMessage(string message)
        {
            Instance.Message = message;
        }

        public static void SetFontSize(int size)
        {
            MessageBox.Instance.Message.FontSize = size;
        }
    }
}
