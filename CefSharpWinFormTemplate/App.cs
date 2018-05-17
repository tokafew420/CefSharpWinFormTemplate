using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Tokafew420.CefSharpWinFormTemplate
{
    /// <summary>
    /// An example of the application class.
    /// </summary>
    internal class App
    {
        private Form _form;
        private ChromiumWebBrowser _browser;
        private SystemEvent _systemEvent;
        private ScriptEvent _scriptEvent;

        /// <summary>
        /// Initalizes a new instance of App
        /// </summary>
        /// <param name="form"></param>
        /// <param name="browser"></param>
        /// <param name="systemEvent"></param>
        /// <param name="scriptEvent"></param>
        internal App(Form form, ChromiumWebBrowser browser, SystemEvent systemEvent, ScriptEvent scriptEvent)
        {
            _form = form ?? throw new ArgumentNullException("form");
            _browser = browser ?? throw new ArgumentNullException("browser");
            _systemEvent = systemEvent ?? throw new ArgumentNullException("systemEvent");
            _scriptEvent = scriptEvent ?? throw new ArgumentNullException("scriptEvent");

            // Register event handlers
            _scriptEvent.On("test", OnTest);
            _scriptEvent.On("multi-args", OnExample1);
            _scriptEvent.On("complex-arg", OnExample2);
            _scriptEvent.On("long-running-task", OnExample3);
        }

        /// <summary>
        /// Handles the "test" event.
        /// </summary>
        /// <param name="args">The event parameters.</param>
        /// <remarks>
        /// If a name was passord, then reply with "Hello {name}", otherwise reply with a message indicating no name.
        /// </remarks>
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

        /// <summary>
        /// Handles the "multi-args" event.
        /// </summary>
        /// <param name="args">The event parameters.</param>
        /// <remarks>
        /// Inspects the event parameters and responsd with a message indicating the type and value of each parameter.
        /// This is a test/example of receiving/sending a message with multiple parameters.
        /// </remarks>
        internal void OnExample1(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            var result = "Length: " + args.Length;

            foreach (var obj in args)
            {
                result += $@";
Type: {(obj == null ? "null" : obj.GetType().ToString())}
; Value: {obj}";
            }

            _systemEvent.Emit("multi-args-reply", result, 1, -1.1, false);
        }

        /// <summary>
        /// Handles the "complex-arg" event.
        /// </summary>
        /// <param name="args">The event parameters.</param>
        /// <remarks>
        /// Inspects the event parameter as a complex object and repsond with the objects properties (i.e. name, type, and value).
        /// This is a test/example of receiving/sending a message with a complex object.
        /// </remarks>
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
                        result += $@"
; Name: {prop.Key}
; Type: {(prop.Value == null ? "null" : prop.Value.GetType().ToString())};
; Value: {prop.Value}";
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

        private static int callCount = 0;

        /// <summary>
        /// Handles the "long-running-task" event.
        /// </summary>
        /// <param name="args">The event parameters.</param>
        /// <remarks>
        /// This is a test/example to demostrate the async nature (non-blocking) of the events.
        /// Note that callCount is used to show events completing out of order.
        /// </remarks>
        internal void OnExample3(object[] args)
        {
            Debug.WriteLine("Event received");
            Debug.WriteLine(args);

            var result = "Call " + callCount++;

            // Do some work
            var sleepTime = (int)(new Random(DateTime.Now.Millisecond).NextDouble() * 10000);
            Thread.Sleep(sleepTime);

            _systemEvent.Emit("delayed-reply", result + $" Did some work for: {sleepTime} milliseconds.");
        }
    }
}