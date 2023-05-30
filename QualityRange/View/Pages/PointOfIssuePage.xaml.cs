using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QualityRange.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для PointOfIssuePage.xaml
    /// </summary>
    public partial class PointOfIssuePage : Page
    {
        public PointOfIssuePage()
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
                gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
                gMapControl1.MinZoom = 4; //минимальный зум
                gMapControl1.MaxZoom = 16; //максимальный зум
                gMapControl1.Zoom = 10; // какой используется зум при открытии 
                gMapControl1.Position = new GMap.NET.PointLatLng(66.4169575018027, 94.25025752215694);// точка в центре карты при открытии (центр России)
                gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
                gMapControl1.CanDragMap = true; // перетаскивание карты мышью
                gMapControl1.DragButton = MouseButton.Left; // какой кнопкой осуществляется перетаскивание
                gMapControl1.ShowCenter = false; //показывать или скрывать красный крестик в центре
                gMapControl1.ShowTileGridLines = false; //показывать или скрывать тайлы
            };

        }
    }
}
