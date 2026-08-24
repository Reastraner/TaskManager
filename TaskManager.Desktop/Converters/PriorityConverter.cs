using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Desktop.Converters
{
    public class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskPriority priority)
            {
                return priority switch
                {
                    TaskPriority.Low => "Низкий",
                    TaskPriority.Medium => "Средний",
                    TaskPriority.High => "Высокий",
                    _ => "Неизвестно"
                };
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
