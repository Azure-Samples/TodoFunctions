import { Component } from 'angular2/core';
import { TodoStore, Todo } from './services/store';

declare var TodoClient: any;

@Component({
    selector: 'todo-app',
    templateUrl: 'app/app.html'
})
export default class TodoApp {
    todoStore: TodoStore;
    todos: Array<Todo>;
    newTodoText = '';
    todoClient = new TodoClient();

    constructor(todoStore: TodoStore) {
        this.todoStore = todoStore;
        this.todos = todoStore.todos;
        this.todoClient.getList(true, true, (err: any, data: any) => {
            this.todos = data.map((todo: { id: String, title: String, isComplete: boolean }) => {
                const newTodo = new Todo(todo.title);
                newTodo.completed = todo.isComplete;
                newTodo.id = todo.id;
                return newTodo;
            });
        });
    }

    stopEditing(todo: Todo, editedTitle: string) {
        todo.editing = false;
    }

    cancelEditingTodo(todo: Todo) {
        todo.editing = false;
    }

    updateEditingTodo(todo: Todo, editedTitle: string) {
        editedTitle = editedTitle.trim();

        if (editedTitle.length === 0) {
            this.remove(todo);
        } else {
            this.todoClient.update({
                id: todo.id,
                title: editedTitle
            }, (err: any, data: any) => {
                if (!err) {
                    todo.title = editedTitle;
                    todo.editing = false;
                }
            });
        }
    }

    editTodo(todo: Todo) {
        todo.editing = true;
    }

    removeCompleted() {
        const idsToDelete: Array<String> = this.todos.filter((t: Todo) => t.completed).map((t: Todo) => t.id);
        this.todoClient.deleteList(idsToDelete, (err: any, data: any) => {
            if (!err) {
                this.todos = this.todos.filter((t: Todo) => idsToDelete.indexOf(t.id) < 0);
            }
        });
    }

    toggleCompletion(todo: Todo) {
        const newStatus = !todo.completed;
        this.todoClient.setIsComplete(todo.id, newStatus, (err: any, data: any) => {
            if (!err) {
                todo.completed = newStatus;
            }
        });
    }

    remove(todo: Todo) {
        this.todoClient.delete(todo.id, (err: any, data: any) => {
            if (!err) {
                this.todos = this.todos.filter(t => t !== todo);
            }
        });
    }

    addTodo() {
        if (this.newTodoText.trim().length) {
            this.todoClient.create({
                // TODO: need to get the new id back
                title: this.newTodoText
            }, (err: any, data: any) => {
                if (!err) {
                    this.todos.push(new Todo(this.newTodoText));
                    this.newTodoText = '';
                }
            });
        }
    }
}
