namespace DarkThemeDataGrid.Models;

public class ConsumptionDTO
{
        public long Id { get; set; }
        public long Index { get; set; }
        public string PrintingDate { get; set; }
        public string PrinterName { get; set; }
        public string FilmSize { get; set; }
        public long FilmCount { get; set; }
        public long RemainingFilmCount { get; set; }
        public string ImagePath { get; set; }
        public long Day { get; set; }
    
}