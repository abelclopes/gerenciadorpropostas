
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using INFRAESTRUCTURE.Util;

namespace INFRAESTRUCTURE
{
    public class FileSystemErrorLog : IErrorLog
    {
        public async Task Write(string data)
        {
            var txt = DateTime.UtcNow + " - " + data + Environment.NewLine;

            await FileUtil.WriteText("Logs\\erros.txt", txt);
        }
    }
}