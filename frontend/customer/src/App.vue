<template>
  <div class="app">
    <el-container>
      <el-header class="header">
        <div class="logo" @click="$router.push('/')">✂ 理发店预约</div>
        <el-menu mode="horizontal" :default-active="$route.path" router class="nav">
          <el-menu-item index="/">首页</el-menu-item>
          <el-menu-item index="/orders">我的订单</el-menu-item>
          <el-menu-item index="/profile">个人中心</el-menu-item>
        </el-menu>
        <div class="user-area">
          <template v-if="user">
            <span class="nickname">{{ user.nickname }}</span>
            <el-button size="small" @click="logout">退出</el-button>
          </template>
          <template v-else>
            <el-button size="small" type="primary" @click="$router.push('/login')">登录</el-button>
          </template>
        </div>
      </el-header>
      <el-main><router-view /></el-main>
    </el-container>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";

const user = ref(null);

onMounted(() => {
  const u = localStorage.getItem("user");
  if (u) user.value = JSON.parse(u);
});

function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  user.value = null;
}
</script>

<style>
* { margin: 0; padding: 0; box-sizing: border-box; }
.header { display: flex; align-items: center; background: #fff; border-bottom: 1px solid #eee; padding: 0 20px; }
.logo { font-size: 20px; font-weight: bold; color: #409eff; cursor: pointer; margin-right: 30px; white-space: nowrap; }
.nav { flex: 1; border-bottom: none !important; }
.user-area { display: flex; align-items: center; gap: 10px; }
.nickname { color: #666; }
</style>
