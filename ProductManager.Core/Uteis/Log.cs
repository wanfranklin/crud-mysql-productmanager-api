namespace ProductManager.Core.Uteis
{
    public static class Log
    {
        public static void Registrar(string mensagem, int id)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timeStamp} - {mensagem} - Id do Produto: {id}";
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "arquivo.log");
            
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logMessage);
            }

            List<string> logLines = File.ReadAllLines(logFilePath).ToList();
            logLines.Sort((x, y) => y.Split('-')[0].Trim().CompareTo(x.Split("-")[0].Trim()));
            //string[] sortLogLines = logLines.OrderByDescending(line => line.Split('-')[0].Trim()).ToArray();

            File.WriteAllLines(logFilePath, logLines);
        }
    }
}