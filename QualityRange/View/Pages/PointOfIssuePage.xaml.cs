using GMap.NET;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QualityRangeForClient.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для PointOfIssuePage.xaml
    /// </summary>
    public partial class PointOfIssuePage : Page
    {
        public static PointOfIssuePage Instance { get; private set; }

        public PointOfIssuePage()
        {
            InitializeComponent();

            Instance = this;

            this.Loaded += (sender, e) =>
            {
                GMaps.Instance.Mode = AccessMode.ServerAndCache;
                gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                gMapControl1.MinZoom = 10;
                gMapControl1.MaxZoom = 25;
                gMapControl1.Zoom = 12;
                gMapControl1.Position = new PointLatLng(55.786419471, 49.121932983);
                gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
                gMapControl1.CanDragMap = true;
                gMapControl1.DragButton = MouseButton.Left;
                gMapControl1.ShowCenter = false;
                gMapControl1.ShowTileGridLines = false;

                foreach (var item in DataBase.ConnectionDataBase.db.PointOfIssue.Local)
                {
                    GMap.NET.WindowsPresentation.GMapMarker gMapMarker = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng((double)item.lat, (double)item.lot))
                    {
                        Shape = InitUIElement(item.Name)
                    };
                    gMapControl1.Markers.Add(gMapMarker);
                }
            };
        }

        private static StackPanel InitUIElement(string name)
        {
            FontAwesome.WPF.ImageAwesome imageAwesome = new FontAwesome.WPF.ImageAwesome()
            {
                Icon = FontAwesome.WPF.FontAwesomeIcon.MapMarker,
                Width = 35,
                Height = 35,
                Foreground = new SolidColorBrush(Colors.Red)
            };

            TextBlock textBlock = new TextBlock()
            {
                Text = name,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
            };

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(imageAwesome);
            stackPanel.Children.Add(textBlock);

            return stackPanel;
        }
    }
}
