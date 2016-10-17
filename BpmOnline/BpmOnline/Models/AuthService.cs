using System.IO;
using System.Net;

namespace BpmOnline.Models
{
    public class AuthService
    {
        // Строка запроса к методу Login сервиса AuthService.svc.
        // Замените "my.bpmonline.com" в строке на адрес вашего приложения bpm'online.
        public const string authServiceUri =
        "http://185.47.152.138:1423/ServiceModel/AuthService.svc/Login";
        // Cookie аутентификации bpm'online.
        public static CookieContainer AuthCookie = new CookieContainer();

        // Метод выполняет аутентификацию пользователя .

        // Параметры:
        // userName - имя пользователя bpm'online,
        // userPassword - пароль пользователя bpm'online.
        public static bool TryLogin(string userName = "Пользователь 1", string userPassword = "Пользователь 1")
        {
            // Создание экземпляра запроса к сервису аутентификации.
            var authRequest = HttpWebRequest.Create(authServiceUri) as HttpWebRequest;
            // Определение метода запроса.
            authRequest.Method = "POST";
            // Определение типа контента запроса.
            authRequest.ContentType = "application/json";
            // Включение использования cookie в запросе.
            authRequest.CookieContainer = AuthCookie;
            // Помещение в тело запроса учетной информации пользователя.
            using (var requestStream = authRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(@"{
                         ""UserName"":""" + userName + @""",
                         ""UserPassword"":""" + userPassword + @"""
                         }");
                }
            }
            // Получение ответа от сервера. Если аутентификация проходит успешно, в свойство AuthCookie будут
            // помещены cookie, которые могут быть использованы для последующих запросов.
            using (var response = (HttpWebResponse)authRequest.GetResponse())
            {
                if (AuthCookie.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}