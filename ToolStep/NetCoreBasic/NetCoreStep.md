### 1、Simple Empty Web Project

#### 1、创建 Empty Web应用项目

```powershell
dotnet new web -o BookEmptyWeb
```

#### 2、运行应用

```powershell
cd BookEmptyWeb
dotnet watch run
```

```
http://localhost:5000/
输出：
Hello World!
```

**`dotnet watch` 命令是一个文件观察程序。 当它检测到支持热重载的更改时，它会热重载指定的应用程序。 当它检测到不受支持的更改时，它会重启应用程序。 此过程对从命令行中进行快速迭代开发很有帮助。**

[dotnet watch 命令 - .NET CLI | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/core/tools/dotnet-watch)

#### 3、Startup





#### 4、Program







#### 参考：

[ASP.NET Core 入门 | Microsoft Learn](https://learn.microsoft.com/zh-cn/aspnet/core/getting-started/?view=aspnetcore-6.0&tabs=windows)

[DotNet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new)

------

### 2、简单的WebAPI项目

#### 1、创建Web API项目

```
dotnet new webapi -o BookWebAPI
```







### 3、简单的MVC项目

#### 1、创建WebMVC项目

```
dotnet new mvc -o BookMVC
```



### 4、简单的DDD项目

#### 1、使用Clean architecture结构

安装模版

```
dotnet new -i Ardalis.CleanArchitecture.Template
```

新建DDD项目

```
dotnet new clean-arch -o BookDDD
```



参考：

[Clean Architecture Solution Template: A starting point for Clean Architecture with ASP.NET Core](https://github.com/ardalis/cleanarchitecture)



### 5、简单的前后端分离项目

#### 后端:

#### Step 1: 创建.NET Core项目

```powershell
dotnet new webapi -n BookSeparation
```

#### Step 2: 安装NuGet packages

```powershell
cd BookSeparation
dotnet add package Microsoft.AspNetCore.Mvc
dotnet add package Microsoft.EntityFrameworkCore
```

#### Step 3: 创建数据库添加上下文

创建一个数据库，并添加适当的实体和数据上下文。

例如，在“TodoContext”数据上下文中创建一个新的“Todo”实体:

```powershell
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }
}
```

#### Step 4: 构建必要的API端点

```c#
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        return await _context.Todos.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }
}
```

#### 前端：

#### Step 1: 安装Vue.js和Vue CLI

安装Vue.js和Vue CLI。例如，使用命令行:

```powershell
npm install -g vue
npm install -g @vue/cli
```

#### Step 2: 创建一个新的Vue项目

使用Vue CLI创建一个新的Vue项目。例如，使用命令行:

```c#
vue create BookSeparationFrontend
```

#### Step 3: 安装Element UI

使用npm或yarn安装Element UI。例如，使用命令行:

```powershell
npm install element-ui
```

#### Step 4: 为前端构建必要的组件

为您的前端构建必要的组件，例如表单、表和模态窗口。例如，创建一个TodoList组件来显示待办事项列表:

```html
<template>
  <div>
    <el-table :data="todos">
      <el-table-column prop="title" label="Title"></el-table-column>
      <el-table-column prop="isCompleted" label="Completed"></el-table-column>
    </el-table>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'TodoList',
  data() {
    return {
      todos: []
    };
  },
  async created() {
    const response = await axios.get('/api/todo');
    this.todos = response.data;
  }
};
</script>
```

#### Step 5: 使用Axios或其他HTTP库对后端进行API调用

使用Axios或其他HTTP库对后端进行API调用。例如，使用Axios发出一个POST请求来创建一个新的Todo:

```html
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
```

#### Step 6: 使用Vuex或其他状态管理库来管理应用程序的状态

使用Vuex或其他状态管理库来管理应用程序的状态。例如，创建一个Vuex存储来跟踪待办事项列表:

```javascript
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
      const response = await axios.get('/api/todo');
      commit('setTodos', response.data);
    },
    async createTodo({ commit }, todo) {
      const response = await axios.post('/api/todo', todo);
      commit('addTodo', response.data);
    }
  },
  getters: {
    completedTodos(state) {
      return state.todos.filter(todo => todo.isCompleted);
    }
  }
});
```

#### Step7:使用CSS或预处理器(如Sass)为应用程序设计样式

使用CSS或预处理器(如Sass)为应用程序设计样式。例如，给你的TodoList组件添加一些样式:

```html
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
```

一旦构建了后端和前端，就可以分别部署它们，并使用HTTP请求在它们之间进行通信。这使得您的项目具有更好的可伸缩性和灵活性。











### 6、模块化系统







