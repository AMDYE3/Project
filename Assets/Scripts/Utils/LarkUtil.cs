using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
    public static class LarkUtil
    {
        private const string LARK_WEBHOOK = "https://open.feishu.cn/open-apis/bot/v2/hook/4e269d47-011a-4b0e-8b7d-68a69a6bcb95";
        private static readonly string DEVICE_NAME = Environment.MachineName;
        private static readonly string USER_NAME = Environment.UserName;
        private static readonly string OS_VERSION = SystemInfo.operatingSystem;
        
        public static void SendMessage(object message)
        {
            var msg = message == null ? "null" : message.ToString();
            var trace = new System.Diagnostics.StackTrace(true).ToString();
            var text = $"[{USER_NAME}] [{DEVICE_NAME}] [{OS_VERSION}]\n{msg}\n{trace}";
            _ = InternalSendMessage(text);
        }

        private static async Task InternalSendMessage(string message)
        {
            using var client = new HttpClient();

            Debug.Log($"Sending Lark Message\n{message}");

            var json = JsonConvert.SerializeObject(new
            {
                msg_type = "text",
                content = new { text = message }
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(LARK_WEBHOOK, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Lark Message Sent Successfully: " + responseString);
                }
                else
                {
                    Debug.LogError($"Failed To Send Lark Message: {response.StatusCode} - {responseString}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception While Sending Lark Message: " + ex.Message);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Debug/Send Lark Message")]
        private static void DebugSendMessage()
        {
            SendMessage("This is a debug message");
        } 
#endif
    }
}