using DarkThemeDataGrid.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DarkThemeDataGrid;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<EntityStatistics> _statistics;
    private DateTime currentDate = new DateTime(2025, 5, 16);

    public MainWindow()
        {
            InitializeComponent();
            
            CreateData();
            
            SetupDataGrid();
            
            currentDate = new DateTime(2025, 5, 16);
            this.Title = $"{GetMonthName(currentDate.Month)} {currentDate.Year} oyidagi statistikalar";
        }
        
        private void CreateData()
        {
            _statistics = PrepareStatistics(GenerateMockData());
            
            var dailyTotals = new EntityStatistics { EntityName = "Kunlik jami" };
            
            for (int day = 1; day < DateTime.DaysInMonth(currentDate.Year, currentDate.Month); day++)
            {
                int dayTotal = 0;
                foreach (var entity in _statistics)
                {
                    if (entity.DayValues.ContainsKey(day))
                    {
                        dayTotal += entity.DayValues[day];
                    }
                }
                
                if (dayTotal > 0)
                {
                    dailyTotals.DayValues[day] = dayTotal;
                }
            dailyTotals.TotalCount += dayTotal;
            } 
            
            _statistics.Add(dailyTotals);
        }
        
        private void SetupDataGrid()
        {
            StatisticsGrid.Columns.Clear();
            
            var entityColumn = new DataGridTextColumn
            {
                Header = "Printer turi",
                Binding = new Binding("EntityName"),
                Width = new DataGridLength(150)
            };
            entityColumn.CellStyle = new Style(typeof(DataGridCell))
            {
                Setters = {
                    new Setter(BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1F1FF")))
                }
            };
            StatisticsGrid.Columns.Add(entityColumn);
            
            for (int day = 1; day <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month); day++)
            {
                var dayColumn = new DataGridTemplateColumn
                {
                    Header = day.ToString(),
                };
                
                var factory = new FrameworkElementFactory(typeof(TextBlock));
                factory.SetBinding(TextBlock.TextProperty, new Binding($"DayValues[{day}]") { TargetNullValue = "0" });
                factory.SetBinding(TextBlock.BackgroundProperty, new Binding($"DayValues[{day}]"));
                
                dayColumn.CellTemplate = new DataTemplate { VisualTree = factory };
                StatisticsGrid.Columns.Add(dayColumn);
            }
            
            var totalColumn = new DataGridTextColumn
            {
                Header = "Jami",
                Binding = new Binding("TotalCount"),
                Width = new DataGridLength(100)
            };
            totalColumn.CellStyle = new Style(typeof(DataGridCell))
            {
                Setters = {
                    new Setter(BackgroundProperty, new SolidColorBrush(Colors.LightGreen)),
                    //new Setter(BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007BFF"))),
                    new Setter(ForegroundProperty, Brushes.White),
                    new Setter(FontWeightProperty, FontWeights.Bold)
                }
            };
            StatisticsGrid.Columns.Add(totalColumn);
            
            StatisticsGrid.ItemsSource = _statistics;
        }
        
        private string GetMonthName(int month)
        {
            return month switch
            {
                1 => "Yanvar",
                2 => "Fevral",
                3 => "Mart",
                4 => "Aprel",
                5 => "May",
                6 => "Iyun",
                7 => "Iyul",
                8 => "Avgust",
                9 => "Sentabr",
                10 => "Oktabr",
                11 => "Noyabr",
                12 => "Dekabr",
                _ => ""
            };
        }
        
        public class EntityStatistics
        {
            public string EntityName { get; set; }
            public Dictionary<int, int> DayValues { get; set; } = new Dictionary<int, int>();
            public int TotalCount { get; set; }
        }

        public ObservableCollection<EntityStatistics> PrepareStatistics(List<ConsumptionDTO> consumptions)
        {
            ObservableCollection<EntityStatistics> statistics = new ObservableCollection<EntityStatistics>();
    
            var printerGroups = consumptions.GroupBy(c => c.PrinterName);
    
            foreach (var printerGroup in printerGroups)
            {
                EntityStatistics entityStat = new EntityStatistics
                {
                    EntityName = printerGroup.Key, 
                    DayValues = new Dictionary<int, int>(),
                    TotalCount = 0
                };
        
                foreach (var consumption in printerGroup)
                {
                    int day = (int)consumption.Day;
                    int filmCount = (int)consumption.FilmCount;
            
                    if (entityStat.DayValues.ContainsKey(day))
                    {
                        entityStat.DayValues[day] += filmCount;
                    }
                    else
                    {
                        entityStat.DayValues[day] = filmCount;
                    }
                    entityStat.TotalCount += filmCount;
                }
        
                statistics.Add(entityStat);
            }
    
            return statistics;
        }
        
        public static List<ConsumptionDTO> GenerateMockData()
        {
            List<ConsumptionDTO> mockData = new List<ConsumptionDTO>();
            Random random = new Random();
    
            string[] printerNames = { "Canon MG3600", "HP LaserJet Pro", "Epson L3150", "Brother HL-L2300D", "Xerox Phaser 3020" };
            string[] filmSizes = { "A4", "A3", "10x15", "13x18", "20x30" };
    
            DateTime startDate = new DateTime(2025, 5, 1); // 1-aprel 2025
    
            for (int i = 1; i <= 1000; i++)
            {
                int daysToAdd = random.Next(0, 31); // 0-30 kun oralig'ida
                DateTime currentDate = startDate.AddDays(daysToAdd);
                int day = currentDate.Day;
        
                int filmCount = random.Next(1, 50); // 1-50 oralig'ida film soni
                int remainingCount = random.Next(0, filmCount); // 0-filmCount oralig'ida qolgan film
        
                ConsumptionDTO consumption = new ConsumptionDTO
                {
                    Id = i,
                    Index = i,
                    PrintingDate = currentDate.ToString("yyyy-MM-dd"),
                    PrinterName = printerNames[random.Next(printerNames.Length)],
                    FilmSize = filmSizes[random.Next(filmSizes.Length)],
                    FilmCount = filmCount,
                    RemainingFilmCount = remainingCount,
                    ImagePath = $"/images/film_{i}.jpg",
                    Day = day
                };
        
                mockData.Add(consumption);
            }
    
            return mockData;
        }

}