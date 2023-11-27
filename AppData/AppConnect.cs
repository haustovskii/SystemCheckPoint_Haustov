namespace SystemCheckPoint.AppData
{
    /// <summary>
    /// Класс AppConnect предоставляет статическое поле modelOdb для работы с базой данных SQL Server 
    /// </summary>
    public class AppConnect
    {
        /// <summary>
        /// Статическое поле, представляющее контекст базы данных для взаимодействия с SQL Server.
        /// </summary>
        public static CheckPointDbEntities1 modelOdb = new CheckPointDbEntities1();
    }
}




