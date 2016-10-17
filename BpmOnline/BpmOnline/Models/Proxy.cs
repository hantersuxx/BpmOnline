using System;
using System.Data.Services.Client;
using System.Net;
using BpmOnline.EntityDataServiceReference;
using System.Linq;

namespace BpmOnline.Models
{
    public class Proxy
    {
        private static Uri serverUri = new Uri("http://185.47.152.138:1423/0/ServiceModel/EntityDataService.svc/");

        static void OnSendingRequestCookie(object sender, SendingRequestEventArgs e)
        {
            // Вызов метода класса AuthService, реализующего аутентификацию переданного в параметрах метода пользователя.
            AuthService.TryLogin();
            var request = e.Request as HttpWebRequest;
            // Добавление полученных аутентификационных cookie в запрос на получение данных.
            request.CookieContainer = AuthService.AuthCookie;
            e.Request = request;
        }

        public static IQueryable<Contact> GetOdataCollectioByLinq()
        {
            // Создание контекста приложения BPMonline.
            var context = new BPMonline(serverUri);
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            try
            {
                // Построение запроса LINQ для получение коллекции контактов.
                var res = from contacts in context.ContactCollection
                          select contacts;
                return res.OrderBy(c => c.Name);
            }
            catch (Exception ex)
            {
                // Обработка ошибок.
                return null;
            }
        }
    }
}