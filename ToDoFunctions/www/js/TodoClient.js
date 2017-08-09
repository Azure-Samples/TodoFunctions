(function () {
    var TodoClient = function (baseUrl) {
        this.baseUrl = baseUrl || '';
    };

    TodoClient.prototype.get = function (id, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', this.baseUrl + '/api/todos/' + id);
        xhr.onload = function () {
            if (xhr.status === 200) {
                callback(null, JSON.parse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send();
    };

    TodoClient.prototype.getList = function (includeCompleted, includeActive, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', this.baseUrl + '/api/todos?includecompleted=' + (includeCompleted === true) + '&includeactive=' + (includeActive === true));
        xhr.onload = function () {
            if (xhr.status === 200) {
                callback(null, JSON.parse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send();
    };

    TodoClient.prototype.create = function (todo, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', this.baseUrl + '/api/todos');
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200 || xhr.status === 201) {
                callback(null, safeJsonParse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send(JSON.stringify(todo));
    };

    TodoClient.prototype.update = function (todo, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('PUT', this.baseUrl + '/api/todos/' + todo.id);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                callback(null, JSON.parse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send(JSON.stringify(todo));
    };

    TodoClient.prototype.setIsComplete = function (id, isComplete, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('PATCH', this.baseUrl + '/api/todos/' + id);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                callback(null, JSON.parse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send(JSON.stringify({ isComplete: isComplete }));
    };

    TodoClient.prototype.delete = function (id, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open('DELETE', this.baseUrl + '/api/todos/' + id);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                callback(null, safeJsonParse(xhr.responseText));
            }
            else {
                callback(xhr.responseText);
            }
        };
        xhr.send();
    };

    TodoClient.prototype.deleteList = function (ids, callback) {
        if (!ids.length) {
            callback(null, []);
        }

        var counter = 0;
        ids.forEach(function (id) {
            this.delete(id, done);
            counter++;
        }.bind(this));

        var statuses = [];
        function done(err, data) {
            counter--;
            statuses.push({
                err: err,
                data: data
            });
            if (counter === 0) {
                var allSucceeded = statuses.filter(function (s) { return err; }).length === 0;
                if (allSucceeded) {
                    callback(null, statuses);
                } else {
                    callback("At least one failure", statuses);
                }
            }
        }
    };

    function safeJsonParse(input) {
        try {
            return JSON.parse(input);
        } catch (e) {
        }
    }

    window.TodoClient = TodoClient;
}());
