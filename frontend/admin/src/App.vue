<template>
  <div class="app">
    <el-container style="height: 100vh">
      <el-aside width="220px" class="sidebar">
        <div class="logo">✂ 店长后台</div>
        <el-menu :default-active="$route.path" router background-color="#304156" text-color="#bfcbd9" active-text-color="#409eff">
          <el-menu-item index="/dashboard"><el-icon><DataAnalysis /></el-icon>工作台</el-menu-item>
          <el-menu-item index="/appointments"><el-icon><Calendar /></el-icon>预约管理</el-menu-item>
          <el-menu-item index="/stylists"><el-icon><User /></el-icon>发型师管理</el-menu-item>
          <el-menu-item index="/services"><el-icon><Goods /></el-icon>服务管理</el-menu-item>
          <el-menu-item index="/customers"><el-icon><UserFilled /></el-icon>顾客管理</el-menu-item>
          <el-menu-item index="/marketing"><el-icon><Present /></el-icon>营销工具</el-menu-item>
          <el-menu-item index="/statistics"><el-icon><TrendCharts /></el-icon>数据统计</el-menu-item>
        </el-menu>
      </el-aside>
      <el-container>
        <el-header class="topbar">
          <span>欢迎，{{ user?.nickname }}</span>
          <el-button size="small" @click="logout">退出</el-button>
        </el-header>
        <el-main><router-view /></el-main>
      </el-container>
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
.sidebar { background: #304156; overflow-y: auto; }
.sidebar .logo { height: 60px; line-height: 60px; text-align: center; color: #fff; font-size: 18px; font-weight: bold; border-bottom: 1px solid #4a5a6a; }
.topbar { display: flex; align-items: center; justify-content: flex-end; gap: 10px; background: #fff; border-bottom: 1px solid #eee; padding: 0 20px; }
</style>
