using System;
using System.IO;

namespace StressHeadset_TEST
{
    class Log_message
    {
        static int name_number = 0;
        static string DestinationDir;

        public static void DirCreate(string name)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log");
            DirectoryInfo destinatinoDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + " " + name);
            if (!dir.Exists)
            {
                dir.Create();
            }
            while (destinatinoDir.Exists)
            {
                name_number++;
                destinatinoDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + " " + name + name_number);
            }

            try
            {
                destinatinoDir.Create();
                DestinationDir = destinatinoDir.ToString();
            }
            catch (System.IO.IOException ex)
            {

            }
            Console.WriteLine(DestinationDir);
        }

        public static void LogWrite()
        {
            for (int i = 1; i <= 6; i++)
            {
                string filename = Path.Combine(DestinationDir, "0" + i + ".txt");
                try
                {
                    FileInfo file = new FileInfo(filename);
                    FileStream fs = file.Create();
                    fs.Close();
                }
                catch (System.IO.IOException ex)
                {
                }
            }
        }

        public static void Add_log(string num ,string str)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(Path.Combine(DestinationDir, num + ".txt")))
                {
                    string temp = string.Format("{0}", str);

                    sw.WriteLine(temp);
                    sw.Close();
                }
            }
            catch (System.IO.IOException ex)
            {

            }
        }
    }
}
