using QualityRangeForClient.ViewModel;
using System.Windows;

namespace QualityRangeForClient
{
    public partial class App : Application
    {
        DataBase.ConnectionDataBase InstanceDataBase = new DataBase.ConnectionDataBase();
        GridAndBarsViewProductPanelVM InstanceLists = new GridAndBarsViewProductPanelVM();
    }
}
