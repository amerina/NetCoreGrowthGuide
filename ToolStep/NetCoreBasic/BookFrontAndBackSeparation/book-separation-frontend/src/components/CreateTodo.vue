<template>
    <div>
      <el-form ref="form" :model="todo" label-width="120px">
        <el-form-item label="Title">
          <el-input v-model="todo.title"></el-input>
        </el-form-item>
        <el-form-item label="Completed">
          <el-switch v-model="todo.isCompleted"></el-switch>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="createTodo">Create</el-button>
        </el-form-item>
      </el-form>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'CreateTodo',
    data() {
      return {
        todo: {
          title: '',
          isCompleted: false
        }
      };
    },
    methods: {
      async createTodo() {
        await axios.post('/api/todo', this.todo);
        this.$emit('created');
        this.$refs.form.resetFields();
      }
    }
  };
  </script>