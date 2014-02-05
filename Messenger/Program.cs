using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MessengerDB;
using System.Net.Mail;
using System.Net;

namespace Messenger
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1-AddUser\n2-CheckCredentials\n3-GetRequestList\n" +
                    "4-AddFriendshipRequest\n5-DeleteFromFriends\n6-GetInfoAboutUser" +
                    "\n7-UserIsBanned\n8-MoveToBan\n9-RemoveFromBan\n10-GetFriendList" +
                    "\n11-UserDidSomething\n12-IsOnline\n13-GetEmail\n14-ChangeEmail" +
                    "\n15-CheckEmail\n16-SendEmail\n17-EmailIsValid\nelse-exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        string login;
                        while (true)
                        {
                            Console.Write("Login: ");
                            login = Console.ReadLine();
                            if (DataBase.LoginIsFree(login))
                                break;
                            else
                                Console.WriteLine("Login is not free. Please, write another login\n\n");
                        }
                        Console.Write("Pass: ");
                        string pass = Console.ReadLine();
                        Console.Write("FirstName: ");
                        string firstName = Console.ReadLine();
                        Console.Write("LastName: ");
                        string lastName = Console.ReadLine();
                        Console.Write("About: ");
                        string about = Console.ReadLine();
                        Console.Write("Gender(1-male,else-female): ");
                        bool gender = (Console.ReadLine() == "1") ? true : false;
                        string salt = MakeSalt();
                        string hash = MakeHash(pass, salt);
                        string email;
                        while (true)
                        {
                            Console.Write("Email: ");
                            email = Console.ReadLine();
                            if (DataBase.EmailIsFree(email))
                                break;
                            else
                                Console.WriteLine("Email is not free. Please, write another login\n\n");
                        }
                        Console.WriteLine(DataBase.AddUser(login, firstName, lastName, about, gender, hash, salt, email));
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Write("Login: ");
                        login = Console.ReadLine();
                        Console.Write("Pass: ");
                        pass = Console.ReadLine();
                        Console.WriteLine(DataBase.CheckCredentials(login, MakeHash(pass, DataBase.GetSalt(login))));
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Write("Login: ");
                        login = Console.ReadLine();
                        foreach(var user in DataBase.GetRequestList(login))
                        {
                            Console.WriteLine(user[0].ToString()+" "+user[1].ToString());
                        }
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.Write("from: ");
                        login = Console.ReadLine();
                        Console.Write("shortMessage: ");
                        string message = Console.ReadLine();
                        Console.Write("to: ");
                        DataBase.AddFriendshipRequest(login, Console.ReadLine(), message);
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.Write("login2: ");
                        string login2 = Console.ReadLine();
                        DataBase.DeleteFromFriends(login,login2);
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        foreach(string item in DataBase.GetInfoAboutUser(login))
                        {
                            Console.Write(item+" ");
                        }
                        Console.ReadKey();
                        break;
                    case "7":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.WriteLine(DataBase.UserIsBanned(login));
                        Console.ReadKey();
                        break;
                    case "8":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        DataBase.MoveToBan(login);
                        Console.ReadKey();
                        break;
                    case "9":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        DataBase.RemoveFromBan(login);
                        Console.ReadKey();
                        break;
                    case "10":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        foreach(string item in DataBase.GetFriendList(login))
                        {
                            Console.Write(item+" ");
                        }                        
                        Console.ReadKey();
                        break;
                    case "11":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        DataBase.UserDidSomething(login);
                        Console.ReadKey();
                        break;
                    case "12":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.WriteLine(DataBase.IsOnline(login));
                        Console.ReadKey();
                        break;
                    case "13":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.WriteLine(DataBase.GetEmail(login));
                        Console.ReadKey();
                        break;
                    case "14":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.Write("newEmail: ");
                        string newEmail = Console.ReadLine();
                        DataBase.ChangeEmail(login,newEmail);
                        Console.ReadKey();
                        break;
                    case "15":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.Write("key: ");
                        string key = Console.ReadLine();
                        DataBase.CheckEmail(login, key);
                        Console.ReadKey();
                        break;
                    case "16":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.Write("subject: ");
                        string subject = Console.ReadLine();
                        Console.Write("text: ");
                        string text = Console.ReadLine();
                        DataBase.SendEmail(login,subject,text);
                        Console.ReadKey();
                        break;
                    case "17":
                        Console.Write("login: ");
                        login = Console.ReadLine();
                        Console.WriteLine(DataBase.EmailIsValid(login));
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
        } 
        
        #region //Код для пользовательских програм
        private static string MakeSalt()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            byte[] saltBytes = new byte[8];
            rnd.NextBytes(saltBytes);
            return Encoding.Default.GetString(saltBytes);
        }
        private static string MakeHash(string password, string salt)
        {
            string hashMD5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "");
            string passHash = hashMD5 + salt;
            return BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(passHash))).Replace("-", "");
        }
        #endregion
    }
}
