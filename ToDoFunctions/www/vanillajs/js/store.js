/*jshint eqeqeq:false */
(function (window) {
	'use strict';

	/**
	 * Creates a new client side storage object and will create an empty
	 * collection if no collection already exists.
	 *
	 * @param {string} name The name of our DB we want to use
	 * @param {function} callback Our fake DB uses callbacks because in
	 * real life you probably would be making AJAX calls
	 */
	function Store(TodoClient) {

		this.todoClient = TodoClient;

	}

	/**
	 * Finds items based on a query given as a JS object
	 *
	 * @param {object} query The query to match against (i.e. {foo: 'bar'})
	 * @param {function} callback	 The callback to fire when the query has
	 * completed running
	 *
	 * @example
	 * db.find({foo: 'bar', hello: 'world'}, function (data) {
	 *	 // data will return any items that have foo: bar and
	 *	 // hello: world in their properties
	 * });
	 */
	Store.prototype.find = function (query, callback) {
		if (!callback) {
			return;
		}

		this.todoClient.getList(true, true, function(err, todos){
			if(!err){
				todos.forEach(function(todo){
					todo = ConvertTodoFromTable(todo)
				});
				callback.call(this, todos.filter(function (todo) {
					for (var q in query) {
						if (query[q] !== todo[q]) {
							return false;
						}
					}
					return true;
				}));
			}
		});		
	};

	/**
	 * Will retrieve all data from the collection
	 *
	 * @param {function} callback The callback to fire upon retrieving data
	 */
	Store.prototype.findAll = function (callback) {
		callback = callback || function () {};
		this.todoClient.getList(true, true, function(err, data){
			if(!err){
				data.forEach(function(todo){
					todo = ConvertTodoFromTable(todo);
				});
				callback.call(this, data);
			}
		}.bind(this));
	};

	/**
	 * Will save the given data to the DB. If no item exists it will create a new
	 * item, otherwise it'll simply update an existing item's properties
	 *
	 * @param {object} updateData The data to save back into the DB
	 * @param {function} callback The callback to fire after saving
	 * @param {number} id An optional param to enter an ID of an item to update
	 */
	Store.prototype.save = function (updateData, callback, id) {
		console.log(updateData);
		console.log(callback);
		console.log(id);

		if (updateData.hasOwnProperty("completed")){
			this.todoClient.setIsComplete(id, updateData.completed, function(err, data){
				if(!err){
					this.todoClient.getList(true, true, function(err, data){
						if (!err){
							data.forEach(function(todo){
								todo = ConvertTodoFromTable(todo);
							});
							callback.call(this, data);
						}
					}.bind(this));
				}
			}.bind(this));
		}
		else if(updateData.hasOwnProperty("title")){
			this.todoClient.get(id, function(err, data){
				if(!err){
					data.title = updateData.title;
					this.todoClient.update(data, function(err, data){
						if(!err){
							this.todoClient.getList(true, true, function(err, data){
								if (!err){
									data.forEach(function(todo){
										todo = ConvertTodoFromTable(todo);
									});
									callback.call(this, data);
								}
							}.bind(this));
						}
					}.bind(this));
				}
			}.bind(this));
		}
	};

	/**
	 * Will remove an item from the Store based on its ID
	 *
	 * @param {number} id The ID of the item you want to remove
	 * @param {function} callback The callback to fire after saving
	 */
	Store.prototype.remove = function (id, callback) {
		console.log(id);
		this.todoClient.delete(id, function(err, data){
			if(!err){
				this.todoClient.getList(true, true, function(err, data){
					if (!err){
						data.forEach(function(todo){
							todo = ConvertTodoFromTable(todo);
						});
						callback.call(this, data);
					}
				}.bind(this));
			}
		}.bind(this));		
	};

	Store.prototype.create = function(todo, callback){
		todo.isComplete = todo.completed;
		this.todoClient.create(todo, function(err, data){
			if(!err){
				callback.call();
			}
		});
	}

	/**
	 * Will drop all storage and start fresh
	 *
	 * @param {function} callback The callback to fire after dropping the data
	 */
	Store.prototype.drop = function (callback) {

	};

	var ConvertTodoFromTable = function(todo){
		todo.completed = todo.isComplete;
		return todo;
	}

	var ConvertTodoToTable = function(todo){

	}

	// Export to window
	window.app = window.app || {};
	window.app.Store = Store;
})(window);
