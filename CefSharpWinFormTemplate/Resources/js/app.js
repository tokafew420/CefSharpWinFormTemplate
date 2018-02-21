// Simple example
function hello() {
    console.log("Testing emitting script event to system.");
    // Sending multiple arguments
    scriptEvent.emit("test", $('#your-name').val());
};

// Test emitting multiple arguments
function example1() {
    console.log("Testing emitting script event to system with multiple arguments.");
    // Sending multiple arguments
    scriptEvent.emit("multi-args", true, 1, -1.0, 'hello');
};

// Testing emitting complex object
function example2() {
    console.log("Testing emitting script event to system with complex object.");
    // Sending complex object
    scriptEvent.emit("complex-arg", {
        isTrue: true,
        isNumber: 1,
        anotherNumber: -1.1,
        string: "hello"
    });
};

// Testing long running task
function example3() {
    console.log("Testing emitting script event to system with long running task.");
    // Sending complex object
    scriptEvent.emit("long-running-task");
};

systemEvent.on('reply', function (greeting) {
    console.debug('Receive system event: reply');

    $('#system-messages').text(greeting);
});

systemEvent.on('multi-args-reply', function () {
    var args = Array.prototype.slice.apply(arguments);

    console.debug('Receive system event: multi-args-reply');
    console.debug(args);

    $('#system-messages').html('Receive system event: multi-args-reply<br/>args: ' + JSON.stringify(args));
});

systemEvent.on('complex-arg-reply', function () {
    var args = Array.prototype.slice.apply(arguments);

    console.debug('Receive system event: complex-arg-reply');
    console.debug(args);

    $('#system-messages').html('Receive system event: complex-arg-reply<br/>args: ' + JSON.stringify(args));
});

systemEvent.on('delayed-reply', function (message) {
    console.debug('Receive system event: complex-arg-reply');

    $('#system-messages').html('Receive system event: delayed-reply<br/>' + message);
});