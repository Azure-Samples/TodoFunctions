var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('angular2/core');
var store_1 = require('./services/store');
var TodoApp = (function () {
    function TodoApp(todoStore) {
        var _this = this;
        this.newTodoText = '';
        this.todoClient = new TodoClient();
        this.todoStore = todoStore;
        this.todos = todoStore.todos;
        this.todoClient.getList(true, true, function (err, data) {
            _this.todos = data.map(function (todo) {
                var newTodo = new store_1.Todo(todo.title);
                newTodo.completed = todo.isComplete;
                newTodo.id = todo.id;
                return newTodo;
            });
        });
    }
    TodoApp.prototype.stopEditing = function (todo, editedTitle) {
        todo.editing = false;
    };
    TodoApp.prototype.cancelEditingTodo = function (todo) {
        todo.editing = false;
    };
    TodoApp.prototype.updateEditingTodo = function (todo, editedTitle) {
        editedTitle = editedTitle.trim();
        if (editedTitle.length === 0) {
            this.remove(todo);
        }
        else {
            this.todoClient.update({
                id: todo.id,
                title: editedTitle
            }, function (err, data) {
                if (!err) {
                    todo.title = editedTitle;
                    todo.editing = false;
                }
            });
        }
    };
    TodoApp.prototype.editTodo = function (todo) {
        todo.editing = true;
    };
    TodoApp.prototype.removeCompleted = function () {
        var _this = this;
        var idsToDelete = this.todos.filter(function (t) { return t.completed; }).map(function (t) { return t.id; });
        this.todoClient.deleteList(idsToDelete, function (err, data) {
            if (!err) {
                _this.todos = _this.todos.filter(function (t) { return idsToDelete.indexOf(t.id) < 0; });
            }
        });
    };
    TodoApp.prototype.toggleCompletion = function (todo) {
        var newStatus = !todo.completed;
        this.todoClient.setIsComplete(todo.id, newStatus, function (err, data) {
            if (!err) {
                todo.completed = newStatus;
            }
        });
    };
    TodoApp.prototype.remove = function (todo) {
        var _this = this;
        this.todoClient.delete(todo.id, function (err, data) {
            if (!err) {
                _this.todos = _this.todos.filter(function (t) { return t !== todo; });
            }
        });
    };
    TodoApp.prototype.addTodo = function () {
        var _this = this;
        if (this.newTodoText.trim().length) {
            this.todoClient.create({
                // TODO: need to get the new id back
                title: this.newTodoText
            }, function (err, data) {
                if (!err) {
                    _this.todos.push(new store_1.Todo(_this.newTodoText));
                    _this.newTodoText = '';
                }
            });
        }
    };
    TodoApp = __decorate([
        core_1.Component({
            selector: 'todo-app',
            templateUrl: 'app/app.html'
        }),
        __metadata('design:paramtypes', [store_1.TodoStore])
    ], TodoApp);
    return TodoApp;
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.default = TodoApp;