using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SMTP
{
    internal class Program
    {
        public static string[] _user = { "Alice", "Maya", "Lue", "Diana" };
        public static string[] _usermail = { "fuchiharayuchi@gmail.com", "yourikoaqours@gmail.com", "Lue@gmaiq.com", "Diana@gmaiq.com" }; //前兩個是真正存在之信箱(測試用)
        //預設使用者資料(如DB)
        public static Dictionary<string, string> _defaultUser = new Dictionary<string, string>()
        {
            {_user[0], _usermail[0]},
            {_user[1], _usermail[1]},
            {_user[2], _usermail[2]},
            {_user[3], _usermail[3]},
        };
        private readonly static Dictionary<string, string> _contentTypes = new Dictionary<string, string>
        {
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".txt", "text/plain"},
            {".doc", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".csx", "text/csv"}
        };

        static void Main(string[] args)
        {
            //程式開始
            Console.WriteLine("<<Welcome to use Send Mail Program>>");
            Console.WriteLine("Please input SendName");
            string inputName = Console.ReadLine();
            string usermail;
            //檢查是否有使用者
            var checkuser = checkInput(inputName, out usermail);
            if (!checkuser)
                return;

            //密件副本:預設三人
            Console.WriteLine("Send BccUser? Y/N");
            Dictionary<string, string> inputBccUser = new Dictionary<string, string>();
            string checkSendBcc = Console.ReadLine();
            var YorN = string.Compare(checkSendBcc, "Y", true) == 0;

            if (YorN)
            {
                for (int i = 1; i < 4; i++)
                {
                    Console.WriteLine($"Add No.{i} BccUser Name or  Cancel(c)");
                    string checkinputBCC = Console.ReadLine();                      //輸入值
                    var YorN1st = string.Compare(checkinputBCC, "C", true) == 0;    //取消對比
                    if (YorN1st)
                    {
                        Console.WriteLine("Cancel Add User");
                        break;
                    }

                    string bccmail;
                    var checkBccUser = checkInput(checkinputBCC, out bccmail);
                    //若有使用者
                    if (checkBccUser)
                    {
                        //加入使用者資料至BccUser
                        if (!inputBccUser.ContainsKey(checkinputBCC))
                        {
                            inputBccUser.Add($"{checkinputBCC}", $"{bccmail}");
                            Console.WriteLine("Add User success!");
                        }
                        else
                        {
                            Console.WriteLine("User has in Bcclist already.");
                            continue;
                        }
                    }
                    //無使用者
                    if (!checkBccUser)
                    {
                        Console.WriteLine("Cancel");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Skip Add BccUser");
                Console.WriteLine("====================================");
            }

            string errormsg;
            try
            {
                //收信人設定
                UserMail userInfo = new UserMail();
                userInfo.userName = inputName;
                userInfo.mail = usermail;
                userInfo.DicUserList = inputBccUser;

                //信件內容設定，測試用預設值，大多寫死
                Mail mailInfo = new Mail();
                mailInfo.title = "～寄信測試 成功～ by Mitty";
                mailInfo.body =
                    @"<h1>～寄信測試 成功～</h1>
                    <p>This email is sampele </p>
                    </br>
                    <p><a href='https://github.com/yuchi0731/Portfolio/tree/main/Portfolio'>Portfolio</a> SMTP port</p>
                    </br>
                    <p>using the .NET System.Net.Mail to send.</p>";
                //應可供使用者選擇為html或為一般文字
                mailInfo.IsHtml = true;

                //讓使用者輸入(X)測試先寫死
                //Console.WriteLine("Upload file, please input path.");
                //var inpfilePath = Console.ReadLine();
                //檔案上傳，預設上傳三個
                List<UploadFile> fileList = new List<UploadFile>();
                //預設上傳資料
                UploadFile file = new UploadFile();
                file.path = $@"D:\ubay\SendGmail\TestFileUpload";
                file.fileName = "測試上傳文件.txt";
                file.mimetype = "text/plain";
                fileList.Add(file);

                UploadFile file2 = new UploadFile();
                file2.path = $@"D:\ubay\SendGmail\TestFileUpload";
                file2.fileName = "測試上傳圖片PNG.png";
                file2.mimetype = "image/png";
                fileList.Add(file2);

                UploadFile file3 = new UploadFile();
                file3.path = $@"D:\ubay\SendGmail\TestFileUpload";
                file3.fileName = "測試音樂.mp3";
                file3.mimetype = "audio/mpeg";
                fileList.Add(file3);

                //進行寄信
                var sendMail = SendMail(userInfo, mailInfo, fileList, out errormsg);
                if (sendMail)
                {
                    Console.WriteLine("Send success.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(errormsg);
                    Console.WriteLine("Send fail.");
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                WriteLog(ex);
                Console.WriteLine("Send fail.");
                Console.ReadLine();
            }

            Console.ReadLine();
        }
        private static bool checkInput(string userName, out string usermail)
        {
            usermail = "";
            //檢查輸入值
            if (_defaultUser.ContainsKey(userName) != true)
            {
                Console.WriteLine("User doesn't exist, please restart the program.");
                Console.ReadLine();
                return false;
            }
            else
            {
                Console.WriteLine($"Recipient is: {userName}");
                Console.WriteLine($"Recipient email: {_defaultUser[userName]}");
                Console.WriteLine("Enter to continue.");
                usermail = _defaultUser[userName];
                Console.ReadLine();
                return true;
            }
        }
        private static bool checkUploadFile(List<UploadFile> fileList, out string msg)
        {
            msg = "";
            //密件副本:預設三人
            foreach (var file in fileList)
            {
                var checkExist = File.Exists($@"{file.path}/{file.fileName}");
                if (!checkExist)
                {
                    msg = $"File {file.fileName} doesn't Exist.";
                    return false;
                }
            }
            //檢查檔案上限
            if (fileList.Count > 3)
            {
                msg = "Only accpect 3 files.";
                return false;
            }

            else
                return true;
        }
        public static bool SendMail(UserMail userInfo, Mail mailInfo, List<UploadFile> fileList, out string msg)
        {
            //預設錯誤訊息
            msg = "";
            var msgfile = "";
            //檢查檔案格式
            if (!checkUploadFile(fileList, out msgfile))
            {
                msg = msgfile;
                return false;
            }
            //Bcc最多三人
            if (userInfo.DicUserList.Count > 3)
            {
                msg = "Over of limit on the number of people, restart program";
                return false;
            }

            //信件優先度
            Console.WriteLine("Please set mail priority. 1:Low 2:Normal 3:Hight");
            string inputpriority = Console.ReadLine();
            int priority;
            try
            {
                priority = int.Parse(inputpriority);
            }
            catch
            {
                Console.WriteLine("Use defalut priority : Normal.");
                Console.WriteLine("====================================");
                priority = 2;
            }

            //寄出郵件
            Console.WriteLine("Start send mail, Enter to continue.");
            Console.ReadLine();

            //連結config
            Configuration configSet = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var mailSettings = configSet.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            //檢查config是否都有值
            if (mailSettings != null)
            {
                int port = mailSettings.Smtp.Network.Port;
                string from = mailSettings.Smtp.From;
                string host = mailSettings.Smtp.Network.Host;
                string pwd = mailSettings.Smtp.Network.Password;
                string uid = mailSettings.Smtp.Network.UserName;

                var message = new MailMessage
                {
                    From = new MailAddress(@from)
                };
                message.To.Add(new MailAddress(userInfo.mail));
                message.Subject = mailInfo.title;
                message.IsBodyHtml = mailInfo.IsHtml;
                message.Body = mailInfo.body;

                //有密件副本mail才寄
                if (userInfo.DicUserList != null)
                {
                    foreach (var user in userInfo.DicUserList)
                    {
                        MailAddress sendBcc = new MailAddress($"{user.Value}", $"{user.Key}");
                        message.Bcc.Add(sendBcc);
                    }
                }

                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                //郵件優先度
                mailInfo.Priority = priority;
                if (mailInfo.Priority == 1)
                    message.Priority = MailPriority.Low;
                if (mailInfo.Priority == 2)
                    message.Priority = MailPriority.Normal;
                if (mailInfo.Priority == 3)
                    message.Priority = MailPriority.High;

                //附件
                var ms = GetMemoryStream(fileList);
                foreach (var item in ms)
                {
                    message.Attachments.Add(item);
                }

                //SMTP設定
                var client = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    Credentials = new NetworkCredential(uid, pwd),
                    EnableSsl = true
                };

                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    WriteLog(ex);
                    return false;
                }
            }
            else
            {
                msg = "Can't find mailSettings.";
                return false;
            }
        }
        #region Custom Class
        public class UserMail
        {
            public string userName { get; set; }
            public string mail { get; set; }
            public List<string> BccUserList { get; set; }
            public Dictionary<string, string> DicUserList { get; set; }
        }
        public class Mail
        {
            public string title { get; set; }
            public string body { get; set; }
            public bool IsHtml { get; set; }
            public string fileName { get; set; }
            public int Priority { get; set; }
            public int Encoding { get; set; }

        }
        public class UploadFile
        {
            public string path;
            public byte[] Data;
            public string fileName;
            public string mimetype;
        }
        #endregion

        public static List<Attachment> GetMemoryStream(List<UploadFile> uploadfile)
        {
            var attList = new List<Attachment>();
            foreach (var upfile in uploadfile)
            {
                MemoryStream memStrm = new MemoryStream();

                StreamWriter writer = new StreamWriter(memStrm);
                writer.WriteLine("Writer to");
                writer.WriteLine("MemoryStream");

                // Force the writer to push the data into the underlying stream
                writer.Flush();  // Flush : 清除writer，並將緩衝資料都寫入基礎資料流

                // Create a file stream
                FileStream filStrm = File.Create($@"{upfile.path}\{upfile.fileName}");
                byte[] file = new byte[filStrm.Length];
                filStrm.Read(file, 0, file.Length);

                // Write the entire Memory stream to the file
                memStrm.WriteTo(filStrm);
                memStrm.Position = 0;

                Attachment att = new Attachment(memStrm, upfile.fileName, upfile.mimetype);

                attList.Add(att);
            }

            return attList;
        }

        internal static void WriteLog(Exception ex)
        {
            string msg =
                $@"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                   {ex.ToString()}
                    ";

            string logPath = "D:\\Logs\\Log2.log";
            string folderPath = System.IO.Path.GetDirectoryName(logPath);

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            if (!System.IO.File.Exists(logPath))
                System.IO.File.Create(logPath);

            System.IO.File.AppendAllText(logPath, msg);

            throw ex;
        }
    }
}
