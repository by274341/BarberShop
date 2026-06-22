<template>
  <div class="login-page">
    <el-card class="login-card">
      <h2>发型师/店长登录</h2>
      <el-form :model="form" label-width="80px">
        <el-form-item label="用户名"><el-input v-model="form.username" /></el-form-item>
        <el-form-item label="密码"><el-input v-model="form.password" type="password" /></el-form-item>
        <el-form-item>
          <el-button type="primary" @click="login" :loading="loading">登录</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup>
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import api from "../api";

const router = useRouter();
const loading = ref(false);
const form = reactive({ username: "", password: "" });

async function login() {
  loading.value = true;
  try {
    const res = await api.post("/auth/login", form);
    localStorage.setItem("token", res.data.token);
    localStorage.setItem("user", JSON.stringify(res.data));
    const role = res.data.role;
    if (role === "Manager") window.location.href = "http://localhost:5175";
    else router.push("/today");
  } catch (e) {
    alert(e.response?.data?.message || "登录失败");
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.login-page { display: flex; justify-content: center; align-items: center; min-height: 80vh; }
.login-card { width: 400px; }
h2 { text-align: center; margin-bottom: 20px; }
</style>
