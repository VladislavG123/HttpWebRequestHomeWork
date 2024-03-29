﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace HttpWebRequestHomeWork
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtinClick(object sender, RoutedEventArgs e)
        {
            dataTextBox.Text = "";
            string urlAddress = urlTextBox.Text;

            HttpWebRequest request = WebRequest.CreateHttp(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                string tag = tagTextBox.Text;
                string pattern = string.Format(@"\<{0}.*?\>(?<tegData>.+?)\<\/{0}\>", tag.Trim());

                Regex regex = new Regex(pattern, RegexOptions.ExplicitCapture);//перечисления
                MatchCollection matches = regex.Matches(data);

                List<string> list = new List<string>();

                for (int i = 0; i < matches.Count; i++)
                {
                    Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);

                    list.Add(reg.Replace(matches[i].Groups["tegData"].Value, ""));
                }

                foreach (var item in list)
                {
                    dataTextBox.Text += item + "\r\n";
                }

                //dataTextBox.Text = clearData;

                response.Close();
                readStream.Close();
            }
        }
    }
}
