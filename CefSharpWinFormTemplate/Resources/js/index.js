(function (window) {
    // Register script event object that interops with the .Net side.
    (async function () {
        await CefSharp.BindObjectAsync("scriptEvent");
    })();

    // Event Emitter polyfill if needed
    // https://gist.github.com/mudge/5830382
    if (typeof window.EventEmitter !== 'function') {
        var EventEmitter = window.EventEmitter = class EventEmitter {
            constructor() {
                this.events = [];
            }
            on(event, listener) {
                if (typeof this.events[event] !== 'object') {
                    this.events[event] = [];
                }

                this.events[event].push(listener);
            }
            removeListener(event, listener) {
                var idx;

                if (typeof this.events[event] === 'object') {
                    idx = indexOf(this.events[event], listener);

                    if (idx > -1) {
                        this.events[event].splice(idx, 1);
                    }
                }
            }
            emit(event) {
                var i, listeners, length, args = [].slice.call(arguments, 1);

                if (typeof this.events[event] === 'object') {
                    listeners = this.events[event].slice();
                    length = listeners.length;

                    for (i = 0; i < length; i++) {
                        listeners[i].apply(this, args);
                    }
                }
            }
            once(event, listener) {
                this.on(event, function g() {
                    this.removeListener(event, g);
                    listener.apply(this, arguments);
                });
            }
        };
    }

    /**
     * Specialize implementation for system events.
     */
    class SystemEvent extends EventEmitter {
        /**
         * Extends the EventEmitter by attempting to parse the arguments to emit().
         *
         * @param {any} event The event name.
         * @param {...} args Any number of arguemnts.
         */
        emit(event) {
            var i, len, args = [].slice.call(arguments);

            len = args.length;

            for (i = 1; i < len; i++) {
                try {
                    args[i] = JSON.parse(args[i]);
                } catch (err) { }
            }

            super.emit.apply(this, args);
        }
    }

    // Define global system event emitter.
    window.systemEvent = new SystemEvent();
}(window));