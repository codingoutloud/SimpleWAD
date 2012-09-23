using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace MvcWebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Configure diagnostics
            // From: http://www.packtpub.com/article/digging-into-windows-azure-diagnostics-tutotial (sic),
            // need to add a diagnostics trace listener; (web.config is just for website tracing)
            Trace.Listeners.Add(new DiagnosticMonitorTraceListener());
            Trace.AutoFlush = true;
            Trace.TraceInformation("Error");

            // Set up to transfer to blob storage
            DiagnosticMonitorConfiguration diagnosticConfiguration = DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticConfiguration.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(10);
            // don't need; if not specificed, will use overall quota (which is 4 GB)            diagnosticConfiguration.Logs.BufferQuotaInMB = 10;

            // Add the startup task logs directory, so the startup logs will get transferred to storage
            var dirConfig = new DirectoryConfiguration();
            dirConfig.Container = "wad-startuplogs-container";
            dirConfig.Path = Path.Combine((new DirectoryInfo(".")).FullName, "startuplogs");
            Trace.TraceInformation("not sure why files not getting picked up: dirConfig = " + dirConfig.Path);
            diagnosticConfiguration.Directories.DataSources.Add(dirConfig);
            diagnosticConfiguration.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(10);

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticConfiguration);

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void Run()
        {
            while (true)
            {
                string when = DateTime.UtcNow.ToLongTimeString() + " UTC";
                Trace.TraceError("this is an error " + when);
                Trace.TraceWarning("this is a warning " + when);
                Trace.TraceInformation("this is an information " + when);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(17));
            }
            base.Run();
        }
    }
}
