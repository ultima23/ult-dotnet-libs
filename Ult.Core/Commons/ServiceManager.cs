using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ServiceProcess;

namespace Ult.Commons
{
    public class ServiceManager
    {

        private const int SleepIntervall  = 25;
        
        private static object _sync = new object();

        // Events
        private static AutoResetEvent _waitPending = new AutoResetEvent(false);
        private static AutoResetEvent _waitStart = new AutoResetEvent(false);
        private static AutoResetEvent _waitStop = new AutoResetEvent(false);
        private static AutoResetEvent _waitRestart = new AutoResetEvent(false);


        private static void WaitPendingServiceHandler(object service)
        {
            ServiceController s = (ServiceController) service;
            try
            {
                while (s.Status == ServiceControllerStatus.StartPending || 
                       s.Status == ServiceControllerStatus.StopPending || 
                       s.Status == ServiceControllerStatus.PausePending || 
                       s.Status == ServiceControllerStatus.ContinuePending)
                {
                    Thread.Sleep(SleepIntervall);
                    s.Refresh();
                }
                _waitPending.Set();
            }
            catch (Exception)
            {
            }
        }

        private static void StartServiceHandler(object service)
        {
            ServiceController s = (ServiceController) service;
            try
            {
                while (s.Status != ServiceControllerStatus.Running)
                {
                    Thread.Sleep(SleepIntervall);
                    s.Refresh();
                }
                _waitStart.Set();
            }
            catch (Exception)
            {
            }
        }

        private static void StopServiceHandler(object service)
        {
            ServiceController s = (ServiceController) service;
            try
            {
                while (s.Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(SleepIntervall);
                    s.Refresh();
                }
                _waitStop.Set();
            }
            catch (Exception)
            {
            }
        }

        private static void RestartServiceHandler(object service)
        {
            ServiceController s = (ServiceController) service;
            try
            {
                // Stopping service
                if (s.Status != ServiceControllerStatus.Stopped)
                {
                    // Checks if the service is already starting
                    if (s.Status != ServiceControllerStatus.StopPending)
                    {
                        s.Stop();
                    }
                    while (s.Status != ServiceControllerStatus.Stopped)
                    {
                        Thread.Sleep(SleepIntervall);
                        s.Refresh();
                    }
                }
                // Restart the service
                s.Start();
                // wait for service running
                while (s.Status != ServiceControllerStatus.Running)
                {
                    Thread.Sleep(SleepIntervall);
                    s.Refresh();
                }
                _waitRestart.Set();
            }
            catch (Exception)
            {
            }
        }

        private static void WaitPendingService(ServiceController service)
        {
            if (IsPending(service))
            {
                Thread waiter = new Thread(WaitPendingServiceHandler);
                waiter.IsBackground = true;
                waiter.Start(service);
                _waitPending.WaitOne(Timeout.Infinite);
            }
        }

        private static ServiceControllerStatus GetServiceStatus(string name)
        {
            return GetService(name).Status;
        }

        public static ServiceController GetService(string name)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == name) return service;
            }
            throw new ServiceManagerException(String.Format("Service {0} not found", name));
        }

        private static bool IsPending(ServiceController service)
        {
            return service.Status == ServiceControllerStatus.StartPending ||
                   service.Status == ServiceControllerStatus.StopPending ||
                   service.Status == ServiceControllerStatus.ContinuePending ||
                   service.Status == ServiceControllerStatus.PausePending;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsPending(string name)
        {
            ServiceControllerStatus status = GetService(name).Status;
            return status == ServiceControllerStatus.StartPending ||
                   status == ServiceControllerStatus.StopPending ||
                   status == ServiceControllerStatus.ContinuePending ||
                   status == ServiceControllerStatus.PausePending;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsRunning(string name)
        {
            bool running = false;
            try
            {
                running = GetServiceStatus(name) == ServiceControllerStatus.Running;
            }
            catch (ServiceManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceManagerException(String.Format("Could not determine if service {0} is running, unexpected error: {1}", name, ex.Message), ex);
            }
            return running;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool Start(string name, int timeout)
        {
            bool success = false;
            try
            {
                lock (_sync)
                {
                    // Retrieve the service by name
                    ServiceController service = GetService(name);
                    // Wait to exit from a pending status
                    WaitPendingService(service);
                    // Check status
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        // Checks if the service is already starting
                        if (service.Status != ServiceControllerStatus.Paused)
                        {
                            // Start service
                            service.Start();
                        }
                        else
                        {
                            // Restart a paused service
                            service.Continue();
                        }
                        // 
                        Thread starter = new Thread(StartServiceHandler);
                        starter.IsBackground = true;
                        starter.Start(service);
                        // Wait service start
                        success = _waitStart.WaitOne(timeout);
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            catch (ServiceManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceManagerException(String.Format("Could not start service {0}, unexpected error: {1}", name, ex.Message), ex);
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool Stop(string name, int timeout)
        {
            bool success = false;
            try
            {
                lock (_sync)
                {
                    // Retrieve the service by name
                    ServiceController service = GetService(name);
                    // Wait to exit from a pending status
                    WaitPendingService(service);
                    // Check status
                    if (service.Status != ServiceControllerStatus.Stopped)
                    {
                        // Stops the service
                        service.Stop();
                        // 
                        Thread starter = new Thread(StopServiceHandler);
                        starter.IsBackground = true;
                        starter.Start(service);
                        // Wait service start
                        success = _waitStop.WaitOne(timeout);
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            catch (ServiceManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceManagerException(String.Format("Could not stop service {0}, unexpected error: {1}", name, ex.Message), ex);
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool Restart(string name, int timeout)
        {
            bool success = false;
            try
            {
                // Retrieve the service by name
                ServiceController service = GetService(name);
                // Wait to exit from a pending status
                WaitPendingService(service);
                // 
                Thread starter = new Thread(RestartServiceHandler);
                starter.IsBackground = true;
                starter.Start(service);
                // Wait service restart
                success = _waitRestart.WaitOne(timeout);
            }
            catch (ServiceManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceManagerException(String.Format("Could not stop service {0}, unexpected error: {1}", name, ex.Message), ex);
            }
            return success;
        }
        
    }


    public class ServiceManagerException : Exception
    {
        public ServiceManagerException() : base() {}
        public ServiceManagerException(string message) : base(message) {}
        public ServiceManagerException(string message, Exception cause) : base(message, cause) {}
    }

}
