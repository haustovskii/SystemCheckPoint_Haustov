using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemCheckPoint.AppData
{
    public class FrameContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Авторизация"; // Значение по умолчанию, если FrameMain пустой
            }

            // Получаем значение роли из параметра
            int role = (int)parameter;

            if (role == MainWindow.RoleID)
            {
                if (role == 1)
                {
                    return "Главное меню администратора";
                }
                else if (role == 2)
                {
                    return "Главное меню охранника";
                }
            }

            return ((System.Windows.Controls.Page)value).Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
