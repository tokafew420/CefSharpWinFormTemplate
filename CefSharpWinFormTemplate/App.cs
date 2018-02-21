using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Tokafew420.CefSharpWinFormTemplate
{
    internal class App
    {
        private Form _form;
        private ChromiumWebBrowser _browser;
        private SystemEvent _systemEvent;
        private ScriptEvent _scriptEvent;

        internal App(Form form, ChromiumWebBrowser browser, SystemEvent systemEvent, ScriptEvent scriptEvent)
        {
            _form = form ?? throw new ArgumentNullException("form");
            _browser = browser ?? throw new ArgumentNullException("browser");
            _systemEvent = systemEvent ?? throw new ArgumentNullException("systemEvent");
            _scriptEvent = scriptEvent ?? throw new ArgumentNullException("scriptEvent");

            // Register event handlers
            _scriptEvent.On("test", (args) => OnTest(args as object[]));
            _scriptEvent.On("multi-args", (args) => OnExample1(args as object[]));
            _scriptEvent.On("complex-arg", (args) => OnExample2(args as object[]));
            _scriptEvent.On("long-running-task", (args) => OnExample3(args as object[]));
        }

        internal void OnTest(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0] as string))
            {
                _systemEvent.Emit("reply", "Hello " + args[0]);
            }
            else
            {
                _systemEvent.Emit("reply", "You didn't enter your name!!");
            }
        }

        internal void OnExample1(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            var result = "Length: " + args.Length;

            foreach (var obj in args)
            {
                result += "; Type: " + (obj == null ? "null" : obj.GetType().ToString());
                result += "; Value: " + obj;
            }

            _systemEvent.Emit("multi-args-reply", result, 1, -1.1, false);
        }

        internal void OnExample2(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            var result = "Empty";
            if (args != null && args.Length > 0)
            {
                var expando = args[0] as IDictionary<string, object>;

                if (expando != null)
                {
                    result = "Length: " + expando.Count;

                    foreach (var prop in expando)
                    {
                        result += "; Name: " + prop.Key;
                        result += "; Type: " + (prop.Value == null ? "null" : prop.Value.GetType().ToString());
                        result += "; Value: " + prop.Value;
                    }
                }
            }

            _systemEvent.Emit("complex-arg-reply", result, new
            {
                someInt = 314,
                someDbl = 3.14,
                someBool = true,
                someString = "pi"
            });
        }

        private static int  callCount = 0;
        internal void OnExample3(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            var result = "Call " + callCount++;

            // Do some work
            var sleepTime = (int)(new Random(DateTime.Now.Millisecond).NextDouble() * 10000);
            Thread.Sleep(sleepTime);

            _systemEvent.Emit("delayed-reply", result + $" Did some work for: {sleepTime} seconds.");
        }
    }
}