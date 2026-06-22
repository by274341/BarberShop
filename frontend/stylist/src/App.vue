<template>
  <div class="app">
    <el-container>
      <el-header class="header">
        <div class="logo">✂ 发型师工作台</div>
        <el-menu mode="horizontal" :default-active="$route.path" router class="nav">
          <el-menu-item index="/today">今日预约</el-menu-item>
          <el-menu-item index="/calendar">预约日历</el-menu-item>
          <el-menu-item index="/schedule">我的排班</el-menu-item>
          <el-menu-item index="/records">服务记录</el-menu-item>
          <el-menu-item index="/stats">我的业绩</el-menu-item>
        </el-menu>
        <div class="user-area">
          <span class="nickname">{{ user?.nickname }}</span>
          <el-button size="small" @click="logout">退出</el-button>
        </div>
      </el-header>
      <el-main><router-view /></el-main>
    </el-container>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const user = ref(null);

onMounted(() => {
  const u = localStorage.getItem("user");
  if (u) user.value = JSON.parse(u);
  else router.push("/login");
});

function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  router.push("/login");
}
</script>

<style>
* { margin: 0; padding: 0; box-sizing: border-box; }
.header { display: flex; align-items: center; background: #fff; border-bottom: 1px solid #eee; padding: 0 20px; }
.logo { font-size: 20px; font-weight: bold; color: #67c23a; cursor: pointer; margin-right: 30px; white-space: nowrap; }
.nav { flex: 1; border-bottom: none !important; }
.user-area { display: flex; align-items: center; gap: 10px; }
.nickname { color: #666; }
</style>
