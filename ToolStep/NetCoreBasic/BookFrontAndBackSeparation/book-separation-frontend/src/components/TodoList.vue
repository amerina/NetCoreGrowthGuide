<template>
  <div>
    <el-table :data="todos" class="todo-list">
      <el-table-column prop="title" label="Title"></el-table-column>
      <el-table-column prop="isCompleted" label="Completed"></el-table-column>
    </el-table>
  </div>
</template>

<style scoped>
.todo-list {
  width: 100%;
}

.el-table__body {
  max-height: 500px;
  overflow-y: auto;
}
</style>