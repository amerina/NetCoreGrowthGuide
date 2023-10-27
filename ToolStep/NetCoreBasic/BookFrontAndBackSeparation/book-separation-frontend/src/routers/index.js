import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    todos: []
  },
  mutations: {
    setTodos(state, todos) {
      state.todos = todos;
    },
    addTodo(state, todo) {
      state.todos.push(todo);
    }
  },
  actions: {
    async fetchTodos({ commit }) {
      const response = await axios.get('/api/todo/list');
      commit('setTodos', response.data);
    },
    async createTodo({ commit }, todo) {
      const response = await axios.post('/api/todo/create', todo);
      commit('addTodo', response.data);
    }
  },
  getters: {
    completedTodos(state) {
      return state.todos.filter(todo => todo.isCompleted);
    }
  }
});