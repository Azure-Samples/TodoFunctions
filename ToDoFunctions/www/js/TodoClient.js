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
            if (xhr.status === 200) {
                callback(null, JSON.parse(xhr.responseText));
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

    window.TodoClient = TodoClient;
}());
