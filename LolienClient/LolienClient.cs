using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LeagueReplayParser;
using LoLienClient;
using Newtonsoft.Json.Linq;

namespace LolienClient
{
    public partial class LolienClient : Form
    {
        public const string HOST = "https://webgori.kr";

        public LolienClient()
        {
            InitializeComponent();
        }

        private void LolienClient_Load(object sender, System.EventArgs e)
        {
            Form lolienClientForm = Application.OpenForms["LolienClient"];
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lolienClientForm.Text = lolienClientForm.Text + " v" + version.Substring(0, version.Length - 2) + " by 노래하는엘사";

            StringBuilder replayDirectoryPathStringBuilder = new StringBuilder();
            string configFilePath = Directory.GetCurrentDirectory() + "\\config.ini";
            GetPrivateProfileString("LolienClient", "ReplayDirectoryPath", "", replayDirectoryPathStringBuilder, Int32.MaxValue, configFilePath);

            string replayDirectoryPath = replayDirectoryPathStringBuilder.ToString();

            if (replayDirectoryPath.Equals(""))
            {
                string userPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    userPath = Directory.GetParent(userPath).ToString();
                }

                replayDirectoryPath = userPath + "\\Documents\\League of Legends\\Replays";
            }

            DirectoryInfo replayDirectoryInfo = new DirectoryInfo(replayDirectoryPath);
            if (replayDirectoryInfo.Exists)
            {
                replayDirectoryText.Text = replayDirectoryPath;
                watchReplayDirectory(replayDirectoryPath);
            }

            JObject leaguesJsonObj = GetLeagues();
            JArray leaguesJsoArray = (JArray) leaguesJsonObj["leagues"];

            foreach (JObject leagueObj in leaguesJsoArray) {
                ComboboxItem combobox = new ComboboxItem();

                string title = (string) leagueObj["title"];
                combobox.Text = title;

                string leagueIdx = (string) leagueObj["idx"];
                combobox.Value = leagueIdx;

                leagueComboBox.Items.Add(combobox);
            }

            if (leaguesJsoArray.Count == 0)
            {
                leagueRadioButton.Enabled = false;

                ComboboxItem combobox = new ComboboxItem();
                combobox.Text = "현재 진행중인 리그가 존재하지 않습니다.";
                combobox.Value = -1;

                leagueComboBox.Items.Add(combobox);
            }

            leagueComboBox.SelectedIndex = 0;
        }

        private void ReplayDirectoryButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string replayDirectoryPath = fbd.SelectedPath;
                    replayDirectoryText.Text = replayDirectoryPath;
                    string configFilePath = Directory.GetCurrentDirectory() + "\\config.ini";
                    WritePrivateProfileString("LolienClient", "ReplayDirectoryPath", replayDirectoryPath, configFilePath);
                    watchReplayDirectory(replayDirectoryPath);
                }
            }
        }

        private JObject GetLeagues()
        {
            string uri = HOST + "/league";
            string responseText = string.Empty;

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }
            } catch (WebException e)
            {
                MessageBox.Show("서버에 문제가 발생하였습니다.", "Lolien Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Process.GetCurrentProcess().Kill();
            }

            return JObject.Parse(responseText);
        }

        private void LeagueRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            leagueComboBox.Enabled = true;
        }

        private void CustomGameReplayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            leagueComboBox.Enabled = false;
        }

        public void watchReplayDirectory(string replayDirectoryPath)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = replayDirectoryPath;

            watcher.NotifyFilter = NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName;

            watcher.Filter = "*.rofl";
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            string replayFilePath = e.FullPath;

            Thread.Sleep(3000);

            long matchId = long.Parse(Path.GetFileNameWithoutExtension(e.Name).Replace("KR-", ""));
            getEntriesFromReplayFile(matchId, replayFilePath);
        }

        private void getEntriesFromReplayFile(long matchId, string replayFilePath)
        {
            Replay replay = Replay.Parse(replayFilePath, Encoding.UTF8);
            List<String> entrieList = new List<String>();

            foreach (Player player in replay.Players)
            {
                String playerName = player.PlayerName.Replace(" ", "");
                entrieList.Add(playerName);
            }

            string entries = String.Join(",", entrieList);
            addGameResultData(matchId, entries);
        }

        private void addGameResultData(long matchId, String entries)
        {
            Boolean checkedCustomGame = customGameReplayRadioButton.Checked;
            Boolean checkedLeague = leagueRadioButton.Checked;

            string uri = string.Empty;

            if (checkedCustomGame)
            {
                uri = HOST + "/custom-game/result";
            }
            else if (checkedLeague)
            {
                uri = HOST + "/league/result";
            }

            this.Invoke(new Action(delegate ()
            {
                int leagueIdx = Int32.Parse((leagueComboBox.SelectedItem as ComboboxItem).Value.ToString());

                if (leagueIdx > 0)
                {
                    string requestJson = "{\"leagueIdx\": " + leagueIdx + ", \"matchId\": " + matchId + ", \"entries\": \"" + entries + "\"}";
                    WebClient webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    webClient.Encoding = UTF8Encoding.UTF8;
                    try
                    {
                        string responseJSON = webClient.UploadString(uri, requestJson);

                        MessageBox.Show("대전 결과가 성공적으로 등록되었습니다.", "Lolien Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException e)
                    {
                        Stream responseStream = e.Response.GetResponseStream();
                        string responseBodyJsonString = string.Empty;

                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            responseBodyJsonString = sr.ReadToEnd();
                        }

                        JObject responseBodyJobject = (JObject)JObject.Parse(responseBodyJsonString);
                        string message = (string)responseBodyJobject["message"];

                        MessageBox.Show(message, "Lolien Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }));
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                        int size, string filePath);
    }
}