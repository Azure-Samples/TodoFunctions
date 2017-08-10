/*jshint quotmark:false */
/*jshint white:false */
/*jshint trailing:false */
/*jshint newcap:false */
var app = app || {};

(function () {
    'use strict';

    var Utils = app.Utils;
    // Generic "model" object. You can use whatever
    // framework you want. For this application it
    // may not even be worth separating this logic
    // out, but we do this to demonstrate one way to
    // separate out parts of your application.
    app.TodoModel = function (key) {
        this.key = key;
        this.todos = Utils.store(key);
        this.onChanges = [];
        this.todoClient = new TodoClient();
        this.todoClient.getList(true, true, function (err, data) {
            if (!err) {
                this.todos = data.map(function (d) {
                    return {
                        id: d.id,
                        title: d.title,
                        completed: d.isComplete
                    };
                });
                this.inform();
            }
        }.bind(this));
    };

    // ???
    app.TodoModel.prototype.subscribe = function (onChange) {
        this.onChanges.push(onChange);
    };

    app.TodoModel.prototype.inform = function () {
        Utils.store(this.key, this.todos);
        this.onChanges.forEach(function (cb) { cb(); });
    };

    // Create
    app.TodoModel.prototype.addTodo = function (title) {
        var todo = {
            id: Utils.uuid(),
            title: title,
            completed: false
        };
        this.todoClient.create(todo, function (err, data) {
            if (!err) {
                this.todos = this.todos.concat(todo);
                this.inform();
            }
        }.bind(this));
    };

    // Toggle all
    // return Utils.extend({}, todo, { completed: checked }); will notwork
    // inside an error check.
    app.TodoModel.prototype.toggleAll = function (checked) {
        // Note: it's usually better to use immutable data structures since they're
        // easier to reason about and React works very well with them. That's why
        // we use map() and filter() everywhere instead of mutating the array or
        // todo items themselves.
        this.todos = this.todos.map(function (todo) {
            this.todoClient.setIsComplete(todo.id, checked, function (err, data) { });
            return Utils.extend({}, todo, { completed: checked });
        }.bind(this));
        this.inform();
    };

    // Complete/Uncomplete
    app.TodoModel.prototype.toggle = function (todoToToggle) {
        this.todoClient.setIsComplete(todoToToggle.id, !todoToToggle.completed, function (err, data) {
            if (!err) {
                this.todos = this.todos.map(function (todo) {
                    return todo !== todoToToggle ?
                        todo :
                        Utils.extend({}, todo, { completed: !todo.completed });

                });
                this.inform();
            }
        }.bind(this));
    };


    // Deletes
    app.TodoModel.prototype.destroy = function (todo) {
        this.todoClient.delete(todo.id, function (err, data) {
            if (!err) {
                this.todos = this.todos.filter(function (candidate) {
                    return candidate !== todo;
                });
                this.inform();
            }
        }.bind(this));
    };

    // Updates
    app.TodoModel.prototype.save = function (todoToSave, text) {
        this.todoClient.update(todoToSave, function (err, data) {
            if (!err) {
                this.todos = this.todos.map(function (todo) {
                    return todo !== todoToSave ? todo : Utils.extend({}, todo, { title: text });
                });
                this.inform();
            }
        }.bind(this));
    };


    // Delete this or implement this?
    app.TodoModel.prototype.clearCompleted = function () {
        this.todos = this.todos.filter(function (todo) {
            return !todo.completed;
        });

        this.inform();
    };

})();
