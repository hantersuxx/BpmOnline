using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace BpmOnline.Models
{
    public class OdataHttpQuery
    {
        // Строка адреса BPMonline сервиса OData.
        private const string serverUri =
        "http://185.47.152.138:1423/0/ServiceModel/EntityDataService.svc/";
        private const string authServiceUtri =
        "http://185.47.152.138:1423/ServiceModel/AuthService.svc/Login";

        // Ссылки на пространства имен XML.
        private static readonly XNamespace ds =
        "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static readonly XNamespace dsmd =
        "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";

        private const string userName = "Пользователь 1";
        private const string userPassword = "Пользователь 1";

        // Строка запроса:
        // POST <Адрес приложения BPMonline>/0/ServiceModel/EntityDataService.svc/ContactCollection/
        public static void CreateBpmEntityByOdataHttp(Contact contact)
        {
            // Создание сообщения xml, содержащего данные о создаваемом объекте.
            var content = new XElement(dsmd + "properties",
                new XElement(ds + "Name", contact.Name),
                new XElement(ds + "MobilePhone", contact.MobilePhone),
                new XElement(ds + "Dear", contact.Dear),
                new XElement(ds + "JobTitle", contact.JobTitle),
                new XElement(ds + "BirthDate", contact.BirthDate));
            var entry = new XElement(atom + "entry",
                new XElement(atom + "content",
                new XAttribute("type", "application/xml"), content));
            Console.WriteLine(entry.ToString());
            // Создание запроса к сервису, который будет добавлять новый объект в коллекцию контактов.
            AuthService.TryLogin();
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri + "ContactCollection/");
            request.CookieContainer = AuthService.AuthCookie;
            request.Credentials = new NetworkCredential(userName, userPassword);
            request.Method = "POST";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            // Запись xml-сообщения в поток запроса.
            using (var writer = XmlWriter.Create(request.GetRequestStream()))
            {
                entry.WriteTo(writer);
            }
            // Получение ответа от сервиса о результате выполнения операции.
            using (WebResponse response = request.GetResponse())
            {
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created)
                {
                    // Обработка результата выполнения операции.
                }
            }
        }

        // Строка запроса:
        // PUT <Адрес приложения BPMonline>/0/ServiceModel/EntityDataService.svc/ContactCollection(guid'00000000-0000-0000-0000-000000000000')
        public static void UpdateExistingBpmEntityByOdataHttp(Contact contact)
        {
            // Создание сообщения xml, содержащего данные об изменяемом объекте.
            var content = new XElement(dsmd + "properties",
                new XElement(ds + "Name", contact.Name),
                new XElement(ds + "MobilePhone", contact.MobilePhone),
                new XElement(ds + "Dear", contact.Dear),
                new XElement(ds + "JobTitle", contact.JobTitle),
                new XElement(ds + "BirthDate", contact.BirthDate));
            var entry = new XElement(atom + "entry",
            new XElement(atom + "content",
            new XAttribute("type", "application/xml"),
            content)
            );
            // Создание запроса к сервису, который будет изменять данные объекта.
            AuthService.TryLogin();
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri
             + "ContactCollection(guid'" + contact.Id + "')");
            request.CookieContainer = AuthService.AuthCookie;
            request.Credentials = new NetworkCredential(userName, userPassword);
            request.Method = "PUT";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            // Запись сообщения xml в поток запроса.
            using (var writer = XmlWriter.Create(request.GetRequestStream()))
            {
                entry.WriteTo(writer);
            }
            // Получение ответа от сервиса о результате выполнения операции.
            using (WebResponse response = request.GetResponse())
            {
                // Обработка результата выполнения операции.
            }
        }

        // Строка запроса:
        // DELETE <Адрес приложения BPMonline>/0/ServiceModel/EntityDataService.svc/ContactCollection(guid'00000000-0000-0000-0000-000000000000')
        public static void DeleteBpmEntityByOdataHttp(Guid id)
        {
            // Создание запроса к сервису, который будет удалять данные.
            AuthService.TryLogin();
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri
             + "ContactCollection(guid'" + id + "')");
            request.CookieContainer = AuthService.AuthCookie;
            request.Credentials = new NetworkCredential(userName, userPassword);
            request.Method = "DELETE";
            // Получение ответа от сервися о результате выполненя операции.
            using (WebResponse response = request.GetResponse())
            {
                // Обработка результата выполнения операции.
            }
        }
    }
}