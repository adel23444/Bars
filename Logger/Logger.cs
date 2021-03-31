using System;
using System.IO;
using System.Collections.Generic;

namespace Logger {
    /// <summary>
    /// Класс логирования, реализация интерфейса ILog.
    /// </summary>
    public class Logger : ILog {
        /// <value>Свойство представляет уникальные ошибки.</value>>
        private List<string> UniqueErrors { get; set; } = new List<string>();

        /// <value>Свойство представляет уникальные предупреждения.</value>>
        private List<string> UniqueWarnings { get; set; } = new List<string>();
        /// <value>Свойство для получения сегодняшней даты (без секунд). </value>
        private static string Today { get { return $"{ DateTime.Now:yyyy.MM.dd}"; } }
        /// <value>Свойство для получения сегодняшней даты с секундами. </value>
        private static string TodaySec { get { return $"{DateTime.Now:yyyy.MM.ddTHH:mm:ssK}"; } }
        /// <value>Свойство для получения текущей директории с логами. </value>
        private static string GetLogsDirectory { get { return $"Logs/{Today}/"; } }


        /// <summary>
        /// Метод проверяет существует ли директория с логами.
        /// </summary>
        /// <returns>Возвращает "true" если директории с логами не существует и "false" в противном случае.</returns>
        private static bool IsExist { get { return !Directory.Exists(GetLogsDirectory); } }
        
        /// <summary>
        /// Создает директорию с логами и обновляет список уникальных ошибок и предупреждений.
        /// </summary>
        private void UpdateLogsDirectory() {
            if (IsExist) {
                Directory.CreateDirectory(GetLogsDirectory);
                UniqueErrors.Clear();
                UniqueWarnings.Clear();
            }
        }

        /// <summary>
        /// Записывает сообщение лога в соотвествующий файл.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="level">Уровень логирования.</param>
        private void LoggerMessageWriter(string message, string level) {
            UpdateLogsDirectory();
            using (StreamWriter logFile = File.AppendText(GetLogsDirectory + $"{level}.log")) {
                logFile.WriteLine($"{TodaySec} | {level.ToUpper()} | {message}");
            }
        }

        /// <summary>
        /// Записывает сообщение лога и StackTrace исключения в соотвествующий файл.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="e">Исключение.</param>
        /// <param name="level">Уровень логирования.</param>
        private void LoggerMessageWithStackTraceWriter(string message, Exception e, string level) {
            UpdateLogsDirectory();
            using (StreamWriter logFile = File.AppendText(GetLogsDirectory + $"{level}.log")) {
                logFile.WriteLine($"{TodaySec} | {level.ToUpper()} | {message}");
                if (!string.IsNullOrEmpty(e.StackTrace)) logFile.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Записывает сообщение лога и его параметры в соответствующий файл.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="level">Уровень логирования.</param>
        /// <param name="args">Параметры.</param>
        private void LoggerMessageWithParamsWriter(string message, string level, params object[] args) {
            UpdateLogsDirectory();
            using (StreamWriter logFile = File.AppendText(GetLogsDirectory + $"{level}.log")) {
                logFile.WriteLine($"{TodaySec} | {level.ToUpper()} | {message}");
                if (args.Length != 0) logFile.WriteLine($"\tParams: {string.Join(", ", args)}");
            }
        }

        public void Fatal(string message) {
            LoggerMessageWriter(message, "Fatal");
        }

        public void Fatal(string message, Exception e) {
            LoggerMessageWithStackTraceWriter(message, e, "Fatal");
        }

        public void Error(string message) {
            LoggerMessageWriter(message, "Error");
        }

        public void Error(string message, Exception e) {
            LoggerMessageWithStackTraceWriter(message, e, "Error");
        }

        public void Error(Exception ex) {
            LoggerMessageWithStackTraceWriter(ex.Message, ex, "Error");
        }

        public void ErrorUnique(string message, Exception e) {
            if (!UniqueErrors.Contains(message) && !IsExist) {
                LoggerMessageWithStackTraceWriter(message, e, "Error_Unique");
                UniqueErrors.Add(message);
            }
        }

        public void Warning(string message) {
            LoggerMessageWriter(message, "warning");
        }

        public void Warning(string message, Exception e) {
            LoggerMessageWithStackTraceWriter(message, e, "Warning");
        }

        public void WarningUnique(string message) {
            if (!UniqueWarnings.Contains(message) && !IsExist) {
                LoggerMessageWriter(message, "Warning_Unique");
                UniqueWarnings.Add(message);
            }
        }

        public void Info(string message) {
            LoggerMessageWriter(message, "Info");
        }

        public void Info(string message, Exception e) {
            LoggerMessageWithStackTraceWriter(message, e, "Info");
        }

        public void Info(string message, params object[] args) {
            LoggerMessageWithParamsWriter(message, "Info", args);
        }

        public void Debug(string message) {
            LoggerMessageWriter(message, "Debug");
        }

        public void Debug(string message, Exception e) {
            LoggerMessageWithStackTraceWriter(message, e, "Debug");
        }

        public void DebugFormat(string message, params object[] args) {
            LoggerMessageWithParamsWriter(message, "Debug", args);
        }

        public void SystemInfo(string message, Dictionary<object, object> properties = null) {
            using (StreamWriter logFile = File.AppendText(GetLogsDirectory + "System_info.log")) {
                logFile.WriteLine($"{TodaySec} | SYSTEM INFO | {message}");
                if (properties != null) {
                    foreach (KeyValuePair<object, object> property in properties) {
                        logFile.WriteLine($"\t{property.Key} = {property.Value}");
                    }
                }
            }
        }
    }
}
