using System;
using System.Collections.Generic;

namespace Logger {
    static class Program {
        static void Main() {
            Logger logger = new Logger();

            // Тестовые переменные для вызова исключения DivideByZeroException
            decimal x = 0;
            decimal y = 10;
            
            // Тестирование метода логирования Fatal
            logger.Fatal("This is <Fatal> logging");
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.Fatal("This is <Fatal> logging", e);
            }

            // Тестирование метода логирования Error
            logger.Error("This is <Error> logging");
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.Error("This is <Error> logging", e);
                logger.Error(e);
            }

            // Тестирование метода логирования ErrorUnique
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.ErrorUnique("This is <ErrorUnique> logging", e);
                logger.ErrorUnique("This is <ErrorUnique> logging", e);
                logger.ErrorUnique("This is <ErrorUnique> logging", e);
                logger.ErrorUnique("This is <ErrorUnique> logging", e);
            }
            
            // Тестирование метода логирования Warning
            logger.Warning("This is <Warning> logging");
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.Warning("This is <Warning> logging", e);
            }

            // Тестирование метода логирования WarningUnique
            logger.WarningUnique("This is <WarningUnique> logging");
            logger.WarningUnique("This is <WarningUnique> logging");
            logger.WarningUnique("This is <WarningUnique> logging");
            logger.WarningUnique("This is <WarningUnique> logging");

            // Тестирование метода логирования Info
            logger.Info("This is <Info> logging");
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.Info("This is <Error> logging", e);
            }

            logger.Info("This is <Info> logging", 25, 37.5, 75m);

            // Тестирование метода логирования Debug
            logger.Debug("This is <Debug> logging");
            try {
                Console.WriteLine(y / x);
            }
            catch (DivideByZeroException e) {
                logger.Debug("This is <Debug> logging", e);
            }

            // Тестирование метода логирования DebugFormat
            logger.DebugFormat("This is <DebugFormat> logging", 543, 0e33455, 332m);

            // Тестирование метода логирования SystemInfo
            logger.SystemInfo("This is <SystemInfo> logging", new Dictionary<object, object> {
                {"Property_1", "Value_1"},
                {"Property_2", "Value_2"},
                {"Property_3", "Value_3"}
            });
        }
    }
}