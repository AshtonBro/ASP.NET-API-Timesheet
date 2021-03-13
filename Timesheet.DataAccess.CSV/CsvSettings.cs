namespace Timesheet.DataAccess.CSV
{
    public class CsvSettings
    {
        public CsvSettings(char delimeter, string path)
        {
            Delimeter = delimeter;
            Path = path;
        }

        public char Delimeter { get; set; }
        public string Path { get; set; }
    }
}