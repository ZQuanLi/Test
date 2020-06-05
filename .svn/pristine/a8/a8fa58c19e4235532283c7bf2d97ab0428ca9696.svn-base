using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DataProcess.Business
{
    public class m2mqtt
    {
        /// <summary>
        ///  发布主题
        /// </summary>
        /// <param name="opsCar"></param>
        public static void Publish(string opsCar)
        {
            uPLibrary.Networking.M2Mqtt.MqttClient client = new MqttClient("127.0.0.1");
            client.Connect(Guid.NewGuid().ToString());
            client.Publish("Yds6000.AppAlarm", System.Text.Encoding.UTF8.GetBytes(opsCar), uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Subscribe()
        {
            uPLibrary.Networking.M2Mqtt.MqttClient client = new MqttClient("127.0.0.1");
            client.Connect(Guid.NewGuid().ToString());
            client.Subscribe(new string[] { "Yds6000.AppAlarm" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            //+=可以存储多个事件方法，这些方法称为订阅者
            client.MqttMsgPublishReceived += client_MqttMsgPublishTransferReceived;
            //client.Subscribe("Yds6000.AlarmTask", System.Text.Encoding.UTF8.GetBytes(opsCar), uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        }

        /// <summary>
        /// 传输文件订阅相应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_MqttMsgPublishTransferReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //处理接收到的消息
            string msg = System.Text.Encoding.Default.GetString(e.Message);

            //分割出文件名
            //string fileName = msg.Substring(msg.LastIndexOf(";") + 1, msg.Length - msg.LastIndexOf(";") - 1);
            ////分割出文件内容
            //byte[] fileContent = Encoding.UTF8.GetBytes(msg.Substring(0, msg.LastIndexOf(";")));
            ////文件路径
            //string filePath = @"D:\" + fileName;
            ////写入文件
            //using (FileStream fsWrite = new FileStream(filePath, FileMode.Append))
            //{
            //    fsWrite.Write(fileContent, 0, fileContent.Length);
            //}
            //SqlHelper sqlHelper = new SqlHelper(AppSettings.CenterDatabaseConnStr);
            ////更新数据库，修改字段issend为true
            //string sql = string.Format("UPDATE DeviceFileTransfer set IsSend = 'true', SendTime = '{0}' where SerialNumber = '{1}'", DateTime.Now.ToString(), serialNumber);
            //int count = sqlHelper.ExecuteSql(sql);
            //if (count > 0)
            //{
            //    LogHelper.Info("文件传输成功！");
            //}
        }
//————————————————
//版权声明：本文为CSDN博主「SurperCT」的原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/SurperCT/article/details/95483826
    }
}
