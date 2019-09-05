using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using ZenProgramming.Chakra.Core.Diagnostic;

namespace Common.Core.Diagnostics
{
    /// <summary>
    /// Tracer for Log4Net
    /// </summary>
    public class SerilogTracer : ITracer
    {
        #region Private fields

        private ILogger _Log;

        #endregion

        private ILogger GetLogger()
        {
            //Se la variabile statica è nulla, la inizializzo
            if (_Log == null)
            {
                //Nome del file di log
                var file = Assembly.GetEntryAssembly().GetName().Name + "-{HalfHour}.log";
                var format = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", file);

                //Inizializzazione
                _Log = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(

                        //Livello di log
                        restrictedToMinimumLevel: LogEventLevel.Information
                    )
                    .WriteTo.RollingFile(

                        //File di uscita
                        filePath, 

                        //Template di uscita
                        outputTemplate: format,

                        //Livello di log
                        restrictedToMinimumLevel: LogEventLevel.Verbose,
                        
                        //Numero massimo di files conservati: 1000
                        retainedFileCountLimit: 100,

                        //Dimensione massima file, 1MB
                        fileSizeLimitBytes: 1024 * 1000)
                    .CreateLogger();

                //Trace iniziale
                _Log.Information("Serilog provider initialized...");
            }

            //Ritorno il logger
            return _Log;
        }
        
        /// <summary>
        /// Format for trace
        /// </summary>
        public string TraceFormat { get; set; }

        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="formatParams">Format parameters</param>
        public void Info(string message, params object[] formatParams)
        {
            //Utilizzo log4net per la scrittura del log
            GetLogger().Information(message, formatParams);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="formatParams">Format parameters</param>
        public void Error(string message, params object[] formatParams)
        {
            //Utilizzo log4net per la scrittura del log
            GetLogger().Error(message, formatParams);
        }

        /// <summary>
        /// Trace a warning message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="formatParams">Format parameters</param>
        public void Warn(string message, params object[] formatParams)
        {
            //Utilizzo log4net per la scrittura del log
            GetLogger().Warning(message, formatParams);
        }

        /// <summary>
        /// Trace a debug message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="formatParams">Format parameters</param>
        public void Debug(string message, params object[] formatParams)
        {
            //Utilizzo log4net per la scrittura del log
            GetLogger().Debug(message, formatParams);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //Nessuna risorsa da rilasciare
            //in questa implementazione
        }
    }
}
